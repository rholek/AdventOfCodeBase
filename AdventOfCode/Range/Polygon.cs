namespace AdventOfCode.Range;

public class Polygon
{
    public IReadOnlyList<Point2D> Vertices { get; }

    public long PerimeterIntegerPoints { get; }
    public long TotalArea { get; }
    public long AreaIntegerPoints { get; }

    public Polygon(IEnumerable<Point2D> vertices)
    {
        Vertices = vertices.ToList();

        long total = 0;
        for (var i = 0; i < Vertices.Count; i++)
        {
            //Shoelace formula
            total += CrossProduct(Vertices[i], Vertices[(i + 1) % Vertices.Count]);
            PerimeterIntegerPoints += Vertices[i].ManhattanDistance(Vertices[(i + 1) % Vertices.Count]);
        }
        
        TotalArea = (total.Abs() / 2);

        //Pick's theorem:
        //TotalArea = AreaIntegerPoints + PerimeterIntegerPoints/2 -1
        //Therefore:
        AreaIntegerPoints = TotalArea - PerimeterIntegerPoints / 2 + 1;

        long CrossProduct(Point2D start, Point2D end)
        {
            return start.column * ((long)end.row) - start.row * ((long)end.column);
        }
    }


}