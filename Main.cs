using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public enum PARTICLE {EMPTY = -1, SAND = 0, WATER = 1, PLANT = 2, OIL = 3, FIRE1 = 4, FIRE2 = 5, FIRE3 = 6, FIRE4 = 7, WALL = 8, AUTOMATA = 9}

public abstract class Particle
{
    public abstract void update(int x, int y, Main grid);
}

public class Sand : Particle
{
    public override void update(int x, int y, Main grid)
    {
        if(grid.GetParticle(x, y+1) == PARTICLE.EMPTY)
        {
            grid.SetChange(x, y, PARTICLE.EMPTY);
            grid.SetChange(x, y+1, PARTICLE.SAND);
        }
        else if(grid.GetParticle(x, y+1) == PARTICLE.WATER)
        {
            grid.SetChange(x,y+1,PARTICLE.SAND);
        }
        else if(grid.GetParticle(x, y+1) == PARTICLE.OIL)
        {
            grid.SetChange(x, y+1, PARTICLE.SAND);
        }
        else if(new[] {PARTICLE.EMPTY, PARTICLE.WATER, PARTICLE.OIL}.Contains(grid.GetParticle(x + 1, y)) && GD.Randf() <= 1f/3f)
        {
            grid.SetChange(x, y, grid.GetParticle(x + 1, y));
            grid.SetChange(x + 1, y, PARTICLE.SAND);
        }
        else if(new[] {PARTICLE.EMPTY, PARTICLE.WATER, PARTICLE.OIL}.Contains(grid.GetParticle(x - 1, y)) && GD.Randf() <= 0.5f)
        {
            grid.SetChange(x, y, grid.GetParticle(x - 1, y));
            grid.SetChange(x - 1, y, PARTICLE.SAND);
        }
    }
}

public class Water : Particle
{
    public override void update(int x, int y, Main grid)
    {
        if(grid.GetParticle(x, y+1) == PARTICLE.EMPTY)
        {
            grid.SetChange(x, y, PARTICLE.EMPTY);
            grid.SetChange(x, y+1, PARTICLE.WATER);
        }
        else if(grid.GetParticle(x, y-1) == PARTICLE.SAND)
        {
            grid.SetChange(x,y-1,PARTICLE.WATER);
        }
        else if(grid.GetParticle(x, y+1) == PARTICLE.OIL)
        {
            grid.SetChange(x, y+1, PARTICLE.WATER);
        }
    }
}

public class Plant : Particle
{
    public override void update(int x, int y, Main grid)
    {

    }
}

public class Oil : Particle
{
    public override void update(int x, int y, Main grid)
    {
        if(grid.GetParticle(x, y+1) == PARTICLE.EMPTY)
        {
            grid.SetChange(x, y, PARTICLE.EMPTY);
            grid.SetChange(x, y+1, PARTICLE.OIL);
        }
        else if(grid.GetParticle(x, y-1) == PARTICLE.WATER)
        {
            grid.SetChange(x, y-1, PARTICLE.OIL);
        }
        else if(grid.GetParticle(x, y-1) == PARTICLE.SAND)
        {
            grid.SetChange(x, y-1, PARTICLE.OIL);
        }
    }
}

public class Fire : Particle
{
    public override void update(int x, int y, Main grid)
    {

    }
}

public class Wall : Particle
{
    public override void update(int x, int y, Main grid)
    {
    }
}

public class Automata : Particle
{
    public override void update(int x, int y, Main grid)
    {
    }
}

public struct ParticleBox
{
    public PARTICLE particle;
    public int x;
    public int y;

    public ParticleBox(int x, int y, PARTICLE p)
    {
        this.x = x;
        this.y = y;
        particle = p;
    }
}

public class Main : TileMap
{
    int floor = 385;
    int rightWall = 512;
    PARTICLE currentParticle = PARTICLE.SAND;
    bool canClick = true;
    bool play = true;
    int brushSize = 1;

    Dictionary<PARTICLE, Particle> particles = new Dictionary<PARTICLE, Particle>()
        {
            {PARTICLE.SAND, new Sand()},
            {PARTICLE.WATER, new Water()},
            {PARTICLE.PLANT, new Plant()},
            {PARTICLE.OIL, new Oil()},
            {PARTICLE.FIRE1, new Fire()},
            {PARTICLE.WALL, new Wall()},
            {PARTICLE.AUTOMATA, new Automata()}
        };

    SparseMatrix<ParticleBox> grid = new SparseMatrix<ParticleBox>();
    List<ParticleBox> changes = new List<ParticleBox>();

    public override void _Process(float delta)
    {
        if(Input.IsActionPressed("LeftClick") && canClick)
        {
            Vector2 selectedTile = WorldToMap(GetViewport().GetMousePosition());
            for(int x = (int)selectedTile.x - brushSize/2; x <= (int)selectedTile.x + brushSize/2; ++x)
            {
                for(int y = (int)selectedTile.y - brushSize/2; y <= (int)selectedTile.y + brushSize/2; ++y)
                {
                    ChangeCell(x, y ,currentParticle);
                }
            }
        }
        else if(Input.IsActionPressed("RightClick"))
        {
            Vector2 selectedTile = WorldToMap(GetViewport().GetMousePosition());
            for(int x = (int)selectedTile.x - brushSize/2; x <= (int)selectedTile.x + brushSize/2; ++x)
            {
                for(int y = (int)selectedTile.y - brushSize/2; y <= (int)selectedTile.y + brushSize/2; ++y)
                {
                    ChangeCell(x, y , PARTICLE.EMPTY);
                }
            }
        }

        if(play)
        {
            UpdateBoard();

            for(int i = changes.Count - 1; i >= 0; --i)
            {
                ChangeCell(changes[i].x, changes[i].y, changes[i].particle);
            }

            changes.Clear();
        }
    }

    public void UpdateBoard()
    {
        int c = 0;
        foreach(ParticleBox p in grid)
        {
            ++c;
            if(c % 2 == 0)
                particles[p.particle].update(p.x, p.y, this);
        }

        c = 0;
        foreach(ParticleBox p in grid)
        {
            ++c;
            if(c % 2 == 1)
                particles[p.particle].update(p.x, p.y, this);
        }
    }

    public void ChangeCell(int x, int y, PARTICLE id)
    {
        if(x < 0 || y < 0 || x > rightWall || y > floor)
            return;

        if(id == PARTICLE.EMPTY)
        {
            SetCell(x, y, -1);
            grid.RemoveAt(x, y);
        }
        else
        {
            SetCell(x,y, (int)id);
            grid.SetAt(x, y, new ParticleBox(x, y, id));
        }
    }

    public PARTICLE GetParticle(int x, int y)
    {
        return (PARTICLE)GetCell(x,y);
    }

    public void SetChange(int x, int y, PARTICLE change)
    {
        changes.Add(new ParticleBox(x, y, change));
    }

    public void SetParticle(int index)
    {
        currentParticle = (PARTICLE) index;
    }

    public void SetParticle(PARTICLE index)
    {
        currentParticle = index;
    }

    public void setCanClick(bool set)
    {
        canClick = set;
    }

    public void TogglePause(bool set)
    {
        play = !set;
    }

    public void SetBrushSize(int set)
    {
        brushSize = set;
    }
}
