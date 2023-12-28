namespace AdventOfCode.Map;

public record Point3D(int X, int Y, int Z)
{
    public static implicit operator (int x, int y, int z)(Point3D d) => (d.X, d.Y, d.Z);
    public static implicit operator Point3D((int x, int y, int z) d) => new(d.x, d.y, d.z);

    public static Point3D operator +(Point3D first, Point3D second) => new(first.X + second.X, first.Y + second.Y, first.Z + second.Z);

    public static Point3D operator -(Point3D first, Point3D second) => new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);

    public static Point3D operator *(Point3D first, int num) => new(first.X * num, first.Y * num, first.Z * num);

    public static Point3D operator /(Point3D first, int num) => new(first.X / num, first.Y / num, first.Z / num);

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public IEnumerable<Point3D> GetAdjacentWithoutDiagonal()
    {
        yield return new(X - 1, Y, Z);
        yield return new(X + 1, Y, Z);
        yield return new(X, Y - 1, Z);
        yield return new(X, Y + 1, Z);
        yield return new(X, Y, Z - 1);
        yield return new(X, Y, Z + 1);
    }

    public int ManhattanDistance(Point3D point2)
    {
        return Math.Abs(X - point2.X) + Math.Abs(Y - point2.Y) + Math.Abs(Z - point2.Z);
    }


}