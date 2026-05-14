namespace Humanizer.Tests.Localisation.sq;

[UseCulture("sq")]
public class AlbanianLocaleParityTests
{
    static readonly CultureInfo Sq = new("sq");

    [Theory]
    [InlineData(0, "zero")]
    [InlineData(1, "një")]
    [InlineData(11, "njëmbëdhjetë")]
    [InlineData(21, "njëzet e një")]
    [InlineData(99, "nëntëdhjetë e nëntë")]
    [InlineData(100, "njëqind")]
    [InlineData(101, "njëqind e një")]
    [InlineData(1234, "një mijë e dyqind e tridhjetë e katër")]
    [InlineData(1000000, "një milion")]
    [InlineData(-21, "minus njëzet e një")]
    public void NumberToWords_ProducesAlbanianCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Sq));
    }

    [Theory]
    [InlineData(1, "i parë")]
    [InlineData(2, "i dytë")]
    [InlineData(20, "i njëzetë")]
    [InlineData(21, "i njëzet e një")]
    public void NumberToOrdinalWords_ProducesAlbanianOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Sq));
    }

    [Theory]
    [InlineData("njëzet e një", 21)]
    [InlineData("njëqind e një", 101)]
    [InlineData("një mijë e dyqind e tridhjetë e katër", 1234)]
    [InlineData("minus njëzet e një", -21)]
    [InlineData("i parë", 1)]
    [InlineData("i njëzet e një", 21)]
    public void WordsToNumber_ParsesAlbanianCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Sq));
        Assert.True(words.TryToNumber(out var parsed, Sq, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, "1-rë")]
    [InlineData(2, "2-të")]
    [InlineData(21, "21-të")]
    public void Ordinalize_UsesAlbanianNumericSuffixes(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Sq));
    }

    [Fact]
    public void DateToOrdinalWords_UsesAlbanianDateOrder()
    {
        Assert.Equal("25 janar 2022", new DateTime(2022, 1, 25).ToOrdinalWords());
    }

    [Fact]
    public void Phrases_UseAlbanianRelativeDateAndDurationOutput()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Sq);

        Assert.Equal("tani", formatter.DateHumanize_Now());
        Assert.Equal("kurrë", formatter.DateHumanize_Never());
        Assert.Equal("2 ditë më parë", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("pas 3 muajsh", formatter.DateHumanize(TimeUnit.Month, Tense.Future, 3));
        Assert.Equal("5 minuta", formatter.TimeSpanHumanize(TimeUnit.Minute, 5));
        Assert.Equal("një orë", formatter.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true));
        Assert.Equal("bajt", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("min", formatter.TimeUnitHumanize(TimeUnit.Minute));
    }

    [Fact]
    public void CollectionFormatter_UsesAlbanianConjunction()
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(Sq);

        Assert.Equal("1, 2 dhe 3", formatter.Humanize([1, 2, 3]));
    }

    [Theory]
    [InlineData(0.0, "veri")]
    [InlineData(45.0, "verilindje")]
    [InlineData(90.0, "lindje")]
    [InlineData(180.0, "jug")]
    [InlineData(270.0, "perëndim")]
    public void Compass_UsesAlbanianDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Sq));
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(1, 0, "ora një herët në mëngjes")]
    [InlineData(13, 23, "ora një e njëzet e tre pasdite")]
    [InlineData(20, 0, "ora tetë në mbrëmje")]
    public void ToClockNotation_ProducesAlbanianClockPhrases(int hour, int minute, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_RoundsWithAlbanianMinuteWords()
    {
        Assert.Equal("ora një e njëzet e pesë pasdite", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif
}