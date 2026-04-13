namespace Humanizer.Tests.Localisation.ur;

public class UrduHijriDateTests
{
    static CultureInfo CreateUrduHijriCulture()
    {
        var culture = (CultureInfo)new CultureInfo("ur").Clone();
        culture.DateTimeFormat.Calendar = new HijriCalendar();
        return culture;
    }

    [Theory]
    [InlineData(2025, 1, 1, "محرم")]
    [InlineData(2025, 2, 1, "صفر")]
    [InlineData(2025, 3, 1, "ربیع الاول")]
    [InlineData(2025, 4, 1, "ربیع الثانی")]
    [InlineData(2025, 5, 1, "جمادی الاول")]
    [InlineData(2025, 6, 1, "جمادی الثانی")]
    [InlineData(2025, 7, 1, "رجب")]
    [InlineData(2025, 8, 1, "شعبان")]
    [InlineData(2025, 9, 1, "رمضان")]
    [InlineData(2025, 10, 1, "شوال")]
    [InlineData(2025, 11, 1, "ذوالقعدہ")]
    [InlineData(2025, 12, 1, "ذوالحجہ")]
    public void HijriCalendar_UsesHijriMonthNames(int hijriYear, int hijriMonth, int hijriDay, string expectedMonthName)
    {
        var hijri = new HijriCalendar();
        var date = new DateTime(hijriYear, hijriMonth, hijriDay, hijri);

        using var _ = new CultureSwap(CreateUrduHijriCulture());
        var result = date.ToOrdinalWords();
        Assert.Contains(expectedMonthName, result);
    }

    [Theory]
    [InlineData(1446, 7, 15, "15 رجب، 1446")]
    [InlineData(1446, 1, 1, "1 محرم، 1446")]
    [InlineData(1446, 9, 1, "1 رمضان، 1446")]
    [InlineData(1446, 12, 10, "10 ذوالحجہ، 1446")]
    public void HijriCalendar_ExactOutput(int hijriYear, int hijriMonth, int hijriDay, string expected)
    {
        var hijri = new HijriCalendar();
        var date = new DateTime(hijriYear, hijriMonth, hijriDay, hijri);

        using var _ = new CultureSwap(CreateUrduHijriCulture());
        Assert.Equal(expected, date.ToOrdinalWords());
    }

    [Fact]
    public void GregorianCalendar_UnaffectedByHijriMonths()
    {
        using var _ = new CultureSwap(new CultureInfo("ur"));
        var date = new DateTime(2025, 1, 15);
        var result = date.ToOrdinalWords();
        Assert.Contains("جنوری", result);
        Assert.Equal("15 جنوری، 2025", result);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(1446, 7, 15, "15 رجب، 1446")]
    [InlineData(1446, 1, 1, "1 محرم، 1446")]
    [InlineData(1446, 9, 1, "1 رمضان، 1446")]
    public void DateOnly_HijriCalendar_ExactOutput(int hijriYear, int hijriMonth, int hijriDay, string expected)
    {
        var hijri = new HijriCalendar();
        var dateTime = new DateTime(hijriYear, hijriMonth, hijriDay, hijri);
        var date = DateOnly.FromDateTime(dateTime);

        using var _ = new CultureSwap(CreateUrduHijriCulture());
        Assert.Equal(expected, date.ToOrdinalWords());
    }
#endif

    [Fact]
    public void HijriCalendar_UrPk_InheritsHijriMonths()
    {
        var urPkHijri = (CultureInfo)new CultureInfo("ur-PK").Clone();
        urPkHijri.DateTimeFormat.Calendar = new HijriCalendar();

        var hijri = new HijriCalendar();
        var date = new DateTime(1446, 9, 1, hijri);

        using var _ = new CultureSwap(urPkHijri);
        var result = date.ToOrdinalWords();
        Assert.Contains("رمضان", result);
    }

    [Fact]
    public void HijriCalendar_UrIn_InheritsHijriMonths()
    {
        var urInHijri = (CultureInfo)new CultureInfo("ur-IN").Clone();
        urInHijri.DateTimeFormat.Calendar = new HijriCalendar();

        var hijri = new HijriCalendar();
        var date = new DateTime(1446, 1, 1, hijri);

        using var _ = new CultureSwap(urInHijri);
        var result = date.ToOrdinalWords();
        Assert.Contains("محرم", result);
    }
}

