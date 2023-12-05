using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Utils;

public static class Cryptography
{
    public static string ComputeSha256Hash(string rawData)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        StringBuilder builder = new StringBuilder();
        foreach (var t in bytes)
            builder.Append(t.ToString("x2"));
        return builder.ToString();
    }
}