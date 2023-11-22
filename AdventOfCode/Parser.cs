using System.Globalization;
using System.Text;

namespace AdventOfCode;

public static class Parser
{
    public static Func<string, ParserResult> Create(string pattern)
    {
        return x => Parse(x, pattern);
    }

    /// <summary>
    /// Pattern:
    /// %N or %n integer
    /// %D or %d numeric
    /// %S or %s string
    /// %C or %c char
    /// %% percent symbol literal 
    /// </summary>
    public static ParserResult Parse(string input, string pattern)
    {
        Dictionary<string, object> result = new();
        using var inputEnumerator = input.GetEnumerator();

        int intCount = 1;
        int decimalCount = 1;
        int stringCount = 1;
        int charCount = 1;


        using var nextPatternEnumerator = GetNextPatternItem().GetEnumerator();
        nextPatternEnumerator.MoveNext();
        inputEnumerator.MoveNext();

        bool hasNext = true;
        foreach (var item in GetNextPatternItem())
        {
            if (!hasNext)
                break;
            nextPatternEnumerator.MoveNext();
            switch (item.type)
            {
                case PatternItem.Int:
                    var intDigitBuilder = new StringBuilder();
                    if (inputEnumerator.Current == '-')
                    {
                        intDigitBuilder.Append("-");
                        inputEnumerator.MoveNext();
                    }

                    while (inputEnumerator.Current.IsDigit())
                    {
                        intDigitBuilder.Append(inputEnumerator.Current);
                        hasNext = inputEnumerator.MoveNext();
                        if (!hasNext)
                            break;
                    }

                    var i = int.Parse(intDigitBuilder.ToString(), CultureInfo.InvariantCulture);
                    var iIndex = intCount++;
                    ((IDictionary<string, object>)result)[$"N{iIndex}"] = i;
                    ((IDictionary<string, object>)result)[$"n{iIndex}"] = i;

                    break;
                case PatternItem.Decimal:
                    var numericDigitBuilder = new StringBuilder();
                    if (inputEnumerator.Current == '-')
                    {
                        numericDigitBuilder.Append("-");
                        inputEnumerator.MoveNext();
                    }
                    while (inputEnumerator.Current.IsDigit() || inputEnumerator.Current.IsDecimalSeparator())
                    {
                        numericDigitBuilder.Append(inputEnumerator.Current);
                        hasNext = inputEnumerator.MoveNext();
                        if (!hasNext)
                            break;
                    }

                    var d = decimal.Parse(numericDigitBuilder.ToString(), CultureInfo.InvariantCulture);
                    var dIndex = decimalCount++;
                    ((IDictionary<string, object>)result)[$"D{dIndex}"] = d;
                    ((IDictionary<string, object>)result)[$"d{dIndex}"] = d;

                    break;
                case PatternItem.String:
                    var stringBuilder = new StringBuilder();
                    while (true)
                    {
                        if (nextPatternEnumerator.Current.type == PatternItem.Literal && nextPatternEnumerator.Current.literal == inputEnumerator.Current)
                            break;

                        stringBuilder.Append(inputEnumerator.Current);
                        hasNext = inputEnumerator.MoveNext();
                        if (!hasNext)
                            break;
                    }

                    var s = stringBuilder.ToString();
                    var sIndex = stringCount++;
                    ((IDictionary<string, object>)result)[$"S{sIndex}"] = s;
                    ((IDictionary<string, object>)result)[$"s{sIndex}"] = s;
                    break;
                case PatternItem.Char:
                    var c = inputEnumerator.Current;
                    var cIndex = charCount++;
                    ((IDictionary<string, object>)result)[$"C{cIndex}"] = c;
                    ((IDictionary<string, object>)result)[$"c{cIndex}"] = c;
                    hasNext = inputEnumerator.MoveNext();
                    break;
                case PatternItem.Literal:
                    if (inputEnumerator.Current != item.literal)
                        throw new Exception("Wrong patter!");
                    hasNext = inputEnumerator.MoveNext();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        return new ParserResult(result);


        IEnumerable<(PatternItem type, char literal)> GetNextPatternItem()
        {
            var isPercent = false;
            foreach (var letter in pattern)
            {
                switch (letter)
                {
                    case '%' when isPercent:
                        isPercent = false;
                        yield return (PatternItem.Literal, '%');
                        break;
                    case 'N' when isPercent:
                    case 'n' when isPercent:
                        isPercent = false;
                        yield return (PatternItem.Int, default);
                        break;
                    case 'D' when isPercent:
                    case 'd' when isPercent:
                        isPercent = false;
                        yield return (PatternItem.Decimal, default);
                        break;
                    case 'S' when isPercent:
                    case 's' when isPercent:
                        isPercent = false;
                        yield return (PatternItem.String, default);
                        break;
                    case 'C' when isPercent:
                    case 'c' when isPercent:
                        isPercent = false;
                        yield return (PatternItem.Char, default);
                        break;
                    case '%':
                        isPercent = true;
                        break;
                    default:
                        yield return (PatternItem.Literal, letter);
                        break;
                }
            }

        }

    }

    private enum PatternItem
    {
        Int,
        Decimal,
        String,
        Char,
        Literal
    }
}