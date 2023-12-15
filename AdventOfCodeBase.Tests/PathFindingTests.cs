namespace AdventOfCodeBase.Tests;

public class PathFindingTests
{
    [Fact]
    public void PathFinding_GetCostTest()
    {
        var pathFinding = new PathFinding<int>(i => new[] { i - 1, i + 1 });
        pathFinding.GetCost(1, 100).Should().Be(99);
    }

    [Fact]
    public void PathFinding_GetPath()
    {
        var pathFinding = new PathFinding<int>(i => new[] { i - 1, i + 1 });
        var path = pathFinding.GetPath(1, 5);
        path.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 }, c => c.WithStrictOrdering());
    }

    [Theory]
    [InlineData("""
                #############
                #S..#..######
                ##.##.#######
                ##..........#
                ##.########.#
                ##E.........#
                #############
                """, 5)]
    [InlineData("""
                #############
                #S..#..######
                ##.##.#######
                ##.....E....#
                ##.########.#
                ##..........#
                #############
                """, 8)]
    public void PathFinding_GetCostMapTest(string mapString, long expectedCost)
    {
        var map = new Map<string>(mapString.SplitByLine());
        var pathFinding = new PathFinding<Point2D>(i => i.GetAdjacentWithoutDiagonal().Where(x => map[x] != "#"));
        pathFinding.GetCost((1, 1), x => map[x] == "E").Should().Be(expectedCost);
    }


    [Theory]
    [InlineData("""
                #############
                #000#00######
                ##0##0#######
                ##0040090000#
                ##0#0######0#
                ##0100#00000#
                #############
                """, 17)]
    [InlineData("""
                #############
                #000#00######
                ##0##0#######
                ##0#40090000#
                ##0#0######0#
                ##0100#00000#
                #############
                """, 19)]
    public void PathFinding_GetCostMapWithCostTest(string mapString, long expectedCost)
    {
        var map = new Map<string>(mapString.SplitByLine());
        var pathFinding = new PathFinding<Point2D>((s, t) => (map[s].AsLong() - map[t].AsLong()).Abs(), i => i.GetAdjacentWithoutDiagonal().Where(x => map[x] != "#"));
        pathFinding.GetCost((1, 1), x => map[x] == "9").Should().Be(expectedCost);
    }

    [Theory]
    [InlineData("""
                #############
                #000#00######
                ##0##0#######
                ##0040090000#
                ##0#0######0#
                ##0100#00000#
                #############
                """, new[] { "1,1", "2,1", "2,2", "2,3", "3,3", "4,3", "5,3", "6,3", "7,3" })]

    [InlineData("""
                #############
                #000#00######
                ##0##0#######
                ##0#40090000#
                ##0#0######0#
                ##0100#00000#
                #############
                """, new[] { "1,1", "2,1", "2,2", "2,3", "2,4", "2,5", "3,5", "4,5", "4,4", "5,3", "6,3", "7,3" })]
    public void PathFinding_GetPathMapWithCostTest(string mapString, string[] path)
    {
        var map = new Map<string>(mapString.SplitByLine());
        var pathFinding = new PathFinding<Point2D>((s, t) => (map[s].AsLong() - map[t].AsLong()).Abs(), i => i.GetAdjacentWithoutDiagonal().Where(x => map[x] != "#"));

        pathFinding.GetPath((1, 1), map.GetSinglePosition("9"))
            .Select(x => $"{x.column},{x.row}")
            .Should().ContainInOrder(path);
    }
}