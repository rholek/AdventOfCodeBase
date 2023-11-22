namespace AdventOfCode.StandardAlgorithms;

//TODO add option to get entire path
//TODO tests
public class PathFinding<T> where T : notnull
{
    private readonly Func<T, T, long> costFunction;
    private readonly Func<T, IEnumerable<T>> reachableFunction;

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
        var toProcess = new Dictionary<T, long>
        {
            { startPoint, 0 }
        };

        var processed = new HashSet<T>();
        while (toProcess.Any())
        {
            var minimalCost = toProcess.Where(x => !processed.Contains(x.Key)).OrderBy(x => x.Value).Select(x => x.Key).First();
            processed.Add(minimalCost);

            foreach (var p in reachableFunction(minimalCost))
            {
                if (processed.Contains(p))
                    continue;

                var cost = toProcess[minimalCost] + costFunction(minimalCost, p);

                if (toProcess.GetValueOrDefault(p, long.MaxValue) > cost)
                    toProcess[p] = cost;

                if (!endReached(p))
                    continue;
                
                totalCost = toProcess[p];
                return true;
            }

            toProcess.Remove(minimalCost);
        }

        totalCost = long.MaxValue;
        return false;
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
}