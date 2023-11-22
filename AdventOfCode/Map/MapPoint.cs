namespace AdventOfCode.Map;

public class MapPoint
{
    protected bool Equals(MapPoint other)
    {
        return Column == other.Column && Row == other.Row;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MapPoint)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }

    public int Column { get; }
    public int Row { get; }

    public MapPoint(int column, int row)
    {
        Column = column;
        Row = row;
    }

    public static implicit operator (int column, int row)(MapPoint d) => (d.Column, d.Row);
    public static implicit operator MapPoint((int column, int row) d) => new(d.column, d.row);

    public static MapPoint operator +(MapPoint first, MapPoint second) => new(first.Column + second.Column, first.Row + second.Row);

    public static MapPoint operator -(MapPoint first, MapPoint second) => new(first.Column - second.Column, first.Row - second.Row);

    public static bool operator ==(MapPoint first, MapPoint second) => Equals(first, second);

    public static bool operator !=(MapPoint first, MapPoint second) => !Equals(first, second);
}