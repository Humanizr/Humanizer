namespace Humanizer.Tests.Localisation.ga;

[UseCulture("ga")]
public class IrishLocaleParityTests
{
    static readonly CultureInfo Irish = CultureInfo.GetCultureInfo("ga");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesIrishConjunction()
    {
        Assert.Equal("1 agus 2", Pair.Humanize());
        Assert.Equal("1, 2 agus 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(0, TimeUnit.Second, Tense.Future, "anois")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "inné")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "amárach")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "bliain ó shin")]
    [InlineData(1, TimeUnit.Year, Tense.Future, "i gceann bliana")]
    [InlineData(2, TimeUnit.Week, Tense.Past, "2 sheachtain ó shin")]
    [InlineData(3, TimeUnit.Year, Tense.Future, "i gceann 3 bliana")]
    [InlineData(11, TimeUnit.Year, Tense.Future, "i gceann 11 bliain")]
    public void DateHumanize_UsesIrishRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Irish);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesIrishNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("riamh", date.Humanize(culture: Irish));
    }

    [Theory]
    [InlineData(TimeUnit.Hour, 2, false, "2 uair an chloig")]
    [InlineData(TimeUnit.Hour, 2, true, "a dó uair an chloig")]
    [InlineData(TimeUnit.Year, 3, false, "3 bliana")]
    [InlineData(TimeUnit.Year, 11, false, "11 bliain")]
    public void TimeSpanHumanize_UsesIrishDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Irish);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void TimeSpanHumanize_ComposesIrishDurationsWithIrishListJoiner()
    {
        Assert.Equal("2 sheachtain, 1 lá agus 1 uair an chloig", TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: Irish, collectionSeparator: null));
        Assert.Equal("1 lá, 3 nóiméad agus 4 soicind", new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: false, culture: Irish, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null));
    }

    [Fact]
    public void ToAge_UsesIrishAgeTemplate() =>
        Assert.Equal("1 bhliain d'aois", TimeSpan.FromDays(366).ToAge(Irish));

    [Fact]
    public void ByteSizeAndTimeUnitSymbols_UseIrishUnits()
    {
        Assert.Equal("1 giotán", ByteSize.FromBits(1).ToFullWords(provider: Irish));
        Assert.Equal("1 beart", ByteSize.FromBytes(1).ToFullWords(provider: Irish));
        Assert.Equal("2 cilibheart", ByteSize.FromKilobytes(2).ToFullWords(provider: Irish));
        Assert.Equal("1.95 KB", ByteSize.FromBytes(2000).Humanize("KB", Irish));
        Assert.Equal("nóim", TimeUnit.Minute.ToSymbol(Irish));
        Assert.Equal("bl.", TimeUnit.Year.ToSymbol(Irish));
    }

    [Theory]
    [InlineData(0, "náid")]
    [InlineData(1, "a haon")]
    [InlineData(21, "fiche agus a haon")]
    [InlineData(42, "daichead agus a dó")]
    [InlineData(101, "a haon céad agus a haon")]
    [InlineData(1001, "a haon míle agus a haon")]
    [InlineData(11000, "a haon déag míle")]
    [InlineData(12000, "a dó dhéag míle")]
    [InlineData(-5, "lúide a cúig")]
    public void NumberToWords_ProducesIrishCardinals(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(Irish));

    [Theory]
    [InlineData(1, "céad")]
    [InlineData(2, "dara")]
    [InlineData(3, "tríú")]
    [InlineData(20, "fichiú")]
    [InlineData(21, "fiche agus aonú")]
    [InlineData(42, "daichead agus dara")]
    [InlineData(100, "céadú")]
    [InlineData(101, "céad agus aonú")]
    [InlineData(1000, "míliú")]
    [InlineData(1000000, "milliúnú")]
    public void NumberToOrdinalWords_ProducesIrishOrdinals(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(Irish));

    [Theory]
    [InlineData("fiche agus a haon", 21)]
    [InlineData("daichead agus a dó", 42)]
    [InlineData("a haon céad", 100)]
    [InlineData("céad", 100)]
    [InlineData("a haon déag míle", 11000)]
    [InlineData("a dó dhéag míle", 12000)]
    [InlineData("a naoi déag míle", 19000)]
    [InlineData("lúide a cúig", -5)]
    [InlineData("aonú", 1)]
    [InlineData("fiche agus aonú", 21)]
    [InlineData("céad agus aonú", 101)]
    [InlineData("céadú", 100)]
    [InlineData("míliú", 1000)]
    [InlineData("milliúnú", 1000000)]
    public void ToNumber_ParsesIrishCardinalAndOrdinalForms(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(Irish));

    [Theory]
    [InlineData(21)]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(1000)]
    [InlineData(1000000)]
    [InlineData(11000)]
    [InlineData(12000)]
    [InlineData(19000)]
    public void ToNumber_RoundTripsGeneratedIrishCardinalScaleForms(long number) =>
        Assert.Equal(number, number.ToWords(Irish).ToNumber(Irish));

    [Theory]
    [InlineData(21)]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(1000)]
    [InlineData(1000000)]
    public void ToNumber_RoundTripsGeneratedIrishExactScaleOrdinals(int number) =>
        Assert.Equal(number, number.ToOrdinalWords(Irish).ToNumber(Irish));

    [Theory]
    [InlineData(1, "1ú")]
    [InlineData(2, "2ú")]
    [InlineData(21, "21ú")]
    [InlineData(-1, "-1ú")]
    public void Ordinalize_UsesIrishNumericOrdinalSuffix(int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(Irish));

    [Fact]
    public void DateToOrdinalWords_UsesIrishDatePatternAndMonthNames() =>
        Assert.Equal("25 Eanáir 2022", new DateTime(2022, 1, 25).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWords_UsesIrishDatePatternAndMonthNames() =>
        Assert.Equal("31 Nollaig 2024", new DateOnly(2024, 12, 31).ToOrdinalWords());

    [Theory]
    [InlineData(3, 15, "ceathrú tar éis a trí")]
    [InlineData(19, 40, "fiche chun a hocht")]
    [InlineData(13, 23, "fiche agus a trí tar éis a haon")]
    public void ToClockNotation_UsesIrishClockPhrases(int hour, int minute, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());

    [Fact]
    public void ToClockNotation_RoundsIrishClockPhrases() =>
        Assert.Equal("fiche a cúig tar éis a haon", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
#endif

    [Fact]
    public void CompassHeadings_UseIrishFullAndShortForms()
    {
        Assert.Equal("tuaisceart", 0d.ToHeading(HeadingStyle.Full, Irish));
        Assert.Equal("oirthear", 90d.ToHeading(HeadingStyle.Full, Irish));
        Assert.Equal("deisceart", 180d.ToHeading(HeadingStyle.Full, Irish));
        Assert.Equal("iarthar", 270d.ToHeading(HeadingStyle.Full, Irish));
        Assert.Equal("T", 0d.ToHeading(HeadingStyle.Abbreviated, Irish));
        Assert.Equal("O", 90d.ToHeading(HeadingStyle.Abbreviated, Irish));
    }
}