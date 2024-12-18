﻿namespace AdventOfCode.Map;

public static class DirectionExtensions
{
    public static readonly IReadOnlyCollection<Direction> AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
    public static readonly IReadOnlyCollection<Direction> MainDirections = [Direction.Up, Direction.Right, Direction.Down, Direction.Left];

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

    public static Point2D Move(this Direction d, Point2D position, int steps)
    {
        switch (d)
        {
            case Direction.Up:
                return (position.column, position.row - steps);
            case Direction.Down:
                return (position.column, position.row + steps);
            case Direction.Left:
                return (position.column - steps, position.row);
            case Direction.Right:
                return (position.column + steps, position.row);
            case Direction.UpRight:
                return (position.column + steps, position.row - steps);
            case Direction.UpLeft:
                return (position.column - steps, position.row - steps);
            case Direction.DownRight:
                return (position.column + steps, position.row + steps);
            case Direction.DownLeft:
                return (position.column - steps, position.row + steps);
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }

    public static Point2D Move(this Point2D position, Direction d)
    {
        return d.Move(position, 1);
    }

    public static Point2D Move(this Point2D position, Direction d, int steps)
    {
        return d.Move(position, steps);
    }

    public static IEnumerable<Point2D> GetAdjacent(this Point2D p)
    {
        return AllDirections.Select(direction => p.Move(direction));
    }

    public static IEnumerable<Point2D> GetAdjacent(this Point2D p, params Direction[] directions)
    {
        return directions.Select(direction => p.Move(direction));
    }

    public static IEnumerable<Point2D> GetAdjacentWithoutDiagonal(this Point2D p)
    {
        return GetAdjacent(p, Direction.Up, Direction.Right, Direction.Down, Direction.Left);
    }

    public static IEnumerable<Point2D> InMap<T>(this IEnumerable<Point2D> p, Map<T> map)
    {
        return p.Where(map.ContainsKey);
    }

    public static Point2D Offset(this Point2D source, Point2D offset)
    {
        return (source.column + offset.column, source.row + offset.row);
    }

    public static Point2D Offset(this Point2D source, int x, int y)
    {
        return Offset(source, (x, y));
    }

    public static int ManhattanDistance(this Point2D point, Point2D other)
    {
        return Math.Abs(point.column - other.column) + Math.Abs(point.row - other.row);
    }

    public static IEnumerable<Point2D> BetweenInclusive(this Point2D pointFrom, Point2D pointTo)
    {
        for (int c = Math.Min(pointFrom.column, pointTo.column); c <= Math.Max(pointFrom.column, pointTo.column); c++)
        {
            for (int r = Math.Min(pointFrom.row, pointTo.row); r <= Math.Max(pointFrom.row, pointTo.row); r++)
                yield return (c, r);
        }
    }

    public static Point2D GetOffsetRelativeTo(this Point2D from, Point2D to)
    {
        var rowDiff = from.row - to.row;
        var columnDiff = from.column - to.column;
        return (columnDiff, rowDiff);
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

    public static bool IsHorizontal(this Direction direction)
    {
        return direction is Direction.Left or Direction.Right;
    }
}