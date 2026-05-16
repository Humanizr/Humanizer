namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliDateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2022, 1, 25, "25 Jannaayo 2022")]
    [InlineData(2015, 2, 3, "3 Febraayo 2015")]
    public void DateTimeToOrdinalWords_UsesSomaliDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Jannaayo 2022")]
    [InlineData(2015, 2, 3, "3 Febraayo 2015")]
    public void DateOnlyToOrdinalWords_UsesSomaliDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }
#endif
}