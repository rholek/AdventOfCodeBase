namespace AdventOfCode.Utils;

public static class NumberConversions
{

    private static readonly Dictionary<string, string> hexToBinDictionary = new Dictionary<string, string>
    {
        { "0", "0000" },
        { "1", "0001" },
        { "2", "0010" },
        { "3", "0011" },
        { "4", "0100" },
        { "5", "0101" },
        { "6", "0110" },
        { "7", "0111" },
        { "8", "1000" },
        { "9", "1001" },
        { "A", "1010" },
        { "B", "1011" },
        { "C", "1100" },
        { "D", "1101" },
        { "E", "1110" },
        { "F", "1111" },
    };

    public static string HexToBin(string s)
    {
        return new string(Process().ToArray());

        IEnumerable<char> Process()
        {
            foreach (var inp in s.Trim())
            {
                var f = hexToBinDictionary[inp.ToString()];
                foreach (var ff in f)
                {
                    yield return ff;
                }
            }
        }
    }

    public static string DecToBin(string number)
    {
        return DecToBin(int.Parse(number));
    }

    public static string DecToBin(int number)
    {
        return Convert.ToString(number, 2);
    }
}