using Xunit;

namespace NumberToWordsApi.Tests;
 
public class NumberToWordsConverterTests
{
    [Theory]
    [InlineData("0", "ZERO DOLLARS")]
    [InlineData("1", "ONE DOLLAR")]
    [InlineData("45.00", "FORTY-FIVE DOLLARS")] // no cents when there aren't any
    [InlineData("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS")] // spec's own example
    [InlineData("1.01", "ONE DOLLAR AND ONE CENT")] // singular dollar/cent
    [InlineData("0.05", "ZERO DOLLARS AND FIVE CENTS")]
    [InlineData("0.996", "ONE DOLLAR")] // rounds up and carries into the whole number
    [InlineData("1000000", "ONE MILLION DOLLARS")]
    [InlineData("1234.56", "ONE THOUSAND, TWO HUNDRED AND THIRTY-FOUR DOLLARS AND FIFTY-SIX CENTS")]
    [InlineData("-45.50", "NEGATIVE FORTY-FIVE DOLLARS AND FIFTY CENTS")]
    public void Currency_formatting(string input, string expected)
    {
        Assert.Equal(expected, NumberToWordsConverter.ConvertWithDecimals(decimal.Parse(input)));
    }
}