using AdventOfCode.Range;

namespace AdventOfCodeBase.Tests;

public class PolygonTest
{
    [Fact]
    public void Polygon_Test()
    {
        var polygon = new Polygon([(1, 1), (5, 1), (5, 5), (1, 5)]);
        polygon.PerimeterIntegerPoints.Should().Be(16);
        polygon.TotalArea.Should().Be(16);
        polygon.AreaIntegerPoints.Should().Be(9);
    }
}