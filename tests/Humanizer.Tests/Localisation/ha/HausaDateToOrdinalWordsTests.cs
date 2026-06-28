namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaDateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2022, 1, 25, "25 Janairu 2022")]
    [InlineData(2015, 2, 3, "3 Fabrairu 2015")]
    public void DateTimeToOrdinalWords_UsesHausaDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Janairu 2022")]
    [InlineData(2015, 2, 3, "3 Fabrairu 2015")]
    public void DateOnlyToOrdinalWords_UsesHausaDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }
#endif
}