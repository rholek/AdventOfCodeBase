using HtmlAgilityPack;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode.Api;

public class AnswerChecker
{
    private readonly AdventOfCodeWebsite httpClient;
    private readonly string filePath;

    internal AnswerChecker(string tmpFolder, AdventOfCodeWebsite httpClient)
    {
        this.httpClient = httpClient;
        filePath = Path.Combine(tmpFolder, "answers.json");
    }

    public bool CheckTestResult(string answer, int level, string? expectedAnswer = null)
    {
        var expected = expectedAnswer ?? LoadExpectedTestResult(level);
        if (expected == answer)
        {
            $"""
                 Test result '{answer}' for part {level} is correct
                 """.Dump(ConsoleColor.Green);
        }
        else
        {
            return $"""
                 Test input for part {level} is incorrect
                 Expected: {expected}
                 Actual: {answer}
                 Should continue?
                 """.Ask(ConsoleColor.Red);
        }

        return true;
    }

    public void CheckRealResult(string answer, int level)
    {
        WithAnswers(answers =>
        {
            var answerForLevel = answers[level];
            if (answerForLevel.Rejected.Contains(answer))
            {
                $"Answer for part {level} '{answer}' was already rejected".Dump(ConsoleColor.Red);
                return;
            }

            if (answerForLevel.Real == answer)
            {
                $"Answer for part {level} '{answer}' was already accepted".Dump(ConsoleColor.Green);
                return;
            }

        submit:

            if (!$"Should submit answer for part {level} '{answer}'?".Ask(ConsoleColor.Yellow))
            {
                "Answer NOT submitted".Dump(ConsoleColor.DarkMagenta);
                return;
            }

            "Submitting answer".Dump();
            var result = httpClient.Send("/answer", new Dictionary<string, string> { { "level", level.ToString() }, { "answer", answer } });

            //already done or correct
            if (result.Contains("You don't seem to be solving the right level.  Did you already complete it?"))
            {
                "This was already answered".Dump(ConsoleColor.Yellow);
                var realAnswer = LoadKnownRealDataResult(level) ?? throw new Exception("Already answered but failed to load the result;");
                answerForLevel.Real = realAnswer;
                return;
            }

            if (result.Contains("That's the right answer"))
            {
                "That's the right answer".Dump(ConsoleColor.Green);
                answerForLevel.Real = answer;
                return;
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);
            var responseText = htmlDoc.DocumentNode.SelectSingleNode("//article").InnerText;

            if (responseText.Contains("You gave an answer too recently"))
            {
                "Wait before answering".Dump(ConsoleColor.Yellow);
                responseText.Dump(ConsoleColor.Yellow);

                var match = Regex.Match(responseText, @"You have ((?<minutes>\d+)m )?((?<seconds>\d+)s )?left to wait");
                if (match.Success)
                {
                    var minutes = match.Groups["minutes"].Value.IsNullOrEmpty() ? 0 : match.Groups["minutes"].Value.AsInt();
                    var seconds = minutes * 60 + (match.Groups["seconds"].Value.IsNullOrEmpty() ? 0 : match.Groups["seconds"].Value.AsInt());

                    for (int i = seconds; i > 0; i--)
                    {
                        $"Wait another {i} seconds".Dump();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Thread.Sleep(1000);
                    }

                    goto submit;
                }

                return;
            }

            "That was not a correct answer!".Dump(ConsoleColor.Red);
            responseText.Dump(ConsoleColor.Red);
            answerForLevel.Rejected.Add(answer);
        });
    }

    private string LoadExpectedTestResult(int level)
    {
        return WithAnswers(answers =>
        {
            var answerForLevel = answers[level];
            if (answerForLevel.Test is not null)
                return answerForLevel.Test;

            var html = httpClient.Get("");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var main = level switch
            {
                1 => doc.DocumentNode.SelectNodes("//article").First(),
                2 => doc.GetElementbyId("part2")?.ParentNode ?? throw new Exception("Part 2 not found!"),
                _ => throw new InvalidOperationException()
            };

            var allNodes = main.SelectNodes("//code/em").Where(x => x.Ancestors().Contains(main)).ToList();
            if (allNodes.Count <= 0)
                throw new Exception("No answers found!");

            answerForLevel.Test = allNodes.Last().InnerText;

            //check if there are known result for real data data
            answerForLevel.Real = ExtractRealDataResult(doc, level);

            return answerForLevel.Test;
        });
    }

    private string? ExtractRealDataResult(HtmlDocument doc, int level)
    {
        var nodes = doc.DocumentNode
            .SelectNodes("//p[contains(., 'Your puzzle answer was')]")?
            .Select(x => x.Descendants("code").Single().InnerText);
        return nodes?.ElementAtOrDefault(level - 1);
    }

    private string? LoadKnownRealDataResult(int level)
    {
        var html = httpClient.Get("");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return ExtractRealDataResult(doc, level);
    }

    private void WithAnswers(Action<AnswersForBothParts> action) => WithAnswers(x =>
    {
        action(x);
        return new object();
    });

    private T WithAnswers<T>(Func<AnswersForBothParts, T> action)
    {
        var answers = Read();
        var result = action(answers);
        Write();
        return result;

        AnswersForBothParts Read()
        {
            if (File.Exists(filePath))
                return JsonSerializer.Deserialize<AnswersForBothParts>(File.ReadAllText(filePath)) ?? throw new Exception("Failed to read answers");
            return new AnswersForBothParts();
        }

        void Write()
        {
            var serialized = JsonSerializer.Serialize(answers);
            File.WriteAllText(filePath, serialized);
        }
    }

}