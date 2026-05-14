namespace Humanizer.Tests.Localisation.bs;

[UseCulture("bs")]
public class BosnianLocaleParityTests
{
    static readonly CultureInfo Bs = new("bs");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionFormatter_UsesBosnianConjunction()
    {
        Assert.Equal("1 i 2", Pair.Humanize());
        Assert.Equal("1, 2 i 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "jučer")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "sutra")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "prije 2 dana")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "za 2 dana")]
    [InlineData(1, TimeUnit.Week, Tense.Past, "prije sedmicu dana")]
    [InlineData(2, TimeUnit.Week, Tense.Future, "za 2 sedmice")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "upravo sada")]
    public void DateHumanize_UsesBosnianPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Bs);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesBosnianNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("nikada", date.Humanize(culture: Bs));
    }

    [Theory]
    [InlineData(1, TimeUnit.Week, false, "1 sedmica")]
    [InlineData(2, TimeUnit.Week, false, "2 sedmice")]
    [InlineData(1, TimeUnit.Week, true, "jedna sedmica")]
    [InlineData(2, TimeUnit.Month, false, "2 mjeseca")]
    [InlineData(0, TimeUnit.Millisecond, true, "nema trajanja")]
    public void TimeSpanHumanize_UsesBosnianDurationForms(int value, TimeUnit unit, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Bs);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, value, toWords: toWords));
    }

    [Fact]
    public void Formatter_UsesBosnianDataAndTimeUnitPhrases()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Bs);

        Assert.Equal("bit", formatter.DataUnitHumanize(DataUnit.Bit, 1, toSymbol: false));
        Assert.Equal("bita", formatter.DataUnitHumanize(DataUnit.Bit, 2, toSymbol: false));
        Assert.Equal("bajta", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("sedmica", formatter.TimeUnitHumanize(TimeUnit.Week));
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Bs));
    }

    [Theory]
    [InlineData(0, "nula")]
    [InlineData(21, "dvadeset jedan")]
    [InlineData(1001, "hiljadu jedan")]
    [InlineData(21000, "dvadeset jedna hiljada")]
    [InlineData(1234567890, "milijarda dvjesto trideset četiri miliona petsto šezdeset sedam hiljada osamsto devedeset")]
    [InlineData(-7516, "minus sedam hiljada petsto šesnaest")]
    public void NumberToWords_UsesBosnianCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Bs));
    }

    [Theory]
    [InlineData("dvadeset jedan", 21)]
    [InlineData("hiljadu jedan", 1001)]
    [InlineData("minus sedam hiljada petsto šesnaest", -7516)]
    public void WordsToNumber_ParsesBosnianCardinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Bs));
        Assert.True(words.TryToNumber(out var parsed, Bs, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("prvi", 1)]
    [InlineData("treći", 3)]
    [InlineData("deseta", 10)]
    public void WordsToNumber_ParsesBosnianOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Bs));
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(21, "21")]
    public void NumberToOrdinalWords_UsesBosnianLocaleConverter(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Bs));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "1.")]
    [InlineData(1, GrammaticalGender.Feminine, "1.")]
    [InlineData(21, GrammaticalGender.Neuter, "21.")]
    [InlineData(-1, GrammaticalGender.Masculine, "-1.")]
    public void Ordinalize_UsesBosnianDotSuffix(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Bs));
        Assert.Equal(expected, number.ToString(Bs).Ordinalize(gender, Bs));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25. januar 2022.")]
    [InlineData(2015, 2, 3, "3. februar 2015.")]
    [InlineData(2024, 12, 31, "31. decembar 2024.")]
    public void DateToOrdinalWords_UsesBosnianDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25. januar 2022.")]
    [InlineData(2024, 12, 31, "31. decembar 2024.")]
    public void DateOnlyToOrdinalWords_UsesBosnianDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(13, 23, ClockNotationRounding.None, "trinaest sati i dvadeset tri minute")]
    [InlineData(13, 23, ClockNotationRounding.NearestFiveMinutes, "trinaest sati i dvadeset pet minuta")]
    [InlineData(1, 5, ClockNotationRounding.None, "jedan sat i pet minuta")]
    public void TimeOnlyToClockNotation_UsesBosnianClockPhrases(int hour, int minute, ClockNotationRounding rounding, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation(rounding));
    }
#endif

    [Fact]
    public void Compass_UsesBosnianHeadings()
    {
        Assert.Equal("sjever", 0d.ToHeading(HeadingStyle.Full, Bs));
        Assert.Equal("S", 0d.ToHeading(HeadingStyle.Abbreviated, Bs));
        Assert.Equal("jugoistok", 135d.ToHeading(HeadingStyle.Full, Bs));
        Assert.Equal("JI", 135d.ToHeading(HeadingStyle.Abbreviated, Bs));
    }
}