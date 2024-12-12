namespace AdventOfCode.Map;

public static class MapExtensions
{
    public static Map<T> FlipVertically<T>(this Map<T> data)
    {
        var height = data.Height();
        return new Map<T>(data.ToDictionary(x => (x.Key.column, height - x.Key.row), x => x.Value));
    }

    public static Map<T> FlipHorizontally<T>(this Map<T> data)
    {
        var width = data.Width();
        return new Map<T>(data.ToDictionary(x => (width - x.Key.column, x.Key.row), x => x.Value));
    }

    public static Map<T> RotateLeft<T>(this Map<T> m)
    {
        var height = m.Height();
        return new Map<T>(m.ToDictionary(x => (x.Key.row, height - x.Key.column), x => x.Value));
    }

    public static IEnumerable<Map<T>> GetAllRotations<T>(this Map<T> map)
    {
        var m = map;
        yield return m;
        yield return m = m.RotateLeft();
        yield return m = m.RotateLeft();
        yield return m.RotateLeft();

        m = map.FlipVertically();
        yield return m;
        yield return m = m.RotateLeft();
        yield return m = m.RotateLeft();
        yield return m.RotateLeft();
    }

    public static List<T> GetEdge<T>(this Map<T> m, Edge edge)
    {
        switch (edge)
        {
            case Edge.Top:
                return m.Where(x => x.Key.row == 0).OrderBy(x => x.Key.column).Select(x => x.Value).ToList();
            case Edge.Left:
                return m.Where(x => x.Key.column == 0).OrderBy(x => x.Key.row).Select(x => x.Value).ToList();
            case Edge.Bottom:
                var h = m.Height();
                return m.Where(x => x.Key.row == h).OrderBy(x => x.Key.column).Select(x => x.Value).ToList();
            case Edge.Right:
                var w = m.Width();
                return m.Where(x => x.Key.column == w).OrderBy(x => x.Key.row).Select(x => x.Value).ToList();
            default:
                throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
        }
    }
    public static List<(int column, int row)> GetAllEdgePoints<T>(this Map<T> m)
    {
        return m.GetEdgePoints(Edge.Left)
            .Union(m.GetEdgePoints(Edge.Top))
            .Union(m.GetEdgePoints(Edge.Bottom))
            .Union(m.GetEdgePoints(Edge.Right)).ToList();
    }

    public static List<(int column, int row)> GetEdgePoints<T>(this Map<T> m, Edge edge)
    {
        switch (edge)
        {
            case Edge.Top:
                return m.Keys.Where(x => x.row == 0).OrderBy(x => x.column).ToList();
            case Edge.Left:
                return m.Keys.Where(x => x.column == 0).OrderBy(x => x.row).ToList();
            case Edge.Bottom:
                var h = m.Height();
                return m.Keys.Where(x => x.row == h).OrderBy(x => x.column).ToList();
            case Edge.Right:
                var w = m.Width();
                return m.Keys.Where(x => x.column == w).OrderBy(x => x.row).ToList();
            default:
                throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
        }
    }


    public static Map<TValue> Merge<TValue>(this Map<TValue> first, Map<TValue> second)
    {
        return Merge(first, second, (value, value1) => throw new Exception("No aggregation function defined"));
    }

    public static Map<TValue> Merge<TValue>(this Map<TValue> first, Map<TValue> second, Func<TValue, TValue, TValue> aggregateFunc)
    {
        var result = first.ToDictionary(x => x.Key, x => x.Value);
        foreach (var (key, value) in second)
        {
            if (result.ContainsKey(key))
                result[key] = aggregateFunc(result[key], value);
            else
                result[key] = value;
        }

        return new Map<TValue>(result);
    }

    public static IEnumerable<IReadOnlyCollection<Point2D>> SplitIntoRegions<TValue>(this Map<TValue> map)
    {
        var allPoints = map.Keys.ToList();
        while (allPoints.Any())
        {
            var region = new HashSet<Point2D>();
            GetRegion(allPoints.First(), region);
            allPoints.RemoveAll(region.Contains);
            yield return region;
        }

        void GetRegion(Point2D point, HashSet<Point2D> pointsInRegion)
        {
            var valueForPoint = map[point];
            if (!pointsInRegion.Add(point))
                return;

            foreach (var adjacentPoint in point.GetAdjacentWithoutDiagonal())
            {
                if (Equals(map[adjacentPoint], valueForPoint))
                    GetRegion(adjacentPoint, pointsInRegion);
            }
        }
    }
}