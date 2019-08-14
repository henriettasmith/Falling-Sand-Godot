using Godot;
using System;
using System.Collections.Generic;

public class ParticleGrid
{
    public Dictionary<Vector2, Particle> grid = new Dictionary<Vector2, Particle>();
    public List<Particle> particles = new List<Particle>();

    public List<Vector2> updateLocations = new List<Vector2>();

    public int Width;
    public int Height;

    public ParticleGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void Update()
    {
        for(int i = 0; i < particles.Count; ++i)
        {
            if(particles[i].deathFlag || particles[i].newlyCreated)
            {
                continue;
            }

            if(inBounds(particles[i].position) == false)
            {
                particles[i].deathFlag = true;
                continue;
            }

            particles[i].Update();
        }

        for(int i = 0; i < particles.Count; ++i)
        {
            particles[i].LateUpdate();
            particles[i].newlyCreated = false;
        }
    }

    public void Refresh()
    {
        updateLocations.Clear();
        for(int i = 0; i < particles.Count; ++i)
        {
            if(particles[i].deathFlag)
            {
                particles.RemoveAt(i);
                --i;
            }
        }
    }

    public Particle GetParticle(Vector2 pos)
    {
        Particle ret;
        if(grid.TryGetValue(pos, out ret) == false)
        {
            ret = new Empty();
            ret.position = pos;
            ret.grid = this;
        }
        return ret;
    }

    private bool inBounds(Vector2 position)
    {
        return !(position.x < 0 || position.y < 0 || position.x >= Width || position.y >= Height);
    }

    private void SetParticle(Particle particle, Vector2 position)
    {
        if(particle.id() != ePARTICLE.EMPTY && inBounds(position))
            grid[position] = particle;
        else
            grid.Remove(position);
        particle.position = position;
    }

    public void AddParticle(Particle particle, Vector2 position)
    {
        Particle other = GetParticle(position);
        other.deathFlag = true;
        SetParticle(particle, position);
        if(particle.id() != ePARTICLE.EMPTY)
            particles.Add(particle);
        particle.grid = this;
        
        updateLocations.Add(position);
    }

    public void MoveParticle(Vector2 origin, Vector2 destination)
    {
        Particle particle = GetParticle(origin);
        Particle other = GetParticle(destination);
        other.deathFlag = true;
        grid.Remove(origin);
        SetParticle(particle, destination);

        updateLocations.Add(origin);
        updateLocations.Add(destination);
    }

    public void SwapParticles(Vector2 pos1, Vector2 pos2)
    {
        Particle p1 = GetParticle(pos1);
        Particle p2 = GetParticle(pos2);
        p1.position = pos2;
        p2.position = pos1;
        SetParticle(p1, pos2);
        SetParticle(p2, pos1);

        updateLocations.Add(pos1);
        updateLocations.Add(pos2);
    }

    public void RemoveParticle(Vector2 position)
    {
        Particle p = GetParticle(position);
        p.deathFlag = true;
        grid.Remove(position);

        updateLocations.Add(position);
    }
}
