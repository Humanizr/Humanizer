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
    [InlineData(1446, 1, 1, "1 محرم، 1446")]
    [InlineData(1446, 2, 1, "1 صفر، 1446")]
    [InlineData(1446, 3, 1, "1 ربیع الاول، 1446")]
    [InlineData(1446, 4, 1, "1 ربیع الثانی، 1446")]
    [InlineData(1446, 5, 1, "1 جمادی الاول، 1446")]
    [InlineData(1446, 6, 1, "1 جمادی الثانی، 1446")]
    [InlineData(1446, 7, 1, "1 رجب، 1446")]
    [InlineData(1446, 8, 1, "1 شعبان، 1446")]
    [InlineData(1446, 9, 1, "1 رمضان، 1446")]
    [InlineData(1446, 10, 1, "1 شوال، 1446")]
    [InlineData(1446, 11, 1, "1 ذوالقعدہ، 1446")]
    [InlineData(1446, 12, 1, "1 ذوالحجہ، 1446")]
    public void HijriCalendar_ExactOutputPerMonth(int hijriYear, int hijriMonth, int hijriDay, string expected)
    {
        var hijri = new HijriCalendar();
        var date = new DateTime(hijriYear, hijriMonth, hijriDay, hijri);

        using var _ = new CultureSwap(CreateUrduHijriCulture());
        Assert.Equal(expected, date.ToOrdinalWords());
    }

    [Theory]
    [InlineData(1446, 7, 15, "15 رجب، 1446")]
    [InlineData(1446, 12, 10, "10 ذوالحجہ، 1446")]
    public void HijriCalendar_ExactOutputNonFirstDay(int hijriYear, int hijriMonth, int hijriDay, string expected)
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

