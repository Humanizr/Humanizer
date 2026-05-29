namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliDateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2022, 1, 25, "25 Januari 2022")]
    [InlineData(2015, 2, 3, "3 Februari 2015")]
    public void DateTimeToOrdinalWords_UsesSwahiliDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Januari 2022")]
    [InlineData(2015, 2, 3, "3 Februari 2015")]
    public void DateOnlyToOrdinalWords_UsesSwahiliDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }
#endif
}