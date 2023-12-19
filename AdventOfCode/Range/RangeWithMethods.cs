namespace AdventOfCode.Range;

public record RangeWithMethods(long Start, long End)
{
    public static readonly RangeWithMethods Empty = new(0, -1);

    public bool Contains(long n) => n >= Start && n <= End;

    public RangeWithMethods Offset(long offset) => new(Start - offset, End - offset);

    public static implicit operator (long start, long end)(RangeWithMethods d) => (d.Start, d.End);
    public static implicit operator RangeWithMethods((long start, long end) d) => new(d.start, d.end);

    public bool IsEmpty => End < Start;

    public long Size => End - Start + 1;
}