namespace Humanizer.Tests.Localisation.gl;

[UseCulture("gl")]
public class GalicianLocaleParityTests
{
    [Theory]
    [InlineData(0, "cero")]
    [InlineData(1, "un")]
    [InlineData(2, "dous")]
    [InlineData(21, "vinte e un")]
    [InlineData(99, "noventa e nove")]
    [InlineData(100, "cen")]
    [InlineData(101, "cento un")]
    [InlineData(2024, "dous mil vinte e catro")]
    public void NumberToWords_UsesGalicianCardinals(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(new CultureInfo("gl")));

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "un")]
    [InlineData(1, GrammaticalGender.Feminine, "unha")]
    [InlineData(2, GrammaticalGender.Feminine, "dúas")]
    [InlineData(221, GrammaticalGender.Feminine, "dúascentas vinte e unha")]
    public void NumberToWords_UsesGalicianGenderedCardinals(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToWords(gender, new CultureInfo("gl")));

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "primeiro")]
    [InlineData(1, GrammaticalGender.Feminine, "primeira")]
    [InlineData(21, GrammaticalGender.Masculine, "vixésimo primeiro")]
    [InlineData(21, GrammaticalGender.Feminine, "vixésima primeira")]
    public void NumberToWords_UsesGalicianOrdinals(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, new CultureInfo("gl")));

    [Theory]
    [InlineData("vinte e un", 21)]
    [InlineData("menos cento vinte e tres", -123)]
    [InlineData("primeiro", 1)]
    [InlineData("vixésimo primeiro", 21)]
    [InlineData("un trillón", 1_000_000_000_000_000_000L)]
    public void WordsToNumber_ParsesGalicianCardinalsAndOrdinals(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(new CultureInfo("gl")));

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1.º")]
    [InlineData(2, "2.º")]
    [InlineData(23, "23.º")]
    public void Ordinalize_UsesGalicianMasculineNumericSuffix(int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("gl")));

    [Fact]
    public void Formatter_UsesGalicianPhrases()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("gl"));

        Assert.Equal("agora", formatter.DateHumanize_Now());
        Assert.Equal("nunca", formatter.DateHumanize_Never());
        Assert.Equal("onte", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal("dentro de 2 días", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 días", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("unha hora", formatter.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true));
        Assert.Equal("byte", formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));
        Assert.Equal("d", formatter.TimeUnitHumanize(TimeUnit.Day));
    }

    [Fact]
    public void CollectionFormatter_UsesGalicianConjunction()
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new CultureInfo("gl"));

        Assert.Equal("1 e 2", formatter.Humanize([1, 2]));
        Assert.Equal("1, 2 e 3", formatter.Humanize([1, 2, 3]));
    }

    [Theory]
    [InlineData(2015, 1, 1, "1 de xaneiro de 2015")]
    [InlineData(2022, 1, 25, "25 de xaneiro de 2022")]
    [InlineData(2024, 12, 31, "31 de decembro de 2024")]
    public void DateToOrdinalWords_UsesGalicianDatePattern(int year, int month, int day, string expected) =>
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(1, 5, ClockNotationRounding.None, "a unha e cinco da madrugada")]
    [InlineData(13, 23, ClockNotationRounding.None, "a unha e vinte e tres da tarde")]
    [InlineData(13, 23, ClockNotationRounding.NearestFiveMinutes, "a unha e vinte e cinco da tarde")]
    [InlineData(20, 59, ClockNotationRounding.NearestFiveMinutes, "as nove da noite")]
    public void TimeOnlyToClockNotation_UsesGalicianPhrases(int hour, int minute, ClockNotationRounding rounding, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation(rounding));
#endif

    [Theory]
    [InlineData(0, "norte")]
    [InlineData(45, "nordés")]
    [InlineData(90, "leste")]
    [InlineData(225, "sudoeste")]
    public void Compass_UsesGalicianHeadings(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}