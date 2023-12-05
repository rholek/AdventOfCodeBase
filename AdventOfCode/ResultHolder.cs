namespace AdventOfCode;

public static class ResultHolder
{
    private static readonly AsyncLocal<string?> part1Result = new();
    private static readonly AsyncLocal<string?> part2Result = new();

    public static IDisposable Begin()
    {
        return new ResultHolderInternal();
    }

    public static bool Part1Set => part1Result.Value is not null;

    public static bool Part2Set => part2Result.Value is not null;

    public static string Part1
    {
        get => part1Result.Value ?? throw new InvalidOperationException();
        set => part1Result.Value = value;
    }

    public static string Part2
    {
        get => part2Result.Value ?? throw new InvalidOperationException();
        set => part2Result.Value = value;
    }

    class ResultHolderInternal : IDisposable
    {
        public void Dispose()
        {
            part1Result.Value = null;
            part2Result.Value = null;
        }
    }

}