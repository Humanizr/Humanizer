namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiDateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2022, 1, 25, "25 ਜਨਵਰੀ 2022")]
    [InlineData(2015, 1, 1, "1 ਜਨਵਰੀ 2015")]
    [InlineData(2015, 2, 3, "3 ਫ਼ਰਵਰੀ 2015")]
    [InlineData(2021, 10, 31, "31 ਅਕਤੂਬਰ 2021")]
    [InlineData(2024, 12, 31, "31 ਦਸੰਬਰ 2024")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ਜਨਵਰੀ 2022")]
    [InlineData(2015, 2, 3, "3 ਫ਼ਰਵਰੀ 2015")]
    [InlineData(2021, 10, 31, "31 ਅਕਤੂਬਰ 2021")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }
#endif
}