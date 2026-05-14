namespace Humanizer.Tests.Localisation.si;

[UseCulture("si")]
public class SinhalaLocaleParityTests
{
    static readonly CultureInfo Si = new("si");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesSinhalaConjunction()
    {
        Assert.Equal("1 සහ 2", Pair.Humanize());
        Assert.Equal("1, 2 සහ 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ඊයේ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 දිනකට පෙර")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "හෙට")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 දිනකින්")]
    public void DateHumanize_UsesSinhalaRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Si);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullDateHumanize_UsesSinhalaNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("කවදාවත් නැහැ", date.Humanize(culture: Si));
    }

    [Theory]
    [InlineData(2, TimeUnit.Day, "2 දින")]
    [InlineData(0, TimeUnit.Second, "0 මිලි තත්පර")]
    public void TimeSpanHumanize_UsesSinhalaDurationPhrases(int count, TimeUnit unit, string expected)
    {
        var span = unit switch
        {
            TimeUnit.Day => TimeSpan.FromDays(count),
            TimeUnit.Second => TimeSpan.FromSeconds(count),
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        Assert.Equal(expected, span.Humanize(culture: Si));
    }

    [Theory]
    [InlineData(DataUnit.Byte, 1, "බයිට්")]
    [InlineData(DataUnit.Byte, 2, "බයිට්")]
    [InlineData(DataUnit.Kilobyte, 2, "කිලෝබයිට්")]
    [InlineData(DataUnit.Megabyte, 2, "මෙගාබයිට්")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Si);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "තත්පරය")]
    [InlineData(TimeUnit.Minute, "මිනිත්තුව")]
    [InlineData(TimeUnit.Hour, "පැය")]
    [InlineData(TimeUnit.Day, "දින")]
    [InlineData(TimeUnit.Year, "අවුරුද්ද")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Si);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "බිංදුව")]
    [InlineData(21, "විසි එක")]
    [InlineData(101, "එකසිය එක")]
    [InlineData(1001, "එක්දහස් එක")]
    [InlineData(1234567, "දොළොස්ලක්ෂ තිස්හතරදහස් පන්සිය හැට හත")]
    public void NumberToWords_ProducesSinhalaCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Si));
    }

    [Theory]
    [InlineData(1000000000, "සියය කෝටි")]
    [InlineData(1010000000, "එකසිය එක කෝටි")]
    [InlineData(1000100000, "සියය කෝටි ලක්ෂය")]
    [InlineData(10000000000, "දාහ කෝටි")]
    [InlineData(10000000000000000, "සියය කෝටි කෝටි")]
    public void NumberToWords_ProducesFallbackScalePhrases(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Si));
    }

    [Theory]
    [InlineData(1, "පළමු")]
    [InlineData(2, "දෙවන")]
    [InlineData(3, "තෙවන")]
    [InlineData(21, "විසි එකවැනි")]
    public void ToOrdinalWords_ProducesSinhalaOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Si));
    }

    [Theory]
    [InlineData("විසි එක", 21)]
    [InlineData("එකසිය එක", 101)]
    [InlineData("දොළොස්ලක්ෂ තිස්හතරදහස් පන්සිය හැට හත", 1234567)]
    [InlineData("සියය කෝටි", 1000000000)]
    [InlineData("එකසිය එක කෝටි", 1010000000)]
    [InlineData("සියය කෝටි ලක්ෂය", 1000100000)]
    [InlineData("දාහ කෝටි", 10000000000)]
    [InlineData("සියය කෝටි කෝටි", 10000000000000000)]
    [InlineData("එකසිය එක කෝටි විසි එක", 1010000021)]
    [InlineData("විසි එකවැනි", 21)]
    [InlineData("ඍණ විසි එක", -21)]
    public void WordsToNumber_ParsesSinhalaCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Si));
    }

    [Theory]
    [InlineData(1000000000)]
    [InlineData(1010000000)]
    [InlineData(1000100001)]
    [InlineData(10000000000)]
    [InlineData(10000000000000000)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void StemmedScaleFallbackPhrases_RoundTrip(long number)
    {
        var words = number.ToWords(Si);
        Assert.Equal(number, words.ToNumber(Si));
    }

    [Theory]
    [InlineData(1, "1වැනි")]
    [InlineData(21, "21වැනි")]
    [InlineData(-1, "-1වැනි")]
    public void Ordinalize_UsesSinhalaNumericSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Si));
    }

    [Fact]
    public void DateToOrdinalWords_UsesSinhalaMonthNames()
    {
        Assert.Equal("25 දුරුතු 2022", new DateTime(2022, 1, 25).ToOrdinalWords());
        Assert.Equal("3 නවම් 2015", new DateTime(2015, 2, 3).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(13, 23, "දවල් එකයි විසි තුන")]
    [InlineData(1, 5, "රාත්‍රී එකයි පහ")]
    public void ToClockNotation_ProducesSinhalaClockPhrases(int hour, int minute, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_RoundsWithSinhalaMinuteWords()
    {
        Assert.Equal("දවල් එකයි විසි පහ", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Fact]
    public void Heading_UsesSinhalaCompassWords()
    {
        Assert.Equal("උතුර", 0d.ToHeading(HeadingStyle.Full, Si));
        Assert.Equal("නැගෙනහිර", 90d.ToHeading(HeadingStyle.Full, Si));
        Assert.Equal("දකුණ", 180d.ToHeading(HeadingStyle.Full, Si));
        Assert.Equal("බස්නාහිර", 270d.ToHeading(HeadingStyle.Full, Si));
    }
}