namespace AdventOfCodeBase.Tests;

public class ExtraMathTests
{
    [Theory]
    [InlineData(15, new long[] { 3, 5 })]
    [InlineData(54879, new long[] { 3, 11, 1663 })]
    [InlineData(164637, new long[] { 3, 3, 11, 1663 })]
    public void ExtraMath_AllPrimeFactorsTest(long number, long[] factors)
    {
        number.AllPrimeFactors().Should().BeEquivalentTo(factors);
    }

    [Theory]
    [InlineData(-5, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(5, true)]
    [InlineData(15, false)]
    [InlineData(10000019, true)]
    public void ExtraMath_IsPrimeTest(long number, bool isPrime)
    {
        number.IsPrime().Should().Be(isPrime);
    }
}