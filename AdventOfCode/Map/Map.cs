using System.Collections;
using System.Drawing;

namespace AdventOfCode.Map;

public class Map<T> : IDictionary<Point2D, T>
{
    private readonly IDictionary<Point2D, T> dictionaryImplementation;

    public Map(IDictionary<Point2D, T> data)
    {
        dictionaryImplementation = new Dictionary<Point2D, T>(data);
    }

    public Map(IEnumerable<string> rows)
    {
        var data = rows.ToList();
        dictionaryImplementation = new Dictionary<Point2D, T>();
        for (int i = 0; i < data.Count; i++)
            for (int j = 0; j < data[i].Length; j++)
                this[(j, i)] = data[i][j].ToString().As<T>();
    }

    public Map(params string[] data) : this(data.ToList()) { }

    private int? widthCache;
    /// <summary>
    /// Cached
    /// </summary>
    public int Width => widthCache ??= CalculateWidth();

    private int? heightCache;
    /// <summary>
    /// Cached
    /// </summary>
    public int Height => heightCache ??= CalculateHeight();

    /// <summary>
    /// NOT cached
    /// </summary>
    public int CalculateWidth() => this.Max(x => x.Key.column) - this.Min(x => x.Key.column) + 1;

    /// <summary>
    /// NOT cached
    /// </summary>
    public int CalculateHeight() => this.Max(x => x.Key.row) - this.Min(x => x.Key.row) + 1;

    private Rectangle? areaCache;
    /// <summary>
    /// Cached
    /// </summary>
    public Rectangle Area => areaCache ??= CalculateArea();

    /// <summary>
    /// NOT cached
    /// </summary>
    public Rectangle CalculateArea() => new Rectangle(this.Min(x => x.Key.column), this.Min(x => x.Key.row), Width, Height);

    public IEnumerable<T> GetColumn(int index) => this.Where(x => x.Key.column == index).OrderBy(x => x.Key.row).Select(x => x.Value);

    public IEnumerable<T> GetRow(int index) => this.Where(x => x.Key.row == index).OrderBy(x => x.Key.column).Select(x => x.Value);


    public void ShiftColumn(int columnIndex, int step)
    {
        var pointsToShift = step switch
        {
            0 => [],
            < 0 => this.OrderBy(x => x.Key.column).Where(x => x.Key.column <= columnIndex),
            > 0 => this.OrderByDescending(x => x.Key.column).Where(x => x.Key.column >= columnIndex)
        };

        foreach (var ((column, row), T) in pointsToShift)
        {
            Add((column + step, row), T);
            Remove((column, row));
        }
    }

    public void ShiftRow(int rowIndex, int step)
    {
        var pointsToShift = step switch
        {
            0 => [],
            < 0 => this.OrderBy(x => x.Key.row).Where(x => x.Key.row <= rowIndex),
            > 0 => this.OrderByDescending(x => x.Key.row).Where(x => x.Key.row >= rowIndex)
        };

        foreach (var ((column, row), T) in pointsToShift)
        {
            Add((column, row + step), T);
            Remove((column, row));
        }
    }


    public T this[Point2D key]
    {
        get
        {
            if (!dictionaryImplementation.ContainsKey(key))
                return default!;
            return dictionaryImplementation[key];
        }
        set => dictionaryImplementation[key] = value;
    }

    public T this[int column, int row]
    {
        get => this[(column, row)];
        set => this[(column, row)] = value;
    }

    public Map<T> Clone()
    {
        return new Map<T>(this);
    }

    #region Other dictionary members

    public IEnumerator<KeyValuePair<Point2D, T>> GetEnumerator()
    {
        return dictionaryImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)dictionaryImplementation).GetEnumerator();
    }

    public void Add(KeyValuePair<Point2D, T> item)
    {
        dictionaryImplementation.Add(item);
    }

    public void Clear()
    {
        dictionaryImplementation.Clear();
    }

    public bool Contains(KeyValuePair<Point2D, T> item)
    {
        return dictionaryImplementation.Contains(item);
    }

    public bool ContainsPosition(Point2D item)
    {
        return dictionaryImplementation.ContainsKey(item);
    }

    public void CopyTo(KeyValuePair<Point2D, T>[] array, int arrayIndex)
    {
        dictionaryImplementation.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<Point2D, T> item)
    {
        return dictionaryImplementation.Remove(item);
    }

    public bool Remove(T item)
    {
        var toRemove = this.Where(x => Equals(item, x.Value)).Select(x => x.Key).ToList();
        toRemove.ForEach(x => dictionaryImplementation.Remove(x));
        return toRemove.Count > 0;
    }

    public Point2D GetSinglePosition(T item)
    {
        var items = this.Where(x => Equals(x.Value, item)).ToList();
        if (items.Count != 1)
            throw new InvalidOperationException($"Found {items.Count} items instead of 1 item");
        return items[0].Key;
    }

    public int Count => dictionaryImplementation.Count;

    public bool IsReadOnly => dictionaryImplementation.IsReadOnly;

    public void Add(Point2D key, T value)
    {
        dictionaryImplementation.Add(key, value);
    }

    public bool ContainsKey(Point2D key)
    {
        return dictionaryImplementation.ContainsKey(key);
    }

    public bool Remove(Point2D key)
    {
        return dictionaryImplementation.Remove(key);
    }

    public bool TryGetValue(Point2D key, out T value)
    {
        return dictionaryImplementation.TryGetValue(key, out value);
    }

    public ICollection<Point2D> Keys => dictionaryImplementation.Keys;

    public ICollection<T> Values => dictionaryImplementation.Values;

    #endregion
}