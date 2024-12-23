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

    public static long AsLong(this object o)
    {
        return o.As<long>();
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
        //char when converted into number uses code instead of value
        if (o is char c)
            o = c.ToString();

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

    public static string[] ToStringArray(this string input)
    {
        return input.ToCharArray().Select(x => x.AsString()).ToArray();
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
        if (p is [IEnumerable enumerable])
            return data.In(enumerable.OfType<object>().ToArray());
        return p.Contains(data);
    }

    public static IEnumerable<T> AsEnumerableWithOneItem<T>(this T item)
    {
        return new[] { item };
    }

    public static List<T> AsListWithOneItem<T>(this T item)
    {
        return item.AsEnumerableWithOneItem().ToList();
    }

    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
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

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection) => collection.Select((x, i) => (x, i));

    /// <summary>
    /// Returns collection without specified item
    /// </summary>
    /// <param name="original">The original.</param>
    /// <param name="remove">The item skip.</param>
    public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> original, TSource remove)
    {
        return original.Where(x => !Equals(x, remove));
    }

    public static bool IsNullOrEmpty(this string? s)
    {
        // ReSharper disable once StringNullOrEmptyExtensionCall
        return string.IsNullOrEmpty(s);
    }
    
    public static bool IsNullOrWhiteSpace(this string s)
    {
        // ReSharper disable once StringNullOrEmptyExtensionCall
        return string.IsNullOrWhiteSpace(s);
    }
    
    public static string Truncate(this string toTruncate, int length)
    {
        return toTruncate.Substring(0, length);
    }

    public static IEnumerable<string> AsEnumerableOfCharacters(this string input)
    {
        return input.Select(x=>x.ToString());
    }

    public static IEnumerable<int> GetAllIndexes(this string source, string matchString)
    {
        //matchString = Regex.Escape(matchString);
        foreach (Match match in Regex.Matches(source, matchString))
        {
            yield return match.Index;
        }
    }

    public static IEnumerable<int> FindAllIndexes<T>(this IEnumerable<T> source, T item)
    {
        return source.Select((x, i) => (x, i)).Where(x => Equals(x.x, item)).Select(x => x.i);
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
            if (!result.TryAdd(key, value))
                result[key] = aggregateFunc(result[key], value);
        }

        return result;
    }

    public static void AddRange<T>(this HashSet<T> hashset, IEnumerable<T> data)
    {
        foreach (var item in data)
            hashset.Add(item);
    }

    public static string Join<T>(this IEnumerable<T> items)
    {
        return string.Join("", items);
    }

    public static string Join<T>(this IEnumerable<T> items, string separator)
    {
        return string.Join(separator, items);
    }

    public static string ConcatIntoString(this IEnumerable<string> items)
    {
        return string.Join(string.Empty, items);
    }

    public static IEnumerable<string> SplitBySpace(this string input, bool removeEmpty = true)
    {
        return input.Split(" ", removeEmpty ? StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries : StringSplitOptions.None);
    }

    public static IEnumerable<string> SplitByComma(this string input, bool removeEmpty = true)
    {
        return input.Split(",", removeEmpty ? StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries : StringSplitOptions.None);
    }

    public static IEnumerable<string> SplitByLine(this string input) => input.Split(Environment.NewLine);

    public static IEnumerable<string> SplitByDoubleLine(this string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}");

    public static string ReplaceAt(this string input, int index, char c)
    {
        var cc = input.ToCharArray();
        cc[index] = c;
        return new string(cc);

    }

    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
    {
        var list = sequence.ToList();
        if (!list.Any())
        {
            yield return Enumerable.Empty<T>();
        }
        else
        {
            var startingElementIndex = 0;

            foreach (var startingElement in list)
            {
                var index = startingElementIndex;
                var remainingItems = list.Where((e, i) => i != index);

                foreach (var permutationOfRemainder in remainingItems.Permute())
                    yield return permutationOfRemainder.Prepend(startingElement);

                startingElementIndex++;
            }
        }
    }

}