namespace Humanizer.Tests.Localisation.pl;

[UseCulture("pl")]
public class PolishPaucalTests
{
    static readonly CultureInfo Polish = new("pl");
    static readonly IFormatter Formatter = Configurator.GetFormatter(new("pl"));

    [Theory]
    [InlineData(12, "12 minut")]
    [InlineData(13, "13 minut")]
    [InlineData(14, "14 minut")]
    [InlineData(21, "21 minut")]
    [InlineData(22, "22 minuty")]
    [InlineData(23, "23 minuty")]
    [InlineData(24, "24 minuty")]
    [InlineData(25, "25 minut")]
    public void TimeSpanHumanize_UsesPaucalFormForMatchingLastDigits(int count, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(count).Humanize(culture: Polish));

    [Theory]
    [InlineData(12, "za 12 minut")]
    [InlineData(22, "za 22 minuty")]
    public void DateHumanize_UsesPaucalFormForMatchingLastDigits(int count, string expected) =>
        Assert.Equal(expected, Formatter.DateHumanize(TimeUnit.Minute, Tense.Future, count));

    [Theory]
    [InlineData(12, "bajtów")]
    [InlineData(21, "bajtów")]
    [InlineData(22, "bajty")]
    public void DataUnitHumanize_UsesPaucalFormForMatchingLastDigits(int count, string expected) =>
        Assert.Equal(expected, Formatter.DataUnitHumanize(DataUnit.Byte, count, toSymbol: false));
}