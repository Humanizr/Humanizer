namespace Humanizer.Tests.Localisation.kk;

[UseCulture("kk")]
public class KazakhLocaleParityTests
{
    static readonly CultureInfo Kk = new("kk");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesKazakhConjunction()
    {
        Assert.Equal("1 және 2", Pair.Humanize());
        Assert.Equal("1, 2 және 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(0, TimeUnit.Second, Tense.Future, "қазір")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "кеше")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ертең")]
    [InlineData(1, TimeUnit.Second, Tense.Past, "бір секунд бұрын")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 күн бұрын")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 күннен кейін")]
    [InlineData(1, TimeUnit.Minute, Tense.Future, "бір минуттан кейін")]
    [InlineData(1, TimeUnit.Year, Tense.Future, "бір жылдан кейін")]
    public void DateHumanize_UsesKazakhRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kk);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesKazakhNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ешқашан", date.Humanize(culture: Kk));
    }

    [Theory]
    [InlineData(TimeUnit.Hour, 1, true, "бір сағат")]
    [InlineData(TimeUnit.Day, 2, false, "2 күн")]
    [InlineData(TimeUnit.Week, 2, false, "2 апта")]
    public void TimeSpanHumanize_UsesKazakhDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kk);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void TimeSpanHumanize_ComposesKazakhDurationsWithKazakhListJoiner()
    {
        Assert.Equal("2 апта, 1 күн және 1 сағат", TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: Kk, collectionSeparator: null));
        Assert.Equal("1 күн, 3 минут және 4 секунд", new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: false, culture: Kk, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null));
    }

    [Fact]
    public void ToAge_UsesKazakhAgeTemplate()
    {
        Assert.Equal("жасы 1 жыл", TimeSpan.FromDays(366).ToAge(Kk));
    }

    [Theory]
    [InlineData(DataUnit.Bit, "бит")]
    [InlineData(DataUnit.Byte, "байт")]
    [InlineData(DataUnit.Kilobyte, "килобайт")]
    [InlineData(DataUnit.Megabyte, "мегабайт")]
    public void DataUnitHumanize_UsesKazakhNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kk);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void ByteSizeToFullWords_UsesKazakhDataUnits()
    {
        Assert.Equal("1 бит", ByteSize.FromBits(1).ToFullWords(provider: Kk));
        Assert.Equal("1 байт", ByteSize.FromBytes(1).ToFullWords(provider: Kk));
        Assert.Equal("2 килобайт", ByteSize.FromKilobytes(2).ToFullWords(provider: Kk));
    }

    [Fact]
    public void ByteSizeHumanize_UsesKazakhNumberFormatting()
    {
        Assert.Equal("1,95 KB", ByteSize.FromBytes(2000).Humanize("KB", Kk));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "мс")]
    [InlineData(TimeUnit.Second, "секунд")]
    [InlineData(TimeUnit.Minute, "минут")]
    [InlineData(TimeUnit.Hour, "сағат")]
    [InlineData(TimeUnit.Day, "күн")]
    [InlineData(TimeUnit.Week, "апта")]
    [InlineData(TimeUnit.Month, "ай")]
    [InlineData(TimeUnit.Year, "жыл")]
    public void TimeUnitHumanize_UsesKazakhLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kk);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "нөл")]
    [InlineData(1, "бір")]
    [InlineData(21, "жиырма бір")]
    [InlineData(105, "жүз бес")]
    [InlineData(1234, "бір мың екі жүз отыз төрт")]
    [InlineData(1001001, "бір миллион бір мың бір")]
    [InlineData(-5, "минус бес")]
    public void NumberToWords_ProducesKazakhCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Kk));
    }

    [Theory]
    [InlineData(1, "бірінші")]
    [InlineData(2, "екінші")]
    [InlineData(6, "алтыншы")]
    [InlineData(20, "жиырмасыншы")]
    [InlineData(21, "жиырма бірінші")]
    public void NumberToOrdinalWords_ProducesKazakhOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Kk));
    }

    [Theory]
    [InlineData("жиырма бір", 21)]
    [InlineData("жиырма бірінші", 21)]
    [InlineData("минус бес", -5)]
    [InlineData("бір миллион бір мың бір", 1001001)]
    public void WordsToNumber_ParsesKazakhCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Kk));
        Assert.True(words.TryToNumber(out var parsed, Kk, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, "1-ші")]
    [InlineData(6, "6-шы")]
    [InlineData(20, "20-шы")]
    [InlineData(-2, "-2-ші")]
    public void Ordinalize_UsesKazakhNumericSuffixes(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Kk));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(Kk));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 қаңтар 2022")]
    [InlineData(2015, 2, 3, "3 ақпан 2015")]
    public void DateTimeToOrdinalWords_UsesKazakhDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 қаңтар 2022")]
    [InlineData(2015, 2, 3, "3 ақпан 2015")]
    public void DateOnlyToOrdinalWords_UsesKazakhDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "бір сағат")]
    [InlineData(1, 5, "бір сағат бес минут")]
    [InlineData(13, 23, "он үш сағат жиырма үш минут")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("он үш сағат жиырма бес минут", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "солтүстік")]
    [InlineData(45.0, "солтүстік-шығыс")]
    [InlineData(90.0, "шығыс")]
    [InlineData(135.0, "оңтүстік-шығыс")]
    [InlineData(180.0, "оңтүстік")]
    [InlineData(225.0, "оңтүстік-батыс")]
    [InlineData(270.0, "батыс")]
    [InlineData(315.0, "солтүстік-батыс")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "С")]
    [InlineData(45.0, "СШ")]
    [InlineData(90.0, "Ш")]
    [InlineData(180.0, "О")]
    [InlineData(270.0, "Б")]
    public void Compass_AbbreviatedDirectionsUseKazakhLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}