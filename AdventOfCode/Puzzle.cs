using AdventOfCode.Api;
using AdventOfCode.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

public abstract class Puzzle
{
    #region Runner

    public void Run()
    {
        $"Running {GetType().Name}".Dump(ConsoleColor.Blue);
        "-----------------------------------------------------".Dump(ConsoleColor.DarkGray);
        Console.WriteLine();

        var stopwatch = Stopwatch.StartNew();
        var day = GetType().Name.Replace("Day", "").AsInt();
        var inputLoader = new AdventOfCodeData(day);

        "Running with test data".Dump(ConsoleColor.Blue);
        "-----------------------------------------------------".Dump(ConsoleColor.DarkGray);

        var runTestData = !GetType().GetCustomAttributes().OfType<SkipTestDataAttribute>().Any();
        if (runTestData)
        {
            using var _ = ResultHolder.Begin();
            RunInternal(inputLoader.TestInput);
            if (ResultHolder.Part1Set && !inputLoader.Answers.CheckTestResult(ResultHolder.Part1, 1))
                return;
            if (ResultHolder.Part2Set && !inputLoader.Answers.CheckTestResult(ResultHolder.Part2, 2))
                return;
        }
        else
        {
            "Test data skipped (SkipTestData attribute found)!".Dump(ConsoleColor.DarkYellow);
        }

        Console.WriteLine();
        "-----------------------------------------------------".Dump(ConsoleColor.DarkGray);
        $"{GetType().Name} (test data) run for {stopwatch.ElapsedMilliseconds}ms.".Dump(ConsoleColor.DarkGray);

        Console.WriteLine();
        "Running with real data".Dump(ConsoleColor.Blue);
        "-----------------------------------------------------".Dump(ConsoleColor.DarkGray);
        Console.WriteLine();
        using (var _ = ResultHolder.Begin())
        {
            RunInternal(inputLoader.RealInput);
            if (ResultHolder.Part1Set)
                inputLoader.Answers.CheckRealResult(ResultHolder.Part1, 1);
            if (ResultHolder.Part2Set)
                inputLoader.Answers.CheckRealResult(ResultHolder.Part2, 2);
        }

        Console.WriteLine();
        "-----------------------------------------------------".Dump(ConsoleColor.DarkGray);
        $"{GetType().Name} (real data) run for {stopwatch.ElapsedMilliseconds}ms.".Dump(ConsoleColor.DarkGray);
        Console.WriteLine();
    }

    protected abstract void RunInternal(Input input);


    #endregion
}