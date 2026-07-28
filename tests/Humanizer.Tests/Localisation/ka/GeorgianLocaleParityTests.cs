namespace Humanizer.Tests.Localisation.ka;

[UseCulture("ka")]
public class GeorgianLocaleParityTests
{
    static readonly CultureInfo Georgian = CultureInfo.GetCultureInfo("ka");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Theory]
    [InlineData(0, "ნული")]
    [InlineData(21, "ოცდაერთი")]
    [InlineData(100, "ასი")]
    [InlineData(101, "ას ერთი")]
    [InlineData(1000, "ათასი")]
    [InlineData(1001, "ათას ერთი")]
    [InlineData(2021, "ორი ათას ოცდაერთი")]
    [InlineData(1234567, "ერთი მილიონ ორას ოცდათოთხმეტი ათას ხუთას სამოცდაშვიდი")]
    public void ToWords_UsesGeorgianJoinedScaleForms(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(Georgian));

    [Theory]
    [InlineData(1, "პირველი")]
    [InlineData(21, "ოცდამეერთე")]
    [InlineData(100, "მეასე")]
    [InlineData(101, "ას პირველი")]
    public void ToOrdinalWords_UsesGeorgianOrdinalForms(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(Georgian));

    [Theory]
    [InlineData("ოცდაერთი", 21)]
    [InlineData("ას ხუთი", 105)]
    [InlineData("ოცდამეერთე", 21)]
    [InlineData("1-ლი", 1)]
    [InlineData("101-ე", 101)]
    [InlineData("ორი ათას ოცდაერთი", 2021)]
    public void ToNumber_ParsesGeorgianCardinalOrdinalAndNumericOrdinalForms(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(Georgian));

    [Theory]
    [InlineData(0, "0-ე")]
    [InlineData(1, "1-ლი")]
    [InlineData(2, "2-ე")]
    [InlineData(101, "101-ე")]
    public void Ordinalize_UsesGeorgianNumericOrdinalSuffixes(int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(Georgian));

    [Fact]
    public void CollectionHumanize_UsesGeorgianConjunction()
    {
        Assert.Equal("1 და 2", Pair.Humanize());
        Assert.Equal("1, 2 და 3", Triple.Humanize());
    }

    [Fact]
    public void DateAndDurationHumanize_UseGeorgianPhrases()
    {
        var comparisonBase = new DateTime(2026, 5, 14, 12, 0, 0, DateTimeKind.Local);

        Assert.Equal("გუშინ", comparisonBase.AddDays(-1).Humanize(false, comparisonBase, Georgian));
        Assert.Equal("2 დღის წინ", comparisonBase.AddDays(-2).Humanize(false, comparisonBase, Georgian));
        Assert.Equal("2 დღეში", comparisonBase.AddDays(2).Humanize(false, comparisonBase, Georgian));
        Assert.Equal("2 დღე", TimeSpan.FromDays(2).Humanize(culture: Georgian));
        Assert.Equal("ერთი საათი", TimeSpan.FromHours(1).Humanize(toWords: true, culture: Georgian));
        Assert.Equal("არასოდეს", ((DateTime?)null).Humanize(culture: Georgian));
    }

    [Fact]
    public void ByteSizeAndTimeUnitSymbols_UseGeorgianUnits()
    {
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Georgian));
        Assert.Equal("2 კილობაიტი", ByteSize.FromKilobytes(2).ToFullWords(provider: Georgian));
        Assert.Equal("წთ", TimeUnit.Minute.ToSymbol(Georgian));
        Assert.Equal("სთ", TimeUnit.Hour.ToSymbol(Georgian));
    }

    [Fact]
    public void DateToOrdinalWords_UsesGeorgianDatePatternAndMonthNames() =>
        Assert.Equal("25 იანვარი 2022", new DateTime(2022, 1, 25).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWords_UsesGeorgianDatePatternAndMonthNames() =>
        Assert.Equal("31 დეკემბერი 2024", new DateOnly(2024, 12, 31).ToOrdinalWords());

    [Theory]
    [InlineData(13, 23, "13 საათი 23 წუთი")]
    [InlineData(4, 0, "4 საათი")]
    public void ToClockNotation_UsesGeorgianClockPhrases(int hour, int minute, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());

    [Fact]
    public void ToClockNotation_RoundsGeorgianClockPhrases() =>
        Assert.Equal("13 საათი 25 წუთი", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
#endif

    [Fact]
    public void CompassHeadings_UseGeorgianFullAndShortForms()
    {
        Assert.Equal("ჩრდილოეთი", 0d.ToHeading(HeadingStyle.Full, Georgian));
        Assert.Equal("აღმოსავლეთი", 90d.ToHeading(HeadingStyle.Full, Georgian));
        Assert.Equal("სამხრეთი", 180d.ToHeading(HeadingStyle.Full, Georgian));
        Assert.Equal("დასავლეთი", 270d.ToHeading(HeadingStyle.Full, Georgian));
        Assert.Equal("ჩ", 0d.ToHeading(HeadingStyle.Abbreviated, Georgian));
        Assert.Equal("ა", 90d.ToHeading(HeadingStyle.Abbreviated, Georgian));
    }
}