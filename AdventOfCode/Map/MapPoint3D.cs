namespace AdventOfCode.Map;

public class MapPoint3D
{
    protected bool Equals(MapPoint3D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MapPoint3D)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public int X { get; }
    public int Y { get; }
    public int Z { get; }


    public MapPoint3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public int ManhattanDistance(MapPoint3D point2)
    {
        return Math.Abs(X - point2.X) + Math.Abs(Y - point2.Y) + Math.Abs(Z - point2.Z);
    }


    public static implicit operator (int x, int y, int z)(MapPoint3D d) => (d.X, d.Y, d.Z);
    public static implicit operator MapPoint3D((int x, int y, int z) d) => new(d.x, d.y, d.z);

    public static MapPoint3D operator +(MapPoint3D first, MapPoint3D second) => new(first.X + second.X, first.Y + second.Y, first.Z + second.Z);

    public static MapPoint3D operator -(MapPoint3D first, MapPoint3D second) => new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);

    public static bool operator ==(MapPoint3D first, MapPoint3D second) => Equals(first, second);

    public static bool operator !=(MapPoint3D first, MapPoint3D second) => !Equals(first, second);

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public IEnumerable<MapPoint3D> GetAdjacentWithoutDiagonal()
    {
        yield return new(X - 1, Y, Z);
        yield return new(X + 1, Y, Z);
        yield return new(X, Y - 1, Z);
        yield return new(X, Y + 1, Z);
        yield return new(X, Y, Z - 1);
        yield return new(X, Y, Z + 1);
    }

}