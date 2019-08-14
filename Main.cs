using Godot;
using System;
using System.Collections.Generic;

public class Main : TileMap
{
    [Export]
    public int Width = 128;
    [Export]
    public int Height = 96;

    public ParticleGrid grid;
    public ePARTICLE particle = ePARTICLE.SAND;
    public int size = 1;
    public bool paused = false;
    public bool step = false;
    public bool gen = false;

    public bool mouseOver = false;

    public override void _Ready()
    {
        grid = new ParticleGrid(Width, Height);
    }

    public void MouseOver(bool mouseOver)
    {
        this.mouseOver = mouseOver;
    }

    public override void _Process(float delta)
    {
        if(mouseOver && Input.IsActionPressed("LeftClick"))
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();
            Vector2 gridPosition = WorldToMap(mousePosition);

            for(int x = (int)gridPosition.x - size/2; x <= (int)gridPosition.x + size/2; ++x)
            {
                for(int y = (int)gridPosition.y - size/2; y <= (int)gridPosition.y + size/2; ++y)
                {
                    Particle p;
                    if(gen)
                    {
                        Generator g = new Generator();
                        g.type = particle;
                        p = (Particle) g;
                    }
                    else
                    {
                        p = Particle.New(particle);
                    }

                    p.newlyCreated = false;
                    grid.AddParticle(p, new Vector2(x, y));
                    
                }
            }
        }
        else if(mouseOver && Input.IsActionPressed("RightClick"))
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();
            Vector2 gridPosition = WorldToMap(mousePosition);

            for(int x = (int)gridPosition.x - size/2; x <= (int)gridPosition.x + size/2; ++x)
            {
                for(int y = (int)gridPosition.y - size/2; y <= (int)gridPosition.y + size/2; ++y)
                {
                    grid.RemoveParticle(new Vector2(x, y));
                }
            }
        }

        if(paused == false || step)
        {
            grid.Update();
            step = false;
        }

        GetGridUpdate(grid.updateLocations);
        grid.Refresh();
    }

    public void SetParticle(int id)
    {
        particle = (ePARTICLE)id;
    }

    public void SetBrushSize(int size)
    {
        this.size = size;
    }

    public void TogglePause(bool paused)
    {
        this.paused = paused;
        step = false;
    }

    public void ToggleGen(bool gen)
    {
        this.gen = gen;
    }

    public void Step()
    {
        paused = true;
        step = true;
    }

    public void GetGridUpdate(List<Vector2> UpdatePositions)
    {
        foreach(Vector2 v in UpdatePositions)
        {
            SetTile(grid.GetParticle(v), v);
        }
    }

    public void SetTile(Particle particle, Vector2 position)
    {
        if(particle.id() == ePARTICLE.EMPTY)
            SetCellv(position, -1);
        else
            SetCellv(position, TileSet.FindTileByName(particle.color()));
    }
}
