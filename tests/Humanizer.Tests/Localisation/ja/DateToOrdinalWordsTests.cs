#if NET6_0_OR_GREATER

namespace ja;

[UseCulture("ja-JP")]
public class DateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2015, 1, 1, "2015年1月1日")]
    [InlineData(2024, 12, 31, "2024年12月31日")]
    public void DateTimeToOrdinalWords_UsesJapaneseDatePattern(int year, int month, int day, string expected) =>
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());

    [Theory]
    [InlineData(2015, 1, 1, "2015年1月1日")]
    [InlineData(2024, 12, 31, "2024年12月31日")]
    public void DateOnlyToOrdinalWords_UsesJapaneseDatePattern(int year, int month, int day, string expected) =>
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
}

#endif
