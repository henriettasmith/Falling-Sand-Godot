using Godot;
using System;

public class Main : TileMap
{
    [Export]
    int currentParticle;

    bool canClick = true;

    public override void _Process(float delta)
    {
        if(Input.IsActionPressed("LeftClick") && canClick)
        {
            Vector2 selectedTile = WorldToMap(GetViewport().GetMousePosition());
            SetCell((int)selectedTile.x, (int)selectedTile.y, currentParticle);
        }
    }

    public void SetParticle(int index)
    {
        currentParticle = index;
    }

    public void setCanClick(bool set)
    {
        canClick = set;
    }
}
