namespace AdventOfCode.Map;

[Flags]
public enum Edge
{
    Top = 0b1,
    Left = 0b10,
    Bottom = 0b100,
    Right = 0b1000
}