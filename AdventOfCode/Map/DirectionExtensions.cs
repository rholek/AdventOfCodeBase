using System.Collections;

namespace AdventOfCode.Map;

public static class DirectionExtensions
{
    public static readonly IReadOnlyCollection<Direction> AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();

    public static Direction GetOpposite(this Direction d)
    {
        return d switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
            Direction.East => Direction.West,
            Direction.NorthEast => Direction.SouthWest,
            Direction.SouthEast => Direction.NorthWest,
            Direction.SouthWest => Direction.NorthEast,
            Direction.NorthWest => Direction.SouthEast,
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }

    public static Direction RotateRight(this Direction d, int degrees)
    {
        if (degrees % 45 != 0)
            throw new ArgumentException("Degrees must be set in 45° intervals.");

        return (Direction)(((int)d + (degrees / 45)) % 8);
    }

    public static Direction RotateLeft(this Direction d, int degrees)
    {
        return d.RotateRight(360 - (degrees % 360));
    }

    public static (int col, int row) Move(this Direction d, (int col, int row) position)
    {
        switch (d)
        {
            case Direction.North:
                return (position.col, position.row - 1);
            case Direction.South:
                return (position.col, position.row + 1);
            case Direction.West:
                return (position.col - 1, position.row);
            case Direction.East:
                return (position.col + 1, position.row);
            case Direction.NorthEast:
                return (position.col + 1, position.row - 1);
            case Direction.NorthWest:
                return (position.col - 1, position.row - 1);
            case Direction.SouthEast:
                return (position.col + 1, position.row + 1);
            case Direction.SouthWest:
                return (position.col - 1, position.row + 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }

    public static (int col, int row) Move(this (int col, int row) position, Direction d)
    {
        return d.Move(position);
    }

    public static (int col, int row) Move(this (int col, int row) position, Direction d, int steps)
    {
        for (int i = 0; i < steps; i++)
            position = d.Move(position);
        return position;
    }

    public static IEnumerable<(int col, int row)> GetAdjacent(this (int col, int row) p)
    {
        return AllDirections.Select(direction => p.Move(direction));
    }

    public static IEnumerable<(int col, int row)> GetAdjacent(this (int col, int row) p, params Direction[] directions)
    {
        return directions.Select(direction => p.Move(direction));
    }

    public static IEnumerable<(int col, int row)> GetAdjacentWithoutDiagonal(this (int col, int row) p)
    {
        return GetAdjacent(p, Direction.North, Direction.East, Direction.South, Direction.West);
    }

    public static IEnumerable<(int col, int row)> InMap<T>(this IEnumerable<(int col, int row)> p, Map<T> map)
    {
        return p.Where(map.ContainsKey);
    }

    public static (int col, int row) Offset(this (int col, int row) source, (int col, int row) offset)
    {
        return (source.col + offset.col, source.row + offset.row);
    }

    public static (int col, int row) Offset(this (int col, int row) source, int x, int y)
    {
        return Offset(source, (x, y));
    }

    public static int ManhattanDistance(this (int col, int row) point, (int col, int row) other)
    {
        return Math.Abs(point.col - other.col) + Math.Abs(point.row - other.row);
    }

    public static IEnumerable<(int col, int row)> BetweenInclusive(this (int col, int row) pointFrom, (int col, int row) pointTo)
    {
        for (int c = Math.Min(pointFrom.col, pointTo.col); c <= Math.Max(pointFrom.col, pointTo.col); c++)
        {
            for (int r = Math.Min(pointFrom.row, pointTo.row); r <= Math.Max(pointFrom.row, pointTo.row); r++)
                yield return (c, r);
        }
    }
 }