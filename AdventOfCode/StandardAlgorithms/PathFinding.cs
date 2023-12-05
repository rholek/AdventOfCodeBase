namespace AdventOfCode.StandardAlgorithms;

public class PathFinding<T> where T : notnull
{
    private readonly Func<T, T, long> costFunction;
    private readonly Func<T, IEnumerable<T>> reachableFunction;

    public PathFinding(Func<T, IEnumerable<T>> reachableFunction) : this((_, _) => 1, reachableFunction) { }

    public PathFinding(Func<T, T, long> costFunction, Func<T, IEnumerable<T>> reachableFunction)
    {
        this.costFunction = costFunction;
        this.reachableFunction = reachableFunction;
    }

    public bool TryGetCost(T startPoint, T endPoint, out long totalCost)
    {
        return TryGetCost(startPoint, p => Equals(p, endPoint), out totalCost);
    }

    public bool TryGetCost(T startPoint, Func<T, bool> endReached, out long totalCost)
    {
        return TryGetCost(startPoint, endReached, out totalCost, out _);
    }

    public long GetCost(T startPoint, T endPoint)
    {
        if (TryGetCost(startPoint, endPoint, out var totalCost))
            return totalCost;
        throw new Exception("Path not found.");
    }

    public long GetCost(T startPoint, Func<T, bool> endReached)
    {
        if (TryGetCost(startPoint, endReached, out var totalCost))
            return totalCost;
        throw new Exception("Path not found.");
    }

    public bool TryGetPath(T startPoint, T endPoint, out IEnumerable<T> bestPath)
    {
        if (!TryGetCost(startPoint, p => Equals(p, endPoint), out _, out var neighbors))
        {
            bestPath = new List<T>();
            return false;
        }

        var pathUsed = new List<T>();
        var current = endPoint;
        do
        {
            pathUsed.Add(current);
            current = neighbors[current];
        } while (!Equals(current, startPoint));

        pathUsed.Add(startPoint);

        bestPath = pathUsed.AsEnumerable().Reverse().ToList();

        return true;
    }

    public IEnumerable<T> GetPath(T startPoint, T endPoint)
    {
        if (TryGetPath(startPoint, endPoint, out var path))
            return path;
        throw new Exception("Path not found.");
    }

    private bool TryGetCost(T startPoint, Func<T, bool> endReached, out long totalCost, out Dictionary<T, T> bestPath)
    {
        var minCosts = new Dictionary<T, long>
        {
            { startPoint, 0 }
        };

        bestPath = new Dictionary<T, T>();

        var processed = new HashSet<T>();
        while (minCosts.Any())
        {
            var minimalCost = minCosts
                .Where(x => !processed.Contains(x.Key))
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .First();

            processed.Add(minimalCost);

            foreach (var p in reachableFunction(minimalCost))
            {
                if (processed.Contains(p))
                    continue;

                var cost = minCosts[minimalCost] + costFunction(minimalCost, p);

                if (minCosts.GetValueOrDefault(p, long.MaxValue) > cost)
                {
                    minCosts[p] = cost;
                    bestPath[p] = minimalCost;
                }

                if (!endReached(p))
                    continue;

                totalCost = minCosts[p];

                return true;
            }

            minCosts.Remove(minimalCost);
        }

        totalCost = long.MaxValue;
        return false;
    }



}