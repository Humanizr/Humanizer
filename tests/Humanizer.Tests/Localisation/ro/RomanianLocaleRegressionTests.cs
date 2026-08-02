namespace Humanizer.Tests.Localisation.ro;

[UseCulture("ro-RO")]
public class RomanianLocaleRegressionTests
{
    static readonly CultureInfo Romanian = new("ro-RO");

    [Theory]
    [InlineData(TimeUnit.Millisecond, "o milisecundă")]
    [InlineData(TimeUnit.Second, "o secundă")]
    [InlineData(TimeUnit.Minute, "un minut")]
    [InlineData(TimeUnit.Hour, "o oră")]
    [InlineData(TimeUnit.Day, "o zi")]
    [InlineData(TimeUnit.Week, "o săptămână")]
    [InlineData(TimeUnit.Month, "o lună")]
    [InlineData(TimeUnit.Year, "un an")]
    public void ToWordsUsesSingularFormForEverySupportedUnit(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Romanian);

        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, 1, toWords: true));
    }

    [Theory]
    [InlineData(-1, "minus primul")]
    [InlineData(-2, "minus al doilea")]
    [InlineData(
        225_010_018,
        "al două sute douăzeci și cinci de milioane zece mii optsprezecelea")]
    [InlineData(
        int.MinValue,
        "minus al două miliarde o sută patruzeci și șapte de milioane patru sute optzeci și trei de mii șase sute patruzeci și optulea")]
    public void ToOrdinalWordsPreservesHighScaleGrammarAndNegativeMagnitude(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(Romanian));

#if NET6_0_OR_GREATER
    [Fact]
    public void ExactHourUsesMin0Template() =>
        Assert.Equal(
            "ora unu",
            new TimeOnly(13, 0).ToClockNotation(ClockNotationRounding.None, Romanian));

    [Theory]
    [InlineData(1, "ora cinci și unu")]
    [InlineData(2, "ora cinci și două")]
    [InlineData(21, "ora cinci și douăzeci și unu")]
    [InlineData(22, "ora cinci și douăzeci și două")]
    public void ClockNotationUsesNeuterMinuteValues(int minutes, string expected) =>
        Assert.Equal(
            expected,
            new TimeOnly(5, minutes).ToClockNotation(ClockNotationRounding.None, Romanian));
#endif
}