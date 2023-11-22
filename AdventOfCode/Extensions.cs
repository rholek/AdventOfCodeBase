using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class Extensions
{
    public static int Min(this int x, params int[] other)
    {
        return other.Prepend(x).Min();
    }

    public static int Max(this int x, params int[] other)
    {
        return other.Prepend(x).Max();
    }

    public static int AsInt(this object o)
    {
        return o.As<int>();
    }

    public static decimal AsDecimal(this object o)
    {
        return o.As<decimal>();
    }

    public static string AsString(this object o)
    {
        return o.As<string>();
    }

    public static T As<T>(this object o)
    {
        return o is null
            ? default
            : (T)Convert.ChangeType(o, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
    }

    public static bool IsDigit(this char letter)
    {
        var digits = "0123456789";
        return digits.ToCharArray().Contains(letter);
    }

    public static bool IsDecimalSeparator(this char letter)
    {
        return letter.ToString() == CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
    }

    public static string AsString(this char letter)
    {
        return Convert.ToString(letter);
    }

    public static bool BetweenInclusive(this int number, int lower, int higher)
    {
        return lower <= number && number <= higher;
    }

    public static bool BetweenExclusive(this int number, int lower, int higher)
    {
        return lower < number && number < higher;
    }

    public static int Width<T>(this IDictionary<(int column, int row), T> data)
    {
        return data.Keys.Width();
    }

    public static int Height<T>(this IDictionary<(int column, int row), T> data)
    {
        return data.Keys.Height();
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> data, TKey key, Func<TKey, TValue> valueToAddFactory)
    {
        if (!data.ContainsKey(key))
            data[key] = valueToAddFactory(key);

        return data[key];
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> data, TKey key, TValue valueToAdd)
    {
        return data.GetOrAdd(key, x => valueToAdd);
    }

    public static bool IsEquivalent<TKey, TValue>(this IDictionary<TKey, TValue> data, IDictionary<TKey, TValue> other)
    {
        if (!data.Keys.All(other.Keys.Contains) || !other.Keys.All(data.Keys.Contains))
            return false;

        return data.All(x => Equals(other[x.Key], x.Value));
    }

    public static int Width(this IEnumerable<(int column, int row)> data)
    {
        return data.Max(x => x.column);
    }

    public static int Height(this IEnumerable<(int column, int row)> data)
    {
        return data.Max(x => x.row);
    }

    public static bool In(this object data, params object[] p)
    {
        if (p.Length == 1 && p[1] is IEnumerable enumerable)
            return data.In(enumerable.OfType<object>().ToArray());
        return p.Contains(data);
    }

    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static double Median(this IEnumerable<int> data)
    {
        return Median(data.Select(x => (long)x));
    }

    public static double Median(this IEnumerable<long> data)
    {
        var array = data.OrderBy(x => x).ToArray();
        var n = array.Length;

        double median;

        var isOdd = n % 2 != 0;
        if (isOdd)
        {
            median = array[(n + 1) / 2 - 1];
        }
        else
        {
            median = (array[n / 2 - 1] + array[n / 2]) / 2.0d;
        }

        return median;
    }


    public static int Abs(this int input)
    {
        return Math.Abs(input);
    }

    public static long Abs(this long input)
    {
        return Math.Abs(input);
    }

    public static double Abs(this double input)
    {
        return Math.Abs(input);
    }

    /// <summary>
    /// Returns collection, that contains items in all given collections
    /// </summary>
    public static IEnumerable<TResult> IntersectMany<TResult>(this IEnumerable<ICollection<TResult>> source)
    {
        return IntersectMany(source, x => x);
    }

    /// <summary>
    /// Returns collection, that contains items in all given collections
    /// </summary>
    public static IEnumerable<TResult> IntersectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            return new TResult[0];

        var returnValue = selector(enumerator.Current);

        while (enumerator.MoveNext())
            returnValue = returnValue.Intersect(selector(enumerator.Current));
        return returnValue;
    }

    public static IDictionary<TValue, TKey> Invert<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary) where TValue : notnull
    {
        return dictionary.ToDictionary(x => x.Value, x => x.Key);
    }

    /// <summary>
    /// Returns collection without specified item
    /// </summary>
    /// <param name="original">The original.</param>
    /// <param name="remove">The item skip.</param>
    public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> original, TSource remove)
    {
        return original.Where(x => !Equals(x, remove));
    }

    public static bool IsNullOrEmpty(this string s)
    {
        // ReSharper disable once StringNullOrEmptyExtensionCall
        return string.IsNullOrEmpty(s);
    }

    public static bool IsNullOrWhiteSpace(this string s)
    {
        // ReSharper disable once StringNullOrEmptyExtensionCall
        return string.IsNullOrWhiteSpace(s);
    }

    public static IEnumerable<int> GetAllIndexes(this string source, string matchString)
    {
        //matchString = Regex.Escape(matchString);
        foreach (Match match in Regex.Matches(source, matchString))
        {
            yield return match.Index;
        }
    }

    public static IEnumerable<int> IndexOfAll(this string sourceString, string subString)
    {
        return Regex.Matches(sourceString, subString).Cast<Match>().Select(m => m.Index);
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> aggregateFunc) where TKey : notnull
    {
        var result = first.ToDictionary(x => x.Key, x => x.Value);
        foreach (var (key, value) in second)
        {
            if (result.ContainsKey(key))
                result[key] = aggregateFunc(result[key], value);
            else
                result[key] = value;
        }

        return result;
    }

    public static void AddRange<T>(this HashSet<T> hashset, IEnumerable<T> data)
    {
        foreach (var item in data)
            hashset.Add(item);
    }

    public static string Join<T>(this IEnumerable<T> items, string separator = ",")
    {
        return string.Join(separator, items);
    }

    public static IEnumerable<string> SplitByLine(this string input) => input.Split(Environment.NewLine);

    public static IEnumerable<string> SplitByDoubleLine(this string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}");
}