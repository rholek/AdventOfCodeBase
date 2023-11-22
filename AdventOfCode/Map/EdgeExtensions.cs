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

}