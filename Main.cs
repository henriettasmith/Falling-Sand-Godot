using Godot;
using System;

public class Main : TileMap
{
    public void SetParticle(int id)
    {
        GD.Print(id);
    }

    public void SetBrushSize(int size)
    {
        GD.Print(size);
    }

    public void TogglePause(bool paused)
    {
        GD.Print(paused);
    }
}
