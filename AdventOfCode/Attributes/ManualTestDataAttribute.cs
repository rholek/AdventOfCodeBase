namespace AdventOfCode.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ManualTestDataAttribute(string input) : Attribute
{
    public string Input { get; } = input;

    public string? Part1Result { get; set; }

    public string? Part2Result { get; set; }
}