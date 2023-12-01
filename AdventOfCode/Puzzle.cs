using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace AdventOfCode;

public abstract class Puzzle
{
    #region Constants

    protected const double TOLERANCE = 10e-15;

    protected static char NEW_LINE = '\n';

    #endregion

    #region Input file processing

    protected string InputFilePath => $"Inputs\\{GetType().Name.PadLeft(2, '0')}.txt";

    private string? inputTextCache;
    protected string InputText => inputTextCache ??= DownloadInput().Result;

    private IReadOnlyList<string>? lines;
    protected IReadOnlyList<string> Lines => lines ??= InputText.Trim().Split(NEW_LINE).Select(x => x.Trim()).ToList();

    protected IList<string> AllLinesWithoutTrim => InputText.Split(NEW_LINE).ToList();

    protected IEnumerable<IEnumerable<string>> GroupsSplitByEmptyLine =>
        InputText.Split(NEW_LINE + NEW_LINE.ToString()).Select(x => x.Split(NEW_LINE)
            .Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()));

    protected IEnumerable<T> ParseInput<T>(Func<string, T> parseLine)
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
    protected IEnumerable<ParserResult> ParseInput(string pattern)
    {
        foreach (var line in Lines)
            yield return Parser.Parse(line, pattern);
    }

    protected IEnumerable<T> ParseInput<T>()
    {
        foreach (var line in Lines)
            yield return (T)Convert.ChangeType(line.Trim(), typeof(T), CultureInfo.InvariantCulture);
    }

    protected IEnumerable<T> ParseInputInLine<T>(string separator = ",")
    {
        foreach (var line in File.ReadAllLines(InputFilePath))
            foreach (var s in line.Split(separator))
                yield return (T)Convert.ChangeType(s.Trim(), typeof(T), CultureInfo.InvariantCulture);
    }

    protected async Task<string> DownloadInput()
    {
        if (File.Exists(InputFilePath))
        {
            var allText = await File.ReadAllTextAsync(InputFilePath);
            if (allText.Length > 0)
                return allText.TrimEnd(Environment.NewLine.ToCharArray());
        }

        Console.WriteLine("Downloading input!");

        var baseAddress = new Uri("https://adventofcode.com/");
        var cookieContainer = new CookieContainer();
        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = baseAddress };
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("foo", "bar"),
            new KeyValuePair<string, string>("baz", "bazinga"),
        });

        var session = await File.ReadAllTextAsync("SessionToken.txt");
        cookieContainer.Add(baseAddress, new Cookie("session", session));
        var dayNum = int.Parse(GetType().Name.Replace("Day", string.Empty));
        var result = await client.PostAsync($"/{PuzzlesRunner.Year}/day/{dayNum}/input", content);
        result.EnsureSuccessStatusCode();
        var text = await result.Content.ReadAsStringAsync();
        text = text.TrimEnd(Environment.NewLine.ToCharArray());

        var directoryName = Path.GetDirectoryName(InputFilePath) ?? throw new InvalidOperationException();
        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

        await using var streamWriter = File.CreateText(InputFilePath);
        await streamWriter.WriteAsync(text);
        return text;
    }


    #endregion

    #region Runner

    public void Run()
    {
        Console.WriteLine("Running " + GetType().Name);
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine();


        var stopwatch = Stopwatch.StartNew();
        RunInternal();
        stopwatch.Stop();

        Console.WriteLine();
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine(GetType().Name + " run for " + stopwatch.ElapsedMilliseconds + "ms.");
        Console.WriteLine();
    }

    protected abstract void RunInternal();

    #endregion
}