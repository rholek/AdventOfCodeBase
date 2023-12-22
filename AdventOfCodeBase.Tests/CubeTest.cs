namespace AdventOfCodeBase.Tests;

public class CubeTest
{
    [Theory]
    [InlineData("0,0,0~10,10,10", "5,5,5~6,6,6", true)]
    [InlineData("0,0,0~10,10,10", "0,0,0~10,10,10", true)]
    [InlineData("0,0,0~10,10,10", "20,20,20~10,10,10", true)]
    [InlineData("0,0,0~10,10,10", "20,0,0~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "0,20,0~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "0,0,20~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "20,0,20~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "20,20,0~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "0,20,20~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "20,20,20~11,11,11", false)]
    [InlineData("0,0,0~10,10,10", "0,20,20~30,30,30", false)]
    [InlineData("0,0,0~10,10,10", "20,0,20~30,30,30", false)]
    [InlineData("0,0,0~10,10,10", "20,20,0~30,30,30", false)]
    [InlineData("0,0,0~10,10,10", "15,15,15~60,60,60", false)]
    public void Cube_IntersectTest(string cubeDefinition1, string cubeDefinition2, bool intersects)
    {
        var cube1 = ParseCube(cubeDefinition1);
        var cube2 = ParseCube(cubeDefinition2);
        cube1.Intersects(cube2).Should().Be(intersects);
        cube2.Intersects(cube1).Should().Be(intersects);

        Cube ParseCube(string cubeString)
        {
            var start = cubeString.Split("~")[0].Split(",").Select(x => x.AsInt()).ToList();
            var end = cubeString.Split("~")[1].Split(",").Select(x => x.AsInt()).ToList();
            return new Cube(new MapPoint3D(start[0], start[1], start[2]), new MapPoint3D(end[0], end[1], end[2]));
        }
    }
}