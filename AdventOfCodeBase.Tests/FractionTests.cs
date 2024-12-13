namespace AdventOfCodeBase.Tests;

public class FractionTests
{
    [Theory]
    [InlineData("1/2", "1/2", "1")]
    [InlineData("10", "15", "25")]
    [InlineData("10", "-15", "-5")]
    public void Fraction_AddTest(string first, string second, string result)
    {
        (Fraction.Parse(first) + Fraction.Parse(second)).Should().Be(Fraction.Parse(result));
    }

    [Theory]
    [InlineData("1/2", "1/2", "0")]
    [InlineData("10", "15", "-5")]
    [InlineData("10", "-15", "25")]
    public void Fraction_SubtractTest(string first, string second, string result)
    {
        (Fraction.Parse(first) - Fraction.Parse(second)).Should().Be(Fraction.Parse(result));
    }

    [Theory]
    [InlineData("1/2", "1/2", "1/4")]
    [InlineData("10", "15", "150")]
    [InlineData("2/3", "-1", "-2/3")]
    public void Fraction_MultiplyTest(string first, string second, string result)
    {
        (Fraction.Parse(first) * Fraction.Parse(second)).Should().Be(Fraction.Parse(result));
    }

    [Theory]
    [InlineData("1/2", "1/2", "1")]
    [InlineData("10", "15", "2/3")]
    [InlineData("2/3", "-1", "-2/3")]
    [InlineData("-1/2", "-1/2", "1")]
    public void Fraction_DivideTest(string first, string second, string result)
    {
        (Fraction.Parse(first) / Fraction.Parse(second)).Should().Be(Fraction.Parse(result));
    }

    [Theory]
    [InlineData("4/2", true)]
    [InlineData("4/4", true)]
    [InlineData("2/4", false)]
    [InlineData("15/5", true)]
    [InlineData("268/111", false)]
    public void Fraction_IsWholeNumberTest(string fraction, bool isWholeNumber)
    {
        Fraction.Parse(fraction).IsWholeNumber().Should().Be(isWholeNumber);
    }
}