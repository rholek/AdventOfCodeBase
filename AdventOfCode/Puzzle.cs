using AdventOfCode.Api;
using System.Diagnostics;

namespace AdventOfCode;

public abstract class Puzzle
{
    #region Runner

    public void Run()
    {
        Console.WriteLine("Running " + GetType().Name);
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine();

        var stopwatch = Stopwatch.StartNew();
        var day = GetType().Name.Replace("Day", "").AsInt();
        var inputLoader = new AdventOfCodeData(day);

        Console.WriteLine("Running with test data");
        Console.WriteLine("-----------------------------------------------------");

        using (var _ = ResultHolder.Begin())
        {
            RunInternal(inputLoader.TestInput);
            if (ResultHolder.Part1Set && !inputLoader.Answers.CheckTestResult(ResultHolder.Part1, 1))
                return;
            if (ResultHolder.Part2Set && !inputLoader.Answers.CheckTestResult(ResultHolder.Part2, 2))
                return;
        }

        Console.WriteLine();
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine(GetType().Name + " (test data) run for " + stopwatch.ElapsedMilliseconds + "ms.");

        Console.WriteLine();
        Console.WriteLine("Running with real data");
        Console.WriteLine("-----------------------------------------------------");
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
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine(GetType().Name + " (real data) run for " + stopwatch.ElapsedMilliseconds + "ms.");
        Console.WriteLine();
    }

    protected abstract void RunInternal(Input input);


    #endregion
}