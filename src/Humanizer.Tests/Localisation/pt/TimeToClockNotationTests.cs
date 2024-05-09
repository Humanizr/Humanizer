#if NET6_0_OR_GREATER

namespace pt;

[UseCulture("pt")]
public class TimeToClockNotationTests
{
    [Theory]
    [InlineData(00, 00, "meia-noite")]
    [InlineData(04, 00, "quatro horas")]
    [InlineData(05, 01, "cinco e um")]
    [InlineData(06, 05, "seis e cinco")]
    [InlineData(07, 10, "sete e dez")]
    [InlineData(08, 15, "oito e um quarto")]
    [InlineData(09, 20, "nove e vinte")]
    [InlineData(10, 25, "dez e vinte e cinco")]
    [InlineData(11, 30, "onze e meia")]
    [InlineData(12, 00, "meio-dia")]
    [InlineData(15, 35, "três e trinta e cinco")]
    [InlineData(16, 40, "cinco menos vinte")]
    [InlineData(17, 45, "seis menos um quarto")]
    [InlineData(18, 50, "sete menos dez")]
    [InlineData(19, 55, "oito menos cinco")]
    [InlineData(20, 59, "oito e cinquenta e nove")]
    public void ConvertToClockNotationTimeOnlyStringPtBr(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(00, 00, "meia-noite")]
    [InlineData(04, 00, "quatro horas")]
    [InlineData(05, 01, "cinco horas")]
    [InlineData(06, 05, "seis e cinco")]
    [InlineData(07, 10, "sete e dez")]
    [InlineData(08, 15, "oito e um quarto")]
    [InlineData(09, 20, "nove e vinte")]
    [InlineData(10, 25, "dez e vinte e cinco")]
    [InlineData(11, 30, "onze e meia")]
    [InlineData(12, 00, "meio-dia")]
    [InlineData(13, 23, "uma e vinte e cinco")]
    [InlineData(14, 32, "duas e meia")]
    [InlineData(15, 35, "três e trinta e cinco")]
    [InlineData(16, 40, "cinco menos vinte")]
    [InlineData(17, 45, "seis menos um quarto")]
    [InlineData(18, 50, "sete menos dez")]
    [InlineData(19, 55, "oito menos cinco")]
    [InlineData(20, 59, "nove horas")]
    public void ConvertToRoundedClockNotationTimeOnlyStringPtBr(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expectedResult, actualResult);
    }
}

#endif
