// ReSharper disable InconsistentNaming

namespace AdventOfCode;

public static class ExtraMath
{
    /// <summary>
    /// Greatest common divisor
    /// </summary>
    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    /// <summary>
    /// Lowest common multiple
    /// </summary>
    public static long LCM(long a, long b)
    {
        return (a / GCD(a, b)) * b;
    }

    /// <summary>
    /// Lowest common multiple
    /// </summary>
    public static long LCM(params long[] data)
    {
        if (data.Length == 1)
            return data[0];

        var list = data.ToList();
        var num = LCM(data[0], data[1]);
        list.RemoveAt(0);
        list.RemoveAt(0);
        if (list.Count == 0)
            return num;
        list.Add(num);
        return LCM(list.ToArray());
    }

    public static IEnumerable<List<T>> CartesianProduct<T>(IEnumerable<List<T>> first, IEnumerable<List<T>> second)
    {
        return from f in first from s in second select f.Concat(s).ToList();
    }



    /// <summary>
    /// Calculates modular inverse
    /// </summary>
    public static long ModInverse(long number, long mod)
    {
        long i = mod, v = 0, d = 1;
        while (number > 0)
        {
            long t = i / number, x = number;
            number = i % x;
            i = x;
            x = d;
            d = v - t * x;
            v = x;
        }
        v %= mod;
        if (v < 0) v = (v + mod) % mod;
        return v;
    }

    public static double ArithmeticSum(double startValue, double distance, int count)
    {
        return (count / 2.0) * (2 * startValue + (count - 1) * distance);
    }

    public static double GeometricSum(double startValue, double distance, int count)
    {
        return startValue * (1 - Math.Pow(distance, count + 1)) / (1 - distance);
    }


    public static int ModuloPositive(int x, int mod) => ((x % mod) + mod) % mod;
    public static long ModuloPositive(long x, long mod) => ((x % mod) + mod) % mod;
}