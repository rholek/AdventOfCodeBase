namespace AdventOfCode.Range;
public record RangeWithMethods(long Start, long End)
{
    public bool Contains(long n) => n >= Start && n < End;

    public RangeWithMethods Offset(long offset) => new(Start - offset, End - offset);
}