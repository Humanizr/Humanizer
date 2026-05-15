namespace Humanizer.Tests.Localisation.et;

[UseCulture("et")]
public class EstonianLocaleParityTests
{
    static readonly CultureInfo Estonian = CultureInfo.GetCultureInfo("et");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Theory]
    [InlineData(0, "null")]
    [InlineData(1, "üks")]
    [InlineData(11, "üksteist")]
    [InlineData(21, "kakskümmend üks")]
    [InlineData(100, "sada")]
    [InlineData(101, "sada üks")]
    [InlineData(200, "kakssada")]
    [InlineData(2000, "kaks tuhat")]
    [InlineData(2000000, "kaks miljonit")]
    [InlineData(1234567, "miljon kakssada kolmkümmend neli tuhat viissada kuuskümmend seitse")]
    public void ToWords_UsesEstonianCardinalForms(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(Estonian));

    [Theory]
    [InlineData(1, "esimene")]
    [InlineData(2, "teine")]
    [InlineData(3, "kolmas")]
    [InlineData(10, "kümnes")]
    [InlineData(20, "kahekümnes")]
    [InlineData(21, "kahekümne esimene")]
    [InlineData(100, "sajas")]
    [InlineData(101, "saja esimene")]
    [InlineData(200, "kahesajas")]
    [InlineData(2021, "kahe tuhande kahekümne esimene")]
    [InlineData(10002, "kümne tuhande teine")]
    public void ToOrdinalWords_UsesEstonianOrdinalForms(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(Estonian));

    [Theory]
    [InlineData("kakskümmend üks", 21)]
    [InlineData("sada viis", 105)]
    [InlineData("kahekümne esimene", 21)]
    [InlineData("saja esimene", 101)]
    [InlineData("kahe tuhande kahekümne esimene", 2021)]
    [InlineData("kahetuhandes", 2000)]
    [InlineData("kahekümnetuhandes", 20000)]
    [InlineData("kahekümneühetuhandes", 21000)]
    [InlineData("21.", 21)]
    [InlineData("miinus kakskümmend üks", -21)]
    public void ToNumber_ParsesEstonianCardinalOrdinalAndNumericOrdinalForms(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(Estonian));


    [Fact]
    public void ToNumber_DoesNotTreatMinusWordAsZeroToken() =>
        Assert.Throws<ArgumentException>(() => "miinus".ToNumber(Estonian));

    [Fact]
    public void ToOrdinalWords_DoesNotUseMalformedDefaultSuffixFallback()
    {
        var ordinal = 10002.ToOrdinalWords(Estonian);

        Assert.Equal("kümne tuhande teine", ordinal);
        Assert.DoesNotContain("kakss", ordinal);
    }

    [Theory]
    [InlineData(10003)]
    [InlineData(21001)]
    public void ToOrdinalWords_RejectsUnmodeledEstonianOrdinalsInsteadOfReturningCardinals(int number) =>
        Assert.Throws<NotImplementedException>(() => number.ToOrdinalWords(Estonian));

    [Theory]
    [InlineData(0, "0.")]
    [InlineData(1, "1.")]
    [InlineData(21, "21.")]
    public void Ordinalize_UsesEstonianDotSuffix(int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(Estonian));

    [Fact]
    public void Ordinalize_UsesEstonianNegativeDotSuffix() =>
        Assert.Equal(ExpectedNegativeOrdinal, (-21).Ordinalize(Estonian));

#if NET48
    const string ExpectedNegativeOrdinal = "-21.";
#else
    const string ExpectedNegativeOrdinal = "−21.";
#endif

    [Fact]
    public void CollectionHumanize_UsesEstonianConjunction()
    {
        Assert.Equal("1 ja 2", Pair.Humanize());
        Assert.Equal("1, 2 ja 3", Triple.Humanize());
    }

    [Fact]
    public void DateAndDurationHumanize_UseEstonianPhrases()
    {
        var comparisonBase = new DateTime(2026, 5, 14, 12, 0, 0, DateTimeKind.Local);

        Assert.Equal("eile", comparisonBase.AddDays(-1).Humanize(false, comparisonBase, Estonian));
        Assert.Equal("2 päeva tagasi", comparisonBase.AddDays(-2).Humanize(false, comparisonBase, Estonian));
        Assert.Equal("2 päeva pärast", comparisonBase.AddDays(2).Humanize(false, comparisonBase, Estonian));
        Assert.Equal("2 päeva", TimeSpan.FromDays(2).Humanize(culture: Estonian));
        Assert.Equal("tund", TimeSpan.FromHours(1).Humanize(toWords: true, culture: Estonian));
        Assert.Equal("mitte kunagi", ((DateTime?)null).Humanize(culture: Estonian));
    }

    [Fact]
    public void ByteSizeAndTimeUnitSymbols_UseEstonianUnits()
    {
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Estonian));
        Assert.Equal("2 kilobaiti", ByteSize.FromKilobytes(2).ToFullWords(provider: Estonian));
        Assert.Equal("min", TimeUnit.Minute.ToSymbol(Estonian));
        Assert.Equal("h", TimeUnit.Hour.ToSymbol(Estonian));
    }

    [Fact]
    public void DateToOrdinalWords_UsesEstonianDatePatternAndMonthNames() =>
        Assert.Equal("25. jaanuar 2022", new DateTime(2022, 1, 25).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWords_UsesEstonianDatePatternAndMonthNames() =>
        Assert.Equal("31. detsember 2024", new DateOnly(2024, 12, 31).ToOrdinalWords());

    [Theory]
    [InlineData(13, 23, "13:23")]
    [InlineData(4, 0, "4:00")]
    [InlineData(1, 5, "1:05")]
    [InlineData(1, 6, "1:06")]
    [InlineData(1, 7, "1:07")]
    public void ToClockNotation_UsesEstonianNumericClockPhrases(int hour, int minute, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());

    [Fact]
    public void ToClockNotation_RoundsEstonianClockPhrases() =>
        Assert.Equal("13:25", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
#endif

    [Fact]
    public void CompassHeadings_UseEstonianFullAndShortForms()
    {
        Assert.Equal("põhi", 0d.ToHeading(HeadingStyle.Full, Estonian));
        Assert.Equal("ida", 90d.ToHeading(HeadingStyle.Full, Estonian));
        Assert.Equal("lõuna", 180d.ToHeading(HeadingStyle.Full, Estonian));
        Assert.Equal("lääs", 270d.ToHeading(HeadingStyle.Full, Estonian));
        Assert.Equal("N", 0d.ToHeading(HeadingStyle.Abbreviated, Estonian));
        Assert.Equal("E", 90d.ToHeading(HeadingStyle.Abbreviated, Estonian));
    }
}