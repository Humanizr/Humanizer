namespace ar;

[UseCulture("ar")]
public class DateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2015, 1, 1, "1 يناير 2015")]
    [InlineData(2024, 12, 31, "31 ديسمبر 2024")]
    public void DateTimeToOrdinalWords_UsesArabicDatePattern(int year, int month, int day, string expected)
    {
        var result = new DateTime(year, month, day).ToOrdinalWords();

        Assert.Equal(expected, result);
        Assert.DoesNotContain('\u200e', result);
        Assert.DoesNotContain('\u200f', result);
        Assert.DoesNotContain('\u061c', result);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2015, 1, 1, "1 يناير 2015")]
    [InlineData(2024, 12, 31, "31 ديسمبر 2024")]
    public void DateOnlyToOrdinalWords_UsesArabicDatePattern(int year, int month, int day, string expected)
    {
        var result = new DateOnly(year, month, day).ToOrdinalWords();

        Assert.Equal(expected, result);
        Assert.DoesNotContain('\u200e', result);
        Assert.DoesNotContain('\u200f', result);
        Assert.DoesNotContain('\u061c', result);
    }
#endif
}
