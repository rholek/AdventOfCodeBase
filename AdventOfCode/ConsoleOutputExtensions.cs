﻿using System.Collections;

namespace AdventOfCode;

public static class ConsoleOutputExtensions
{
    public static T Dump<T>(this T o)
    {
        if (o is string)
        {
            Console.WriteLine(o);
            return o;
        }

        if (o is IEnumerable enumerable)
        {
            foreach (var e in enumerable)
                e.Dump();
            return o;
        }

        Console.WriteLine(o);
        return o;
    }

    public static T Dump<T>(this T o, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            return Dump(o);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }

    public static bool Ask(this object o, ConsoleColor color)
    {
        o.Dump(color);

        try
        {
            "Yes(Y/Enter) or No(n/Esc)".Dump(ConsoleColor.DarkCyan);
            Console.SetCursorPosition(0, Console.CursorTop - 1);

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key is ConsoleKey.Enter or ConsoleKey.Y)
                    return true;
                if (key.Key is ConsoleKey.Escape or ConsoleKey.N)
                    return false;

                Console.WriteLine();
                "Invalid option".Dump(ConsoleColor.Red);
            }
        }
        finally
        {
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

    }

    public static T Part1<T>(this T o, bool print = false)
    {
        if (ResultHolder.Part1Set)
            return o;
        ResultHolder.Part1 = o.ToString();
        if (print)
            Console.WriteLine("Part 1: " + o);
        return o;
    }

    public static T Part2<T>(this T o, bool print = false)
    {
        if (ResultHolder.Part2Set)
            return o;
        ResultHolder.Part2 = o.ToString();
        if (print)
            Console.WriteLine("Part 2: " + o);
        return o;
    }

    public static void Dump<T>(this Map<T> data)
    {
        PrintDictionary(data, x => x?.ToString() ?? string.Empty);
    }

    public static void Dump<T>(this IDictionary<(int x, int y), T> data)
    {
        PrintDictionary(data, x => x?.ToString() ?? string.Empty);
    }

    public static string PrintToString<T>(this IDictionary<(int x, int y), T> data, string emptySpace = " ")
    {
        var sb = new StringWriter();
        PrintDictionary(data, x => x?.ToString() ?? string.Empty, sb, emptySpace);
        return sb.ToString();
    }


    public static void PrintDictionary<T>(this IDictionary<(int x, int y), T> data, Func<T, string> selector, string emptySpace = " ")
    {
        PrintDictionary(data, selector, Console.Out, emptySpace);
    }


    public static void PrintDictionaryToTempFile(this IDictionary<(int x, int y), string> data, string emptySpace = " ")
    {
        PrintDictionaryToTempFile(data, x => x, emptySpace);
    }

    public static void PrintDictionaryToTempFile<T>(this IDictionary<(int x, int y), T> data, Func<T, string> selector, string emptySpace = " ")
    {
        var path = Path.ChangeExtension(Path.GetTempFileName(), "txt");
        PrintDictionaryToFile(data, selector, path, emptySpace);
        $"Printed to {path}".Dump();
    }

    public static void PrintDictionaryToFile<T>(this IDictionary<(int x, int y), T> data, Func<T, string> selector, string path, string emptySpace = " ")
    {
        using var fileStream = File.Create(path);
        using var writer = new StreamWriter(fileStream);
        PrintDictionary(data, selector, writer, emptySpace);
    }

    public static void PrintDictionary<T>(this IDictionary<(int x, int y), T> data, Func<T, string> selector, TextWriter textWriter, string emptySpace = " ")
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
                textWriter.Write(print[i, j]);
            textWriter.WriteLine();
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