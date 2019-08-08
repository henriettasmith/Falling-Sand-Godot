using System.Collections.Generic;

public struct Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Position operator +(Position p1, Position p2)
    {
        return new Position(p1.x + p2.x, p1.y + p2.y);
    }
}

public enum PARTICLE{EMPTY = -1, SAND, WATER, PLANT, OIL, FIRE, WALL, AUTOMATA}
public enum DIRECTION{RIGHT, DOWN, LEFT, UP}

public abstract class Particle
{
    public Position position;
    public bool deathFlag = false;
    public abstract void Update(Dictionary<Position, Particle> grid);
    public abstract int id();

    public Particle[] Surroundings(Dictionary<Position, Particle> grid)
    {
        Particle[] ret = new Particle[4];

        //right
        if(grid.TryGetValue(new Position(position.x + 1, position.y), out ret[(int)DIRECTION.RIGHT]) == false)
            ret[(int)DIRECTION.RIGHT] = new Empty();
        
        //down
        if(grid.TryGetValue(new Position(position.x, position.y + 1), out ret[(int)DIRECTION.DOWN]) == false)
            ret[(int)DIRECTION.DOWN] = new Empty();
        
        //left
        if(grid.TryGetValue(new Position(position.x - 1, position.y), out ret[(int)DIRECTION.LEFT]) == false)
            ret[(int)DIRECTION.LEFT] = new Empty();
        
        //up
        if(grid.TryGetValue(new Position(position.x, position.y - 1), out ret[(int)DIRECTION.UP]) == false)
            ret[(int)DIRECTION.UP] = new Empty();
        
        return ret;
    }
}

public class Empty : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {
        return;
    }

    public override int id()
    {
        return -1;
    }
}

public class Sand : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {
        Particle[] surrounding = Surroundings(grid);
    }

    public override int id()
    {
        return 0;
    }
}

public class Water : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {

    }

    public override int id()
    {
        return 1;
    }
}

public class Plant : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {

    }

    public override int id()
    {
        return 2;
    }
}

public class Oil : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {

    }

    public override int id()
    {
        return 3;
    }
}

public class Fire : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {

    }

    public override int id()
    {
        return 4;
    }
}

public class Wall : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {
        return;
    }

    public override int id()
    {
        return 5;
    }
}

public class Automata : Particle
{
    public override void Update(Dictionary<Position, Particle> grid)
    {

    }

    public override int id()
    {
        return 6;
    }
}