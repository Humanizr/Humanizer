namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduDateToOrdinalWordsTests
{
    [Theory]
    [InlineData(2022, 1, 25, "25 جنوری، 2022")]
    [InlineData(2015, 1, 1, "1 جنوری، 2015")]
    [InlineData(2015, 2, 3, "3 فروری، 2015")]
    [InlineData(2024, 12, 31, "31 دسمبر، 2024")]
    [InlineData(2020, 6, 15, "15 جون، 2020")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        var result = new DateTime(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 جنوری، 2022")]
    [InlineData(2015, 1, 1, "1 جنوری، 2015")]
    [InlineData(2015, 2, 3, "3 فروری، 2015")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        var result = new DateOnly(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
#endif

    [Theory]
    [InlineData(2022, 1, 25, "25 جنوری، 2022")]
    public void UrPk_InheritsDateOutput(int year, int month, int day, string expected)
    {
        using var _ = new CultureSwap(new CultureInfo("ur-PK"));
        var result = new DateTime(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 جنوری، 2022")]
    public void UrIn_InheritsDateOutput(int year, int month, int day, string expected)
    {
        using var _ = new CultureSwap(new CultureInfo("ur-IN"));
        var result = new DateTime(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}