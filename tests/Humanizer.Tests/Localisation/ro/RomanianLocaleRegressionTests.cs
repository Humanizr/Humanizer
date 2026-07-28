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

#if NET6_0_OR_GREATER
    [Fact]
    public void ExactHourUsesMin0Template() =>
        Assert.Equal(
            "ora unu",
            new TimeOnly(13, 0).ToClockNotation(ClockNotationRounding.None, Romanian));
#endif
}