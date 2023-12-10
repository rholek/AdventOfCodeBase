// ReSharper disable InconsistentNaming

namespace AdventOfCode;

public static class ExtraMath
{
    public const double TOLERANCE = 10e-15;


    public static bool IsPrime(this long number)
    {
        if (number < 2)
            return false;
        return number.AllPrimeFactors().Count == 1;
    }

    public static IReadOnlyList<long> AllPrimeFactors(this long number)
    {
        if (number < 1)
            throw new InvalidOperationException("Negative numbers are not allowed for prime factorization");

        var primes = new List<long>();

        var maxFactor = Math.Sqrt(number);
        for (int div = 2; div <= maxFactor; div++)
        {
            while (number % div == 0)
            {
                primes.Add(div);
                number /= div;
            }
        }

        if (number > 2)
            primes.Add(number);
        return primes;
    }


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