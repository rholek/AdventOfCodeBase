namespace AdventOfCode.Api;

internal record AnswerForSinglePart
{
    public string? Test { get; set; }
    public string? Real { get; set; }

    public List<string> Rejected { get; set; } = new();

}