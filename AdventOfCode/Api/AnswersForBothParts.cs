namespace AdventOfCode.Api;

internal record AnswersForBothParts
{
    public AnswerForSinglePart Part1 { get; set; } = new();
    public AnswerForSinglePart Part2 { get; set; } = new();

    public AnswerForSinglePart this[int part] => part switch
    {
        1 => Part1,
        2 => Part2,
        _ => throw new InvalidOperationException()
    };
}