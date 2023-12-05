using System.Collections;

namespace AdventOfCode;

public static class ConsoleOutputExtensions
{
    public static void Dump(this object o)
    {
        if (o is string)
        {
            Console.WriteLine(o);
            return;
        }

        if (o is IEnumerable enumerable)
        {
            foreach (var e in enumerable)
                e.Dump();
            return;
        }

        Console.WriteLine(o);
    }

    public static void Dump(this object o, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Dump(o);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }

    public static bool Ask(this object o, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Dump(o);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }

        var read = Console.ReadLine().ToLower();
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        return read.In("yes", "y");
    }

    public static T Part1<T>(this T o)
    {
        ResultHolder.Part1 = o.ToString();
        Console.WriteLine("Part 1: " + o);
        return o;
    }

    public static T Part2<T>(this T o)
    {
        ResultHolder.Part2 = o.ToString();
        Console.WriteLine("Part 2: " + o);
        return o;
    }

    public static void Dump<T>(this IDictionary<(int x, int y), T> data)
    {
        PrintDictionary(data, x => x?.ToString() ?? string.Empty);
    }

    public static void PrintDictionary<T>(this IDictionary<(int x, int y), T> data, Func<T, string> selector, string emptySpace = " ")
    {
        if (!data.Any())
            return;

        var minX = data.Keys.Select(x => x.x).MinBy(x => x);
        var maxX = data.Keys.Select(x => x.x).MaxBy(x => x);

        var minY = data.Keys.Select(x => x.y).MinBy(x => x);
        var maxY = data.Keys.Select(x => x.y).MaxBy(x => x);

        var xOffset = -minX;
        var yOffset = -minY;

        var print = new string[maxY + yOffset + 1, maxX + xOffset + 1];

        for (int i = 0; i < print.GetLength(0); i++)
            for (int j = 0; j < print.GetLength(1); j++)
                print[i, j] = emptySpace;

        foreach (var l in data)
            print[l.Key.y + yOffset, l.Key.x + xOffset] = selector(l.Value);

        for (int i = 0; i < print.GetLength(0); i++)
        {
            for (int j = 0; j < print.GetLength(1); j++)
                Console.Write(print[i, j]);
            Console.WriteLine();
        }
    }

    public static void PrintCollection(this IEnumerable<(int x, int y)> source, string symbol, string emptySpace = " ")
    {
        source.PrintCollection(_ => symbol, emptySpace);
    }

    public static void PrintCollection(this IEnumerable<(int x, int y)> source, Func<(int x, int y), string> selector, string emptySpace = " ")
    {
        var data = source.ToList();
        if (!data.Any())
            return;

        var minX = data.Select(x => x.x).MinBy(x => x);
        var maxX = data.Select(x => x.x).MaxBy(x => x);

        var minY = data.Select(x => x.y).MinBy(x => x);
        var maxY = data.Select(x => x.y).MaxBy(x => x);

        var xOffset = -minX;
        var yOffset = -minY;

        var print = new string[maxY + yOffset + 1, maxX + xOffset + 1];

        for (int i = 0; i < print.GetLength(0); i++)
            for (int j = 0; j < print.GetLength(1); j++)
                print[i, j] = emptySpace;

        foreach (var l in data)
            print[l.y + yOffset, l.x + xOffset] = selector(l);

        for (int i = 0; i < print.GetLength(0); i++)
        {
            for (int j = 0; j < print.GetLength(1); j++)
                Console.Write(print[i, j]);
            Console.WriteLine();
        }
    }
}