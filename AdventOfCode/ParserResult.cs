namespace AdventOfCode;

public class ParserResult
{
    private readonly IReadOnlyDictionary<string, object> data;

    public ParserResult(IReadOnlyDictionary<string, object> data)
    {
        this.data = data;
    }

    public int Number(int index) => (int) data[$"n{index}"];
    public int N1 => Number(1);
    public int N2 => Number(2);
    public int N3 => Number(3);
    public int N4 => Number(4);

    public double Decimal(int index) => (double)data[$"d{index}"];
    public double D1 => Decimal(1);
    public double D2 => Decimal(2);
    public double D3 => Decimal(3);
    public double D4 => Decimal(4);

    public string String(int index) => (string)data[$"s{index}"];
    public string S1 => String(1);
    public string S2 => String(2);
    public string S3 => String(3);
    public string S4 => String(4);

    public char Char(int index) => (char)data[$"c{index}"];
    public char C1 => Char(1);
    public char C2 => Char(2);
    public char C3 => Char(3);
    public char C4 => Char(4);

}