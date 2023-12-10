using System.Collections;

namespace AdventOfCode.Map;

public static class DirectionExtensions
{
    public static readonly IReadOnlyCollection<Direction> AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();

    public static Direction GetOpposite(this Direction d)
    {
        return d switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.UpRight => Direction.DownLeft,
            Direction.DownRight => Direction.UpLeft,
            Direction.DownLeft => Direction.UpRight,
            Direction.UpLeft => Direction.DownRight,
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }

    public static Direction RotateRight(this Direction d, int degrees)
    {
        if (degrees < 0)
            throw new ArgumentException("Degrees must be > 0°.");
        if (degrees % 45 != 0)
            throw new ArgumentException("Degrees must be set in 45° intervals.");

        for (int i = 0; i < degrees / 45; i++)
        {
            d = d switch
            {
                Direction.Up => Direction.UpRight,
                Direction.UpRight => Direction.Right,
                Direction.Right => Direction.DownRight,
                Direction.DownRight => Direction.Down,
                Direction.Down => Direction.DownLeft,
                Direction.DownLeft => Direction.Left,
                Direction.Left => Direction.UpLeft,
                Direction.UpLeft => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };
        }

        return d;
    }

    public static Direction RotateLeft(this Direction d, int degrees)
    {
        return d.RotateRight(360 - (degrees % 360));
    }

    public static (int col, int row) Move(this Direction d, (int col, int row) position)
    {
        switch (d)
        {
            case Direction.Up:
                return (position.col, position.row - 1);
            case Direction.Down:
                return (position.col, position.row + 1);
            case Direction.Left:
                return (position.col - 1, position.row);
            case Direction.Right:
                return (position.col + 1, position.row);
            case Direction.UpRight:
                return (position.col + 1, position.row - 1);
            case Direction.UpLeft:
                return (position.col - 1, position.row - 1);
            case Direction.DownRight:
                return (position.col + 1, position.row + 1);
            case Direction.DownLeft:
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
        return GetAdjacent(p, Direction.Up, Direction.Right, Direction.Down, Direction.Left);
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

    public static Direction GetDirection(this Point2D from, Point2D to)
    {
        var rowDiff = to.row - from.row;
        var columnDiff = to.column - from.column;
        if (rowDiff != 0 && columnDiff != 0 && columnDiff.Abs() != rowDiff.Abs())
            throw new InvalidOperationException("Cannot detect direction, not 45°");
        
        if (rowDiff == 0)
            return columnDiff > 0 ? Direction.Right : Direction.Left;

        if (columnDiff == 0)
            return rowDiff > 0 ? Direction.Down : Direction.Up;

        if (rowDiff > 0 && columnDiff > 0)
            return Direction.DownRight;
        if (rowDiff > 0 && columnDiff < 0)
            return Direction.DownLeft;
        if (rowDiff < 0 && columnDiff > 0)
            return Direction.UpRight;
        return Direction.UpLeft;
    }
}