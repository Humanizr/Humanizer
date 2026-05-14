namespace Humanizer.Tests.Localisation.be;

[UseCulture("be")]
public class BelarusianLocaleParityTests
{
    static readonly CultureInfo Be = new("be");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];
    static readonly int[] Quadruple = [1, 2, 3, 4];

    [Fact]
    public void CollectionFormatter_UsesBelarusianConjunction()
    {
        Assert.Equal("1 і 2", Pair.Humanize());
        Assert.Equal("1, 2 і 3", Triple.Humanize());
        Assert.Equal("1, 2, 3 і 4", Quadruple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "учора")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "заўтра")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 дні таму")]
    [InlineData(5, TimeUnit.Day, Tense.Future, "праз 5 дзён")]
    [InlineData(21, TimeUnit.Day, Tense.Past, "21 дзень таму")]
    [InlineData(1, TimeUnit.Month, Tense.Future, "праз месяц")]
    [InlineData(2, TimeUnit.Year, Tense.Past, "2 гады таму")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "зараз")]
    public void DateHumanize_UsesBelarusianRelativePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Be);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesBelarusianNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ніколі", date.Humanize(culture: Be));
    }

    [Theory]
    [InlineData(0, TimeUnit.Millisecond, true, "няма часу")]
    [InlineData(1, TimeUnit.Second, false, "1 секунда")]
    [InlineData(1, TimeUnit.Second, true, "адна секунда")]
    [InlineData(2, TimeUnit.Minute, false, "2 хвіліны")]
    [InlineData(5, TimeUnit.Hour, false, "5 гадзін")]
    [InlineData(21, TimeUnit.Day, false, "21 дзень")]
    [InlineData(2, TimeUnit.Week, true, "два тыдні")]
    public void TimeSpanHumanize_UsesBelarusianDurationForms(int value, TimeUnit unit, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Be);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, value, toWords: toWords));
    }

    [Fact]
    public void Formatter_UsesBelarusianDataUnitsAndTimeUnitSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Be);

        Assert.Equal("біт", formatter.DataUnitHumanize(DataUnit.Bit, 1, toSymbol: false));
        Assert.Equal("байты", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("кілабайтаў", formatter.DataUnitHumanize(DataUnit.Kilobyte, 5, toSymbol: false));
        Assert.Equal("хв", TimeUnit.Minute.ToSymbol(Be));
        Assert.Equal("1,95 КБ", ByteSize.FromBytes(2000).Humanize("KB", Be));
        Assert.Equal("2 кілабайты", ByteSize.FromKilobytes(2).ToFullWords(provider: Be));
    }

    [Theory]
    [InlineData(0, "нуль")]
    [InlineData(21, "дваццаць адзін")]
    [InlineData(122, "сто дваццаць два")]
    [InlineData(1001, "адна тысяча адзін")]
    [InlineData(2000, "дзве тысячы")]
    [InlineData(2000000, "два мільёны")]
    [InlineData(-123, "мінус сто дваццаць тры")]
    public void NumberToWords_UsesBelarusianCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Be));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "адзін")]
    [InlineData(1, GrammaticalGender.Feminine, "адна")]
    [InlineData(1, GrammaticalGender.Neuter, "адно")]
    [InlineData(22, GrammaticalGender.Feminine, "дваццаць дзве")]
    [InlineData(101, GrammaticalGender.Neuter, "сто адно")]
    public void NumberToWords_UsesBelarusianCardinalGenders(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Be));
    }

    [Theory]
    [InlineData(0, "нулявы")]
    [InlineData(21, "дваццаць першы")]
    [InlineData(99, "дзевяноста дзявяты")]
    [InlineData(100, "соты")]
    [InlineData(1001, "адна тысяча першы")]
    public void NumberToOrdinalWords_UsesBelarusianOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Be));
    }

    [Theory]
    [InlineData(2, GrammaticalGender.Masculine, "другі")]
    [InlineData(2, GrammaticalGender.Feminine, "другая")]
    [InlineData(2, GrammaticalGender.Neuter, "другое")]
    [InlineData(21, GrammaticalGender.Feminine, "дваццаць першая")]
    [InlineData(100, GrammaticalGender.Neuter, "сотае")]
    public void NumberToOrdinalWords_UsesBelarusianOrdinalGenders(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Be));
    }

    [Theory]
    [InlineData("дваццаць адзін", 21)]
    [InlineData("мінус сто дваццаць тры", -123)]
    [InlineData("дзве тысячы", 2000)]
    [InlineData("дваццаць першая", 21)]
    [InlineData("тысячнае", 1000)]
    public void WordsToNumber_ParsesBelarusianCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Be));
        Assert.True(words.TryToNumber(out var parsed, Be, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "1-ы")]
    [InlineData(2, GrammaticalGender.Masculine, "2-і")]
    [InlineData(3, GrammaticalGender.Masculine, "3-і")]
    [InlineData(21, GrammaticalGender.Feminine, "21-я")]
    [InlineData(102, GrammaticalGender.Neuter, "102-е")]
    [InlineData(-1, GrammaticalGender.Masculine, "-1-ы")]
    public void Ordinalize_UsesBelarusianNumericSuffixes(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Be));
        Assert.Equal(expected, number.ToString(Be).Ordinalize(gender, Be));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 студзеня 2022")]
    [InlineData(2015, 2, 3, "3 лютага 2015")]
    [InlineData(2021, 10, 31, "31 кастрычніка 2021")]
    [InlineData(2024, 12, 31, "31 снежня 2024")]
    public void DateToOrdinalWords_UsesBelarusianGenitiveMonths(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 студзеня 2022")]
    [InlineData(2024, 12, 31, "31 снежня 2024")]
    public void DateOnlyToOrdinalWords_UsesBelarusianGenitiveMonths(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(13, 23, ClockNotationRounding.None, "першая дваццаць тры")]
    [InlineData(13, 23, ClockNotationRounding.NearestFiveMinutes, "першая дваццаць пяць")]
    [InlineData(1, 5, ClockNotationRounding.None, "першая пяць")]
    public void TimeOnlyToClockNotation_UsesBelarusianClockPhrases(int hour, int minute, ClockNotationRounding rounding, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation(rounding));
    }
#endif

    [Fact]
    public void Compass_UsesBelarusianHeadings()
    {
        Assert.Equal("поўнач", 0d.ToHeading(HeadingStyle.Full, Be));
        Assert.Equal("Пн", 0d.ToHeading(HeadingStyle.Abbreviated, Be));
        Assert.Equal("паўднёвы ўсход", 135d.ToHeading(HeadingStyle.Full, Be));
        Assert.Equal("ПдУ", 135d.ToHeading(HeadingStyle.Abbreviated, Be));
    }
}