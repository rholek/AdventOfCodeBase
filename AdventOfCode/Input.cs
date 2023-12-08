using AdventOfCode.Map;
using System.Globalization;

namespace AdventOfCode;

public class Input
{
    #region Constants

    private const char NEW_LINE = '\n';

    #endregion

    #region Fields

    private Func<string> factoryToUse;
    private readonly Func<string> factory;
    private readonly Func<string>? factoryPart2;
    private string? cache;

    #endregion

    #region Constructors

    public Input(Func<string> factory) : this(factory, null)
    {
    }

    public Input(Func<string> factory, Func<string>? factoryPart2)
    {
        this.factory = factory;
        this.factoryPart2 = factoryPart2;
        factoryToUse = factory;
    }

    #endregion

    #region Public methods

    public void SwitchToPart2()
    {
        factoryToUse = factoryPart2 ?? factory;
        cache = null;
        lines = null;
        IsPart1 = false;
    } 

    #endregion

    #region Input file processing

    public bool IsPart1 { get; private set; } = true;

    public string InputText => cache ??= factoryToUse();

    private IReadOnlyList<string>? lines;
    public IReadOnlyList<string> Lines => lines ??= InputText.Trim().Split(NEW_LINE).Select(x => x.Trim()).ToList();

    public IList<string> AllLinesWithoutTrim => InputText.Split(NEW_LINE).ToList();

    public IEnumerable<IEnumerable<string>> GroupsSplitByEmptyLine =>
        InputText.Split(NEW_LINE + NEW_LINE.ToString()).Select(x => x.Split(NEW_LINE)
            .Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()));

    public IEnumerable<T> ParseInput<T>(Func<string, T> parseLine)
    {
        foreach (var line in Lines)
            yield return parseLine(line);
    }

    /// <summary>
    /// Pattern:
    /// %N or %n integer
    /// %D or %d numeric
    /// %S or %s string
    /// %C or %c char
    /// %% percent symbol literal 
    /// </summary>
    public IEnumerable<ParserResult> ParseInput(string pattern)
    {
        foreach (var line in Lines)
            yield return Parser.Parse(line, pattern);
    }

    public IEnumerable<T> ParseInput<T>()
    {
        foreach (var line in Lines)
            yield return (T)Convert.ChangeType(line.Trim(), typeof(T), CultureInfo.InvariantCulture);
    }

    public IEnumerable<T> ParseInputInLine<T>(string separator = ",")
    {
        foreach (var s in Lines[0].Split(separator))
            yield return (T)Convert.ChangeType(s.Trim(), typeof(T), CultureInfo.InvariantCulture);
    }

    public Map<string> AsMap()
    {
        return new Map<string>(Lines.ToArray());
    }

    #endregion
}