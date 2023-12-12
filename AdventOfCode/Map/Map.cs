﻿using System.Collections;
using System.Drawing;

namespace AdventOfCode.Map;

public class Map<T> : IDictionary<(int column, int row), T>
{
    private readonly IDictionary<(int column, int row), T> dictionaryImplementation;

    public Map(IDictionary<(int column, int row), T> data)
    {
        dictionaryImplementation = new Dictionary<(int column, int row), T>(data);
    }

    public Map(IEnumerable<string> rows)
    {
        var data = rows.ToList();
        dictionaryImplementation = new Dictionary<(int column, int row), T>();
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

    public IEnumerable<T> GetColumn(int index) => this.Where(x => x.Key.column == index).Select(x => x.Value);

    public IEnumerable<T> GetRow(int index) => this.Where(x => x.Key.row == index).Select(x => x.Value);


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


    public T this[(int column, int row) key]
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

    public IEnumerator<KeyValuePair<(int column, int row), T>> GetEnumerator()
    {
        return dictionaryImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)dictionaryImplementation).GetEnumerator();
    }

    public void Add(KeyValuePair<(int column, int row), T> item)
    {
        dictionaryImplementation.Add(item);
    }

    public void Clear()
    {
        dictionaryImplementation.Clear();
    }

    public bool Contains(KeyValuePair<(int column, int row), T> item)
    {
        return dictionaryImplementation.Contains(item);
    }

    public void CopyTo(KeyValuePair<(int column, int row), T>[] array, int arrayIndex)
    {
        dictionaryImplementation.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<(int column, int row), T> item)
    {
        return dictionaryImplementation.Remove(item);
    }

    public int Count => dictionaryImplementation.Count;

    public bool IsReadOnly => dictionaryImplementation.IsReadOnly;

    public void Add((int column, int row) key, T value)
    {
        dictionaryImplementation.Add(key, value);
    }

    public bool ContainsKey((int column, int row) key)
    {
        return dictionaryImplementation.ContainsKey(key);
    }

    public bool Remove((int column, int row) key)
    {
        return dictionaryImplementation.Remove(key);
    }

    public bool TryGetValue((int column, int row) key, out T value)
    {
        return dictionaryImplementation.TryGetValue(key, out value);
    }

    public ICollection<(int column, int row)> Keys => dictionaryImplementation.Keys;

    public ICollection<T> Values => dictionaryImplementation.Values;

    #endregion
}