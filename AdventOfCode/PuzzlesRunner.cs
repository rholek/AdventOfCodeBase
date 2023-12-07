using System.Reflection;

namespace AdventOfCode;

public static class PuzzlesRunner
{
    public static int? Year { get; set; }

    private static Puzzle GetPuzzle(int day)
    {
        if (Year is null)
            throw new InvalidOperationException("Year not set");

        var dayString = "Day" + day.ToString().PadLeft(2, '0');
        var entryAssembly = Assembly.GetEntryAssembly() ?? throw new ArgumentNullException();
        var type = entryAssembly.GetTypes().SingleOrDefault(x => x.Name == dayString) ?? throw new InvalidOperationException($"Cannot create puzzle for day {day}.");
        return Activator.CreateInstance(type) as Puzzle ?? throw new InvalidOperationException($"Cannot create puzzle for day {day}.");
    }

    public static IEnumerable<Puzzle> AllPuzzles => Enumerable.Range(1, 25).Select(GetPuzzle);

    public static Puzzle Today
    {
        get
        {
            if (DateTime.Now.Month != 12 || DateTime.Now.Day > 25)
                throw new InvalidOperationException("No challenge today");
            return GetPuzzle(DateTime.Now.Day);
        }
    }

    public static Puzzle Day01 => GetPuzzle(1);
    public static Puzzle Day02 => GetPuzzle(2);
    public static Puzzle Day03 => GetPuzzle(3);
    public static Puzzle Day04 => GetPuzzle(4);
    public static Puzzle Day05 => GetPuzzle(5);
    public static Puzzle Day06 => GetPuzzle(6);
    public static Puzzle Day07 => GetPuzzle(7);
    public static Puzzle Day08 => GetPuzzle(8);
    public static Puzzle Day09 => GetPuzzle(9);
    public static Puzzle Day10 => GetPuzzle(10);
    public static Puzzle Day11 => GetPuzzle(11);
    public static Puzzle Day12 => GetPuzzle(12);
    public static Puzzle Day13 => GetPuzzle(13);
    public static Puzzle Day14 => GetPuzzle(14);
    public static Puzzle Day15 => GetPuzzle(15);
    public static Puzzle Day16 => GetPuzzle(16);
    public static Puzzle Day17 => GetPuzzle(17);
    public static Puzzle Day18 => GetPuzzle(18);
    public static Puzzle Day19 => GetPuzzle(19);
    public static Puzzle Day20 => GetPuzzle(20);
    public static Puzzle Day21 => GetPuzzle(21);
    public static Puzzle Day22 => GetPuzzle(22);
    public static Puzzle Day23 => GetPuzzle(23);
    public static Puzzle Day24 => GetPuzzle(24);
    public static Puzzle Day25 => GetPuzzle(25);
}

public static class PuzzleExtensions
{
    public static void Run(this IEnumerable<Puzzle> puzzles)
    {
        foreach (var puzzle in puzzles)
            puzzle.Run();
    }
}