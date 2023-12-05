using System.Net;

namespace AdventOfCode.Api;

internal class AdventOfCodeWebsite
{
    private readonly HttpClient httpClient;
    private readonly string baseAddress;

    public AdventOfCodeWebsite(int day, string sessionToken)
    {
        var cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler();
        handler.CookieContainer = cookieContainer;
        httpClient = new HttpClient(handler);
        baseAddress = $"https://adventofcode.com/{PuzzlesRunner.Year}/day/{day}";
        cookieContainer.Add(new Uri("https://adventofcode.com/"), new Cookie("session", sessionToken));
    }

    public string Get(string path)
    {
        return httpClient.GetStringAsync(baseAddress + path).Result;
    }

    public string Send(string path, Dictionary<string, string> values)
    {
        var content = new FormUrlEncodedContent(values);
        var result = httpClient.PostAsync(baseAddress + path, content).Result;
        return result.Content.ReadAsStringAsync().Result;
    }
}