namespace AdventOfCode.Comparers;
public class StringComparers
{
    /// <summary>
    /// Go through both string character by character
    /// All characters must be in <see cref="priority"/>
    /// 1. If same continue
    /// 2. If different the lowest index of character in <param name="priority"/> wins
    /// </summary>
    public static Comparer<string> Sequential(string priority) => Comparer<string>.Create((a, b) =>
    {
        for (int i = 0; i < 5; i++)
        {
            if (a[i] == b[i])
                continue;

            var indexOfA = priority.IndexOf(a[i]);
            if (indexOfA < 0)
                throw new InvalidOperationException($"Character '{a[i]}' not found!");
            var indexOfB = priority.IndexOf(b[i]);
            if (indexOfB < 0)
                throw new InvalidOperationException($"Character '{b[i]}' not found!");
            return indexOfB - indexOfA;
        }

        return 0;
    });
}
