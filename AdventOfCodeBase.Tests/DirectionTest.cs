namespace AdventOfCodeBase.Tests;

public class DirectionTest
{
    [Theory]
    [InlineData(Direction.Left, Direction.Right)]
    [InlineData(Direction.Up, Direction.Down)]
    [InlineData(Direction.UpLeft, Direction.DownRight)]
    [InlineData(Direction.UpRight, Direction.DownLeft)]

    public void Direction_GetOppositeTest(Direction input, Direction result)
    {
        input.GetOpposite().Should().Be(result);
        result.GetOpposite().Should().Be(input);
    }

    [Theory]
    [InlineData(45, new[] { Direction.Up, Direction.UpRight, Direction.Right, Direction.DownRight, Direction.Down, Direction.DownLeft, Direction.Left, Direction.UpLeft })]
    [InlineData(90, new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left })]
    [InlineData(180, new[] { Direction.Up, Direction.Down })]
    [InlineData(180, new[] { Direction.Left, Direction.Right })]
    [InlineData(360, new[] { Direction.Up })]
    [InlineData(360, new[] { Direction.Right })]
    [InlineData(360, new[] { Direction.Down })]
    [InlineData(360, new[] { Direction.Left })]
    public void Direction_Rotate90Test(int degrees, Direction[] order)
    {
        for (int i = 0; i < order.Length; i++)
            order[i].RotateRight(degrees).Should().Be(order[(i + 1) % order.Length]);

        order = order.Reverse().ToArray();
        for (int i = 0; i < order.Length; i++)
            order[i].RotateLeft(degrees).Should().Be(order[(i + 1) % order.Length]);
    }

    [Theory]
    [InlineData(Direction.Up, Edge.Top)]
    [InlineData(Direction.Right, Edge.Right)]
    [InlineData(Direction.Down, Edge.Bottom)]
    [InlineData(Direction.Left, Edge.Left)]
    [InlineData(Direction.UpRight, Edge.TopRight)]
    [InlineData(Direction.DownRight, Edge.BottomRight)]
    [InlineData(Direction.UpLeft, Edge.TopLeft)]
    [InlineData(Direction.DownLeft, Edge.BottomLeft)]
    public void Direction_ToEdgeTest(Direction input, Edge result)
    {
        input.ToEdge().Should().Be(result);
        result.ToDirection().Should().Be(input);
    }

    [Theory]
    [InlineData(5, 5, Direction.Up, 5, 4)]
    [InlineData(5, 5, Direction.Right, 6, 5)]
    [InlineData(5, 5, Direction.Down, 5, 6)]
    [InlineData(5, 5, Direction.Left, 4, 5)]
    [InlineData(5, 5, Direction.UpRight, 6, 4)]
    [InlineData(5, 5, Direction.DownRight, 6, 6)]
    [InlineData(5, 5, Direction.DownLeft, 4, 6)]
    [InlineData(5, 5, Direction.UpLeft, 4, 4)]
    public void Direction_MoveTest(int columnIn, int rowIn, Direction direction, int columnOut, int rowOut)
    {
        (columnIn, rowIn).Move(direction).Should().Be((columnOut, rowOut));
    }

    [Fact]
    public void Direction_GetAdjacentTest()
    {
        (5, 5).GetAdjacent().Should().BeEquivalentTo(new[] { (4, 4), (4, 5), (6, 6), (6, 4), (4, 6), (5, 4), (5, 6), (6, 5) });
        (5, 5).GetAdjacentWithoutDiagonal().Should().BeEquivalentTo(new[] { (4, 5), (5, 4), (5, 6), (6, 5) });
    }
}