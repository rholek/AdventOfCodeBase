namespace AdventOfCode.Map;

public sealed record Vector3(double X, double Y, double Z)
{
    public static implicit operator (double x, double y, double z)(Vector3 d) => (d.X, d.Y, d.Z);
    public static implicit operator Vector3((double x, double y, double z) d) => new(d.x, d.y, d.z);

    public static Vector3 operator +(Vector3 first, Vector3 second) => new(first.X + second.X, first.Y + second.Y, first.Z + second.Z);

    public static Vector3 operator -(Vector3 first, Vector3 second) => new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);

    public static Vector3 operator *(Vector3 first, double number) => new(first.X * number, first.Y * number, first.Z * number);

    public static Vector3 operator /(Vector3 first, double number) => new(first.X / number, first.Y / number, first.Z / number);
    
    public bool IsInteger => X % 1 == 0 && Y % 1 == 0 && Z % 1 == 0;

    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }
}