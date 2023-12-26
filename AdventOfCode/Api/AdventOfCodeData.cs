using System.Text.Json;
using HtmlAgilityPack;

namespace AdventOfCode.Api;

public class AdventOfCodeData
{
    #region Fields

    private readonly AdventOfCodeWebsite website;
    private readonly string tmpFolder;

    #endregion

    #region Constructor

    public AdventOfCodeData(int day)
    {
        var session = File.ReadAllText("SessionToken.txt");
        website = new AdventOfCodeWebsite(day, session);

        var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"AdventOfCode{PuzzlesRunner.Year}");
        if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(rootPath);
        var tokenToUserIdJson = Path.Combine(rootPath, "tokenToUser.json");
        if (!File.Exists(tokenToUserIdJson))
            File.WriteAllText(tokenToUserIdJson, "{}");

        var tokens = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(tokenToUserIdJson)) ?? throw new InvalidOperationException();
        var hash = Cryptography.ComputeSha256Hash(session);
        if (!tokens.ContainsKey(hash))
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(website.Get(""));
            tokens[hash] = doc.DocumentNode.SelectNodes("//div").First(x => x.HasClass("user")).GetDirectInnerText().Trim();
            File.WriteAllText(tokenToUserIdJson, JsonSerializer.Serialize(tokens));
        }

        var userName = tokens[hash];
        tmpFolder = Path.Combine(rootPath, userName, day.ToString());
        if (!Directory.Exists(tmpFolder))
            Directory.CreateDirectory(tmpFolder);
    }

    #endregion

    #region Properties

    public AnswerChecker Answers => new(tmpFolder, website);

    public Input RealInput => new(() => GetFromCachedFile("RealData", () => website.Get("/input")));

    public Input TestInput
    {
        get
        {
            return new Input(() => GetFromCachedFile("TestInputPart1", GetPart1), () => GetFromCachedFile("TestInputPart2", GetPart2));

            string GetPart1()
            {
                var html = website.Get("");
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var main = doc.DocumentNode.SelectSingleNode("//main");
                var allNodes = main.SelectNodes("//pre/code");
                if (allNodes.Count <= 0)
                    throw new Exception("No input found!");
                if (allNodes.Count > 2)
                {
                    "Found multiple inputs:".Dump(ConsoleColor.Yellow);
                    foreach (var node in allNodes)
                    {
                        node.InnerText.TrimEnd().Dump(ConsoleColor.DarkMagenta);
                        if ("Should use this?".Ask(ConsoleColor.DarkCyan))
                            return node.InnerText;
                    }

                    throw new Exception("No input found!");
                }

                return allNodes.First().InnerText;
            }

            string GetPart2()
            {
                var html = website.Get("");
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var main = doc.DocumentNode.SelectSingleNode("//main");
                var allNodes = main.SelectNodes("//pre/code");
                if (allNodes.Count <= 0)
                    throw new Exception("No input found!");
                if (allNodes.Count > 2)
                {
                    "Found multiple inputs using:".Dump(ConsoleColor.Yellow);
                    allNodes.First().InnerText.Dump();
                }

                return allNodes.Last().InnerText;
            }
        }
    }

    #endregion

    #region Private methods

    private string GetFromCachedFile(string filename, Func<string> factory)
    {
        var path = Path.Combine(tmpFolder, $"{filename}.txt");
        if (File.Exists(path))
        {
            var data = File.ReadAllText(path);
            if (!data.IsNullOrEmpty())
                return data;
        }

        var createdData = factory();
        File.WriteAllText(path, createdData);
        return createdData;
    }

    #endregion
}