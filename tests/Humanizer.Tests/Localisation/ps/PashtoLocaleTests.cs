namespace Humanizer.Tests.Localisation.ps;

[UseCulture("ps")]
public class PashtoLocaleTests
{
    static readonly CultureInfo Ps = new("ps");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionFormatter_UsesPashtoConjunction()
    {
        Assert.Equal("1 او 2", Pair.Humanize());
        Assert.Equal("1, 2 او 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "یوه ثانیه مخکې")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "په 2 ثانیو کې")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "پرون")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "سبا")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ورځې مخکې")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "په 2 ورځو کې")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "اوس")]
    public void RelativeDate_ExactOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var result = Configurator.Formatters.ResolveForCulture(Ps).DateHumanize(unit, tense, count);
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, "1 ثانیه")]
    [InlineData(2, "2 ثانیې")]
    [InlineData(1, "1 ورځ")]
    [InlineData(2, "2 ورځې")]
    public void Duration_ExactOutput(int value, string expected)
    {
        var result = TimeSpan.FromSeconds(value).Humanize(culture: Ps);
        if (value is 1 or 2 && expected.Contains("ورځ", StringComparison.Ordinal))
        {
            result = TimeSpan.FromDays(value).Humanize(culture: Ps);
        }

        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void NullDate_UsesPashtoNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("هېڅکله", date.Humanize(culture: Ps));
    }

    [Theory]
    [InlineData(0, "صفر")]
    [InlineData(21, "یوویشت")]
    [InlineData(99, "نهه نوي")]
    [InlineData(100, "سل")]
    [InlineData(101, "سل او یو")]
    [InlineData(1234567, "یو میلیون او دوه سوه او څلور دېرش زره او پنځه سوه او اووه شپېته")]
    public void NumberToWords_ProducesPashtoCardinals(long number, string expected)
    {
        var result = number.ToWords(Ps);
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "یو")]
    [InlineData(1, GrammaticalGender.Feminine, "یوه")]
    [InlineData(101, GrammaticalGender.Feminine, "سل او یوه")]
    public void NumberToWords_HonorsPashtoCardinalGender(long number, GrammaticalGender gender, string expected)
    {
        var result = number.ToWords(gender, Ps);
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "لومړی")]
    [InlineData(1, GrammaticalGender.Feminine, "لومړۍ")]
    [InlineData(21, GrammaticalGender.Masculine, "یوویشتم")]
    [InlineData(21, GrammaticalGender.Feminine, "یوویشتمه")]
    public void Ordinals_UsePashtoWordForms(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ps));
        Assert.Equal(expected, number.Ordinalize(gender, Ps));
    }

    [Theory]
    [InlineData("یوویشت", 21)]
    [InlineData("سل او دوه ویشت", 122)]
    [InlineData("دوه زره او درې ویشت", 2023)]
    [InlineData("سل او یو", 101)]
    [InlineData("یوویشتمه", 21)]
    [InlineData("دوه ویشتم", 22)]
    [InlineData("دوه ویشتمه", 22)]
    [InlineData("منفي پنځه", -5)]
    public void WordsToNumber_ParsesPashtoCardinalAndOrdinalTokens(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ps));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 جنوري 2022")]
    [InlineData(2015, 2, 3, "3 فبروري 2015")]
    [InlineData(2024, 12, 31, "31 دسمبر 2024")]
    public void DateTime_ToOrdinalWords_UsesPashtoGregorianMonthNames(int year, int month, int day, string expected)
    {
        var result = new DateTime(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 جنوري 2022")]
    [InlineData(2015, 2, 3, "3 فبروري 2015")]
    public void DateOnly_ToOrdinalWords_UsesPashtoGregorianMonthNames(int year, int month, int day, string expected)
    {
        var result = new DateOnly(year, month, day).ToOrdinalWords();
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, 5, "د شپې یوه بجه او پنځه دقیقې")]
    [InlineData(1, 1, "د شپې یوه بجه او یوه دقیقه")]
    [InlineData(13, 23, "د غرمې یوه بجه او درې ویشت دقیقې")]
    [InlineData(13, 23, "د غرمې یوه بجه او پنځه ویشت دقیقې", true)]
    [InlineData(18, 0, "ماښام شپږ بجې")]
    public void TimeOnly_ToClockNotation_ExactOutput(int hour, int minute, string expected, bool rounded = false)
    {
        var time = new TimeOnly(hour, minute);
        var result = rounded ? time.ToClockNotation(ClockNotationRounding.NearestFiveMinutes) : time.ToClockNotation();
        Assert.Equal(expected, result);
        PashtoBidiControlSweep.AssertNoBidiControls(result);
    }
#endif

    [Theory]
    [InlineData(0.0, "شمال")]
    [InlineData(45.0, "شمال ختیځ")]
    [InlineData(90.0, "ختیځ")]
    [InlineData(180.0, "جنوب")]
    [InlineData(270.0, "لوېدیځ")]
    public void Compass_UsesPashtoDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Ps));
    }

    [Fact]
    public void NumberFormatting_UsesStablePashtoSeparatorsWithoutBidiControls()
    {
        var byteResult = 2000.Bytes().Humanize(Ps);
        var metricResult = (-1234L).ToMetric(decimals: 1);

        Assert.Equal("1٫95 KB", byteResult);
        Assert.Equal("-1٫2k", metricResult);
        PashtoBidiControlSweep.AssertNoBidiControls(byteResult);
        PashtoBidiControlSweep.AssertNoBidiControls(metricResult);
    }
}