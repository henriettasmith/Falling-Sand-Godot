using Godot;
using System.Collections.Generic;

public enum ePARTICLE{EMPTY = -1, SAND, WATER, PLANT, OIL, FIRE, WALL, AUTOMATA, AUTOMATA_OFF, GENERATOR}

public abstract class Particle
{
    public Vector2 position;
    public bool newlyCreated = true;
    public bool deathFlag = false;
    public ParticleGrid grid;
    public virtual void Update() {}
    public virtual void LateUpdate() {}
    public abstract ePARTICLE id();
    public abstract string color();

    public static Particle New(ePARTICLE id)
    {
        switch(id)
        {
            case ePARTICLE.WALL:
                return new Wall();
            case ePARTICLE.SAND:
                return new Sand();
            case ePARTICLE.WATER:
                return new Water();
            case ePARTICLE.OIL:
                return new Oil();
            case ePARTICLE.PLANT:
                return new Plant();
            case ePARTICLE.FIRE:
                return new Fire();
            case ePARTICLE.AUTOMATA:
            case ePARTICLE.AUTOMATA_OFF:
                return new Automata();
            default:
                return new Empty();
        }
    }
}
public class Empty : Particle
{
    public override void Update()
    {
        GD.Print("Empty Updated. Something is Wrong");
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.EMPTY;
    }

    public override string color()
    {
        GD.Print("Empty Colored. Something is Wrong");
        return "Black";
    }
}
public class Wall : Particle
{
    public override ePARTICLE id()
    {
        return ePARTICLE.WALL;
    }

    public override string color()
    {
        return "Gray";
    }
}
public class Sand : Particle
{
    public override void Update()
    {
        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.WATER:
            case ePARTICLE.OIL:
                grid.SwapParticles(position, down.position);
                break;
            default:
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.WATER:
            case ePARTICLE.OIL:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, left.position);
                break;
            default:
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.WATER:
            case ePARTICLE.OIL:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, right.position);
                break;
            default:
                break;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.SAND;
    }

    public override string color()
    {
        return "Tan";
    }
}
public class Water : Particle
{
    public override void Update()
    {
        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.OIL:
                grid.SwapParticles(position, down.position);
                break;
            default:
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.OIL:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, left.position);
                break;
            default:
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.EMPTY:
            case ePARTICLE.OIL:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, right.position);
                break;
            default:
                break;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.WATER;
    }

    public override string color()
    {
        return "Blue";
    }
}
public class Plant : Particle
{
    public override void Update()
    {
        Particle up = grid.GetParticle(position + Vector2.Up);
        switch(up.id())
        {
            case ePARTICLE.WATER:
                grid.AddParticle(Particle.New(ePARTICLE.PLANT),position + Vector2.Up);
                break;
            default:
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.WATER:
                grid.AddParticle(Particle.New(ePARTICLE.PLANT),position + Vector2.Left);
                break;
            default:
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.WATER:
                grid.AddParticle(Particle.New(ePARTICLE.PLANT),position + Vector2.Right);
                break;
            default:
                break;
        }

        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.WATER:
                grid.AddParticle(Particle.New(ePARTICLE.PLANT),position + Vector2.Down);
                break;
            default:
                break;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.PLANT;
    }

    public override string color()
    {
        return "Green";
    }
}
public class Oil : Particle
{
    public override void Update()
    {
        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.EMPTY:
                grid.SwapParticles(position, down.position);
                break;
            default:
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.EMPTY:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, left.position);
                break;
            default:
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.EMPTY:
                if(GD.Randf() < (double)1/3)
                    grid.SwapParticles(position, right.position);
                break;
            default:
                break;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.OIL;
    }

    public override string color()
    {
        return "Brown";
    }
}
public class Fire : Particle
{
    int life = 20;

    public override void Update()
    {
        Particle up = grid.GetParticle(position + Vector2.Up);
        switch(up.id())
        {
            case ePARTICLE.EMPTY:
                grid.SwapParticles(position, up.position);
                break;
            case ePARTICLE.FIRE:
                break;
            case ePARTICLE.PLANT:
            case ePARTICLE.OIL:
                grid.AddParticle(Particle.New(ePARTICLE.FIRE),position + Vector2.Up);
                break;
            default:
                grid.RemoveParticle(position);
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.EMPTY:
                if(GD.Randf() < (double)1/8)
                {
                    life -=2;
                    grid.SwapParticles(position, left.position);
                }
                break;
            case ePARTICLE.FIRE:
                break;
            case ePARTICLE.PLANT:
            case ePARTICLE.OIL:
                grid.AddParticle(Particle.New(ePARTICLE.FIRE),position + Vector2.Left);
                break;
            default:
                grid.RemoveParticle(position);
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.EMPTY:
                if(GD.Randf() < (double)1/8)
                {
                    life -=2;
                    grid.SwapParticles(position, right.position);
                }
                break;
            case ePARTICLE.FIRE:
                break;
            case ePARTICLE.PLANT:
            case ePARTICLE.OIL:
                grid.AddParticle(Particle.New(ePARTICLE.FIRE),position + Vector2.Right);
                break;
            default:
                grid.RemoveParticle(position);
                break;
        }

        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.PLANT:
            case ePARTICLE.OIL:
                grid.AddParticle(Particle.New(ePARTICLE.FIRE),position + Vector2.Down);
                break;
            default:
                break;
        }

        --life;
        if(life <= 0)
        {
            grid.RemoveParticle(position);
            return;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.FIRE;
    }

    public override string color()
    {
        if(life >= 17)
            return "White";
        if(life >= 14)
            return "Yellow";
        if(life >= 8)
            return "Orange";
        return "Red";
    }
}
public class Automata : Particle
{
    bool willActivate = false;
    bool active = true;

    public bool shouldActivate()
    {
        int count = 0;
        for(int x = -1; x <= 1; ++x)
        {
            for(int y = -1; y <= 1; ++y)
            {
                if(x == 0 && y == 0)
                    continue;
                Vector2 pos = position + new Vector2(x, y);
                Particle p = grid.GetParticle(pos);
                if(p.id() == ePARTICLE.AUTOMATA)
                {
                    ++count;
                }
            }
        }
        if(active && (count == 2 || count == 3))
        {
            return true;
        }
        else if(!active && count == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Update()
    {
        for(int x = -1; x <= 1; ++x)
        {
            for(int y = -1; y <= 1; ++y)
            {
                if(x == 0 && y == 0)
                    continue;
                Vector2 pos = position + new Vector2(x, y);
                Particle p = grid.GetParticle(pos);
                if(active && p.id() == ePARTICLE.EMPTY)
                {
                    Automata a = new Automata();
                    a.active = false;
                    grid.AddParticle(a, pos);
                    a.willActivate = a.shouldActivate();
                }
            }
        }

        willActivate = shouldActivate();

        //GD.Print(position, count, willActivate);
    }

    public override void LateUpdate()
    {
        active = willActivate;
        if(active == false)
        {
            grid.RemoveParticle(position);
        }
    }

    public override ePARTICLE id()
    {
        if(active)
            return ePARTICLE.AUTOMATA;
        return ePARTICLE.AUTOMATA_OFF;
    }

    public override string color()
    {
        if(active)
            return "Purple";
        return "Pink";
    }
}
public class Generator: Particle
{
    public ePARTICLE type = ePARTICLE.EMPTY;

    public override void Update()
    {
        Particle down = grid.GetParticle(position + Vector2.Down);
        switch(down.id())
        {
            case ePARTICLE.EMPTY:
                grid.AddParticle(Particle.New(type), down.position);
                break;
            default:
                break;
        }

        Particle left = grid.GetParticle(position + Vector2.Left);
        switch(left.id())
        {
            case ePARTICLE.EMPTY:
                grid.AddParticle(Particle.New(type), left.position);
                break;
            default:
                break;
        }

        Particle right = grid.GetParticle(position + Vector2.Right);
        switch(right.id())
        {
            case ePARTICLE.EMPTY:
                grid.AddParticle(Particle.New(type), right.position);
                break;
            default:
                break;
        }

        Particle up = grid.GetParticle(position + Vector2.Up);
        switch(up.id())
        {
            case ePARTICLE.EMPTY:
                grid.AddParticle(Particle.New(type), up.position);
                break;
            default:
                break;
        }
    }

    public override ePARTICLE id()
    {
        return ePARTICLE.GENERATOR;
    }

    public override string color()
    {
        return "Pink";
    }
}