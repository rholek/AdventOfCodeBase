using AdventOfCode.StandardAlgorithms;
using FluentAssertions;
using Xunit;

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



}
