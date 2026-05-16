namespace Humanizer.Tests.Localisation.tk;

[UseCulture("tk")]
public class TurkmenLocaleParityTests
{
    static readonly CultureInfo Tk = new("tk");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesTurkmenConjunction()
    {
        Assert.Equal("1 we 2", Pair.Humanize());
        Assert.Equal("1, 2 we 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(0, TimeUnit.Second, Tense.Future, "häzir")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "düýn")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ertir")]
    [InlineData(1, TimeUnit.Second, Tense.Past, "bir sekunt öň")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 gün öň")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 gün soň")]
    [InlineData(1, TimeUnit.Minute, Tense.Future, "bir minut soň")]
    public void DateHumanize_UsesTurkmenRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Tk);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesTurkmenNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("hiç haçan", date.Humanize(culture: Tk));
    }

    [Theory]
    [InlineData(TimeUnit.Hour, 1, true, "bir sagat")]
    [InlineData(TimeUnit.Day, 2, false, "2 gün")]
    [InlineData(TimeUnit.Week, 2, false, "2 hepde")]
    public void TimeSpanHumanize_UsesTurkmenDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Tk);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void TimeSpanHumanize_ComposesTurkmenDurationsWithTurkmenListJoiner()
    {
        Assert.Equal("2 hepde, 1 gün we 1 sagat", TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: Tk, collectionSeparator: null));
    }

    [Fact]
    public void ToAge_UsesTurkmenAgeTemplate()
    {
        Assert.Equal("1 ýyl ýaşynda", TimeSpan.FromDays(366).ToAge(Tk));
    }

    [Theory]
    [InlineData(DataUnit.Bit, "bit")]
    [InlineData(DataUnit.Byte, "baýt")]
    [InlineData(DataUnit.Kilobyte, "kilobaýt")]
    [InlineData(DataUnit.Megabyte, "megabaýt")]
    public void DataUnitHumanize_UsesTurkmenNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Tk);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void ByteSizeToFullWords_UsesTurkmenDataUnits()
    {
        Assert.Equal("1 bit", ByteSize.FromBits(1).ToFullWords(provider: Tk));
        Assert.Equal("1 baýt", ByteSize.FromBytes(1).ToFullWords(provider: Tk));
        Assert.Equal("2 kilobaýt", ByteSize.FromKilobytes(2).ToFullWords(provider: Tk));
    }

    [Fact]
    public void ByteSizeHumanize_UsesTurkmenNumberFormatting()
    {
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Tk));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "ms")]
    [InlineData(TimeUnit.Second, "sekunt")]
    [InlineData(TimeUnit.Minute, "minut")]
    [InlineData(TimeUnit.Hour, "sagat")]
    [InlineData(TimeUnit.Day, "gün")]
    [InlineData(TimeUnit.Week, "hepde")]
    [InlineData(TimeUnit.Month, "aý")]
    [InlineData(TimeUnit.Year, "ýyl")]
    public void TimeUnitHumanize_UsesTurkmenLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Tk);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "nol")]
    [InlineData(1, "bir")]
    [InlineData(21, "ýigrimi bir")]
    [InlineData(105, "ýüz bäş")]
    [InlineData(1234, "müň iki ýüz otuz dört")]
    [InlineData(1001001, "bir million müň bir")]
    [InlineData(-5, "minus bäş")]
    public void NumberToWords_ProducesTurkmenCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Tk));
    }

    [Theory]
    [InlineData(1, "birinji")]
    [InlineData(2, "ikinji")]
    [InlineData(3, "üçünji")]
    [InlineData(4, "dördünji")]
    [InlineData(6, "altynjy")]
    [InlineData(9, "dokuzynjy")]
    [InlineData(10, "onunjy")]
    [InlineData(20, "ýigriminji")]
    [InlineData(21, "ýigrimi birinji")]
    [InlineData(30, "otuzynjy")]
    [InlineData(101, "ýüz birinji")]
    [InlineData(100, "ýüzünji")]
    [InlineData(200, "iki ýüzünji")]
    [InlineData(1000000, "bir millionunjy")]
    public void NumberToOrdinalWords_ProducesTurkmenOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Tk));
    }

    [Theory]
    [InlineData("ýigrimi bir", 21)]
    [InlineData("ýigrimi birinji", 21)]
    [InlineData("otuzynjy", 30)]
    [InlineData("ýüz birinji", 101)]
    [InlineData("iki ýüzünji", 200)]
    [InlineData("bir millionunjy", 1000000)]
    [InlineData("minus bäş", -5)]
    [InlineData("bir million müň bir", 1001001)]
    public void WordsToNumber_ParsesTurkmenCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Tk));
        Assert.True(words.TryToNumber(out var parsed, Tk, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, "1-nji")]
    [InlineData(3, "3-nji")]
    [InlineData(6, "6-njy")]
    [InlineData(10, "10-njy")]
    [InlineData(20, "20-nji")]
    [InlineData(120, "120-nji")]
    [InlineData(200, "200-nji")]
    [InlineData(1000000, "1000000-njy")]
    [InlineData(1000000000, "1000000000-njy")]
    [InlineData(-6, "-6-njy")]
    public void Ordinalize_UsesTurkmenNumericSuffixes(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Tk));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(Tk));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 Ýanwar 2022")]
    [InlineData(2015, 2, 3, "3 Fewral 2015")]
    [InlineData(2024, 12, 31, "31 Dekabr 2024")]
    public void DateTimeToOrdinalWords_UsesTurkmenDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Ýanwar 2022")]
    [InlineData(2015, 2, 3, "3 Fewral 2015")]
    [InlineData(2024, 12, 31, "31 Dekabr 2024")]
    public void DateOnlyToOrdinalWords_UsesTurkmenDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "bir sagat")]
    [InlineData(1, 5, "bir sagat bäş minut")]
    [InlineData(13, 23, "on üç sagat ýigrimi üç minut")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("on üç sagat ýigrimi bäş minut", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "demirgazyk")]
    [InlineData(45.0, "demirgazyk-gündogar")]
    [InlineData(90.0, "gündogar")]
    [InlineData(135.0, "günorta-gündogar")]
    [InlineData(180.0, "günorta")]
    [InlineData(225.0, "günorta-günbatar")]
    [InlineData(270.0, "günbatar")]
    [InlineData(315.0, "demirgazyk-günbatar")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "D")]
    [InlineData(45.0, "DGd")]
    [InlineData(90.0, "Gd")]
    [InlineData(180.0, "G")]
    [InlineData(270.0, "Gb")]
    public void Compass_AbbreviatedDirectionsUseTurkmenLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}