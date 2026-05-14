namespace Humanizer.Tests.Localisation.mk;

[UseCulture("mk")]
public class MacedonianLocaleParityTests
{
    static readonly CultureInfo Macedonian = CultureInfo.GetCultureInfo("mk");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Theory]
    [InlineData(0, "нула")]
    [InlineData(1, "еден")]
    [InlineData(2, "два")]
    [InlineData(21, "дваесет и еден")]
    [InlineData(101, "сто и еден")]
    [InlineData(121, "сто дваесет и еден")]
    [InlineData(1001, "илјада и еден")]
    [InlineData(1001001, "еден милион илјада и еден")]
    public void ToWords_UsesMacedonianCardinalForms(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(Macedonian));

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "еден")]
    [InlineData(1, GrammaticalGender.Feminine, "една")]
    [InlineData(1, GrammaticalGender.Neuter, "едно")]
    [InlineData(2, GrammaticalGender.Masculine, "два")]
    [InlineData(2, GrammaticalGender.Feminine, "две")]
    [InlineData(2, GrammaticalGender.Neuter, "две")]
    public void ToWords_UsesMacedonianGenderedUnitForms(long number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToWords(gender, Macedonian));

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "прв")]
    [InlineData(1, GrammaticalGender.Feminine, "прва")]
    [InlineData(1, GrammaticalGender.Neuter, "прво")]
    [InlineData(21, GrammaticalGender.Masculine, "дваесет и прв")]
    [InlineData(21, GrammaticalGender.Feminine, "дваесет и прва")]
    [InlineData(21, GrammaticalGender.Neuter, "дваесет и прво")]
    [InlineData(100, GrammaticalGender.Masculine, "стоти")]
    [InlineData(1000, GrammaticalGender.Feminine, "илјадита")]
    public void ToOrdinalWords_UsesMacedonianGenderedOrdinalForms(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, Macedonian));

    [Theory]
    [InlineData("дваесет и еден", 21)]
    [InlineData("сто и еден", 101)]
    [InlineData("илјада и дванаесет", 1012)]
    [InlineData("два милиони", 2000000)]
    [InlineData("единаесетта", 11)]
    [InlineData("единаесетто", 11)]
    [InlineData("дваесет и прв", 21)]
    [InlineData("дваесет и прва", 21)]
    [InlineData("дваесет и прво", 21)]
    [InlineData("стота", 100)]
    [InlineData("илјадито", 1000)]
    public void ToNumber_ParsesMacedonianCardinalAndOrdinalForms(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(Macedonian));


    [Fact]
    public void ToOrdinalWords_HandlesMinimumInteger() =>
        Assert.Equal(
            "минус две милијарди сто четириесет и седум милиони четиристотини осумдесет и три илјади шестотини четириесет и осми",
            int.MinValue.ToOrdinalWords(Macedonian));

    [Theory]
    [InlineData(1, "1.")]
    [InlineData(21, "21.")]
    [InlineData(-1, "-1.")]
    public void Ordinalize_UsesMacedonianNumericOrdinalSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Macedonian));
        Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine, Macedonian));
    }

    [Fact]
    public void CollectionHumanize_UsesMacedonianConjunction()
    {
        Assert.Equal("1 и 2", Pair.Humanize());
        Assert.Equal("1, 2 и 3", Triple.Humanize());
    }

    [Fact]
    public void DateAndDurationHumanize_UseMacedonianPhrases()
    {
        var comparisonBase = new DateTime(2026, 5, 14, 12, 0, 0, DateTimeKind.Local);

        Assert.Equal("вчера", comparisonBase.AddDays(-1).Humanize(false, comparisonBase, Macedonian));
        Assert.Equal("пред 2 дена", comparisonBase.AddDays(-2).Humanize(false, comparisonBase, Macedonian));
        Assert.Equal("за 2 дена", comparisonBase.AddDays(2).Humanize(false, comparisonBase, Macedonian));
        Assert.Equal("сега", DateTime.UtcNow.Humanize(culture: Macedonian));
        Assert.Equal("никогаш", ((DateTime?)null).Humanize(culture: Macedonian));
        Assert.Equal("2 дена", TimeSpan.FromDays(2).Humanize(culture: Macedonian));
        Assert.Equal("еден ден", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Macedonian));
    }

    [Fact]
    public void ByteSizeAndTimeUnitSymbols_UseMacedonianUnits()
    {
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Macedonian));
        Assert.Equal("2 килобајти", ByteSize.FromKilobytes(2).ToFullWords(provider: Macedonian));
        Assert.Equal("мин.", TimeUnit.Minute.ToSymbol(Macedonian));
        Assert.Equal("д.", TimeUnit.Day.ToSymbol(Macedonian));
    }

    [Fact]
    public void DateToOrdinalWords_UsesMacedonianDatePatternAndMonthNames() =>
        Assert.Equal("25 јануари 2022 г.", new DateTime(2022, 1, 25).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWords_UsesMacedonianDatePatternAndMonthNames() =>
        Assert.Equal("3 февруари 2015 г.", new DateOnly(2015, 2, 3).ToOrdinalWords());

    [Theory]
    [InlineData(13, 23, "тринаесет часот и дваесет и три минути")]
    [InlineData(1, 5, "еден часот и пет минути")]
    public void ToClockNotation_UsesMacedonianClockPhrases(int hour, int minute, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());

    [Fact]
    public void ToClockNotation_RoundsMacedonianClockPhrases() =>
        Assert.Equal("тринаесет часот и дваесет и пет минути", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
#endif

    [Fact]
    public void CompassHeadings_UseMacedonianFullAndShortForms()
    {
        Assert.Equal("север", 0d.ToHeading(HeadingStyle.Full, Macedonian));
        Assert.Equal("исток", 90d.ToHeading(HeadingStyle.Full, Macedonian));
        Assert.Equal("југ", 180d.ToHeading(HeadingStyle.Full, Macedonian));
        Assert.Equal("запад", 270d.ToHeading(HeadingStyle.Full, Macedonian));
        Assert.Equal("С", 0d.ToHeading(HeadingStyle.Abbreviated, Macedonian));
        Assert.Equal("И", 90d.ToHeading(HeadingStyle.Abbreviated, Macedonian));
    }
}