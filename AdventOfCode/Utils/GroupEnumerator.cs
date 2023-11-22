namespace AdventOfCode.Utils;

public class GroupEnumerator
{
    private readonly CharEnumerator innerCharEnumerator;

    public GroupEnumerator(string s)
    {
        innerCharEnumerator = s.GetEnumerator();
        innerCharEnumerator.MoveNext();
    }

    public bool HasNext { get; private set; } = true;

    public string GetNext(int count)
    {
        var s = "";
        for (int i = 0; i < count; i++)
        {
            if (!HasNext)
                throw new Exception("Cannot find enough characters!");
            s += innerCharEnumerator.Current;
            HasNext = innerCharEnumerator.MoveNext();
        }

        return s;
    }

}