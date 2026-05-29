namespace Humanizer.Tests.Localisation.eu;

[UseCulture("eu")]
public class BasqueLocaleParityTests
{
    [Theory]
    [InlineData(0, "zero")]
    [InlineData(1, "bat")]
    [InlineData(21, "hogeita bat")]
    [InlineData(99, "laurogeita hemeretzi")]
    [InlineData(100, "ehun")]
    [InlineData(101, "ehun eta bat")]
    [InlineData(2024, "bi mila eta hogeita lau")]
    [InlineData(1000001, "milioi bat eta bat")]
    [InlineData(1000000001, "mila milioi eta bat")]
    public void NumberToWords_UsesBasqueCardinals(long number, string expected) =>
        Assert.Equal(expected, number.ToWords(new CultureInfo("eu")));

    [Theory]
    [InlineData(1, "lehen")]
    [InlineData(2, "bigarren")]
    [InlineData(21, "hogeita batgarren")]
    [InlineData(1000000, "milioigarren")]
    [InlineData(1000000000, "mila milioigarren")]
    public void NumberToWords_UsesBasqueOrdinals(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(new CultureInfo("eu")));

    [Theory]
    [InlineData("hogeita bat", 21)]
    [InlineData("minus ehun eta hogeita hiru", -123)]
    [InlineData("milioi bat", 1000000)]
    [InlineData("milioi bat eta bat", 1000001)]
    [InlineData("mila milioi eta bat", 1000000001)]
    [InlineData("lehen", 1)]
    [InlineData("hogeita batgarren", 21)]
    [InlineData("milioigarren", 1000000)]
    [InlineData("mila milioigarren", 1000000000)]
    [InlineData("bi mila milioigarren", 2000000000)]
    [InlineData("1.", 1)]
    [InlineData("23.", 23)]
    public void WordsToNumber_ParsesBasqueCardinalsAndOrdinals(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(new CultureInfo("eu")));

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1.")]
    [InlineData(23, "23.")]
    public void Ordinalize_UsesBasqueNumericSuffix(int number, string expected) =>
        Assert.Equal(expected, number.Ordinalize(new CultureInfo("eu")));

    [Fact]
    public void Formatter_UsesBasquePhrases()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("eu"));

        Assert.Equal("orain", formatter.DateHumanize_Now());
        Assert.Equal("inoiz ez", formatter.DateHumanize_Never());
        Assert.Equal("atzo", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal("etzi", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 egun", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("ordu bat", formatter.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true));
        Assert.Equal("byte", formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));
        Assert.Equal("eg", formatter.TimeUnitHumanize(TimeUnit.Day));
    }

    [Fact]
    public void CollectionFormatter_UsesBasqueConjunction()
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new CultureInfo("eu"));

        Assert.Equal("1 eta 2", formatter.Humanize([1, 2]));
        Assert.Equal("1, 2 eta 3", formatter.Humanize([1, 2, 3]));
    }

    [Theory]
    [InlineData(2015, 1, 1, "2015eko urtarrilaren 1")]
    [InlineData(2022, 1, 25, "2022eko urtarrilaren 25")]
    [InlineData(2024, 12, 31, "2024eko abenduaren 31")]
    public void DateToOrdinalWords_UsesBasqueDatePattern(int year, int month, int day, string expected) =>
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(1, 5, ClockNotationRounding.None, "ordu bata eta bost gauean")]
    [InlineData(13, 23, ClockNotationRounding.None, "ordu bata eta hogeita hiru arratsaldean")]
    [InlineData(13, 23, ClockNotationRounding.NearestFiveMinutes, "ordu bata eta hogeita bost arratsaldean")]
    public void TimeOnlyToClockNotation_UsesBasquePhrases(int hour, int minute, ClockNotationRounding rounding, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation(rounding));
#endif

    [Theory]
    [InlineData(0, "ipar")]
    [InlineData(45, "ipar-ekialde")]
    [InlineData(90, "ekialde")]
    [InlineData(225, "hego-mendebalde")]
    public void Compass_UsesBasqueHeadings(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}