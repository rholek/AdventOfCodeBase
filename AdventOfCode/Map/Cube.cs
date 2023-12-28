namespace AdventOfCode.Map;

public record Cube
{
    public int Width { get; }
    public int Depth { get; }
    public int Height { get; }

    public Point3D Origin { get; set; }

    public int Left => Origin.X;
    public int Near => Origin.Y;
    public int Bottom => Origin.Z;

    public int Right => Origin.X + Width;
    public int Far => Origin.Y + Depth;
    public int Top => Origin.Z + Height;

    public Cube(Point3D start, Point3D end)
    {
        Width = (start.X - end.X).Abs();
        Depth = (start.Y - end.Y).Abs();
        Height = (start.Z - end.Z).Abs();

        Origin = new Point3D(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z));
    }
    
    public bool Intersects(Cube otherCube)
    {
        return IntersectX() && IntersectY() && IntersectZ();

        bool IntersectX()
        {
            return !(otherCube.Left > Right || Left > otherCube.Right);
        }
        bool IntersectY()
        {
            return !(otherCube.Near > Far || Near > otherCube.Far);
        }
        bool IntersectZ()
        {
            return !(otherCube.Bottom > Top || Bottom > otherCube.Top);
        }
    }

}