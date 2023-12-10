namespace AdventOfCode.Map;

public static class EdgeExtensions
{
    public static Edge GetOpposite(this Edge e)
    {
        switch (e)
        {
            case Edge.Top:
                return Edge.Bottom;
            case Edge.Left:
                return Edge.Right;
            case Edge.Bottom:
                return Edge.Top;
            case Edge.Right:
                return Edge.Left;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }
    }

    public static Direction ToDirection(this Edge e)
    {
        return e switch
        {
            Edge.Top => Direction.Up,
            Edge.Left => Direction.Left,
            Edge.Bottom => Direction.Down,
            Edge.Right => Direction.Right,
            Edge.TopLeft => Direction.UpLeft,
            Edge.TopRight => Direction.UpRight,
            Edge.BottomLeft => Direction.DownLeft,
            Edge.BottomRight => Direction.DownRight,
            _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
        };
    }

    public static Edge ToEdge(this Direction e)
    {
        return e switch
        {
            Direction.Up => Edge.Top,
            Direction.UpRight => Edge.TopRight,
            Direction.Right => Edge.Right,
            Direction.DownRight => Edge.BottomRight,
            Direction.Down => Edge.Bottom,
            Direction.DownLeft => Edge.BottomLeft,
            Direction.Left => Edge.Left,
            Direction.UpLeft => Edge.TopLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(e), e, null)
        };
    }

}