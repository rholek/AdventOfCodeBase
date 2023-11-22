namespace AdventOfCode;

public static class ProductExtensions
{
    public static int Product(this IEnumerable<int> data)
    {
        return data.Aggregate(1, (a, b) => a * b);
    }

    public static long Product(this IEnumerable<long> data)
    {
        return data.Aggregate(1L, (a, b) => a * b);
    }

    public static double Product(this IEnumerable<double> data)
    {
        return data.Aggregate(1.0, (a, b) => a * b);
    }

}
