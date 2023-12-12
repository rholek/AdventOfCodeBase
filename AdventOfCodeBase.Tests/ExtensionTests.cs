using System.Globalization;
using AdventOfCode;
namespace AdventOfCodeBase.Tests;

public class ExtensionTests
{
    [Theory]
    [InlineData("ADD78", 'D', new[] { 1, 2 })]
    [InlineData("123456789", '0', new int[0])]
    public void Direction_GetOppositeTest(string input, char item, int[] indexes)
    {
        input.FindAllIndexes(item).Should().BeEquivalentTo(indexes);
    }

    [Theory]
    [InlineData("7", typeof(int), 7)]
    [InlineData("77", typeof(long), 77L)]
    [InlineData("7.7", typeof(double), 7.7)]
    [InlineData(123, typeof(string), "123")]
    [InlineData("A", typeof(char), 'A')]
    [InlineData('7', typeof(string), "7")]
    [InlineData('7', typeof(int), 7)]
    public void Direction_AsTest(object input, Type castType, object result)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var m = typeof(Extensions).GetMethod("As").MakeGenericMethod(castType);
        m.Invoke(null, new[] { input }).Should().Be(result);
    }
}