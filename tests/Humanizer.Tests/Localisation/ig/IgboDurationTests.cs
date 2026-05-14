namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboDurationTests
{
    [Theory]
    [InlineData(TimeUnit.Day, 1, "ụbọchị 1")]
    [InlineData(TimeUnit.Day, 2, "ụbọchị 2")]
    [InlineData(TimeUnit.Month, 1, "ọnwa 1")]
    [InlineData(TimeUnit.Month, 2, "ọnwa 2")]
    public void TimeSpanHumanize_UsesIgboDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesIgboSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal("otu ụbọchị", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    // Age statements use the Igbo predicate pattern "dị afọ"; examples include
    // "Ọ dị afọ ise" (he is five years old) and "Afọ ole ka ị dị?" (how old are you?).
    // Sources: https://nkowaokwu.com/word?word=year and
    // https://elias.fas.harvard.edu/languages/igbo/beginning/3/introducing-self-and-others
    [Theory]
    [InlineData(false, "dị afọ 1")]
    [InlineData(true, "dị otu afọ")]
    public void ToAge_UsesIgboTemplate(bool toWords, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(366).ToAge(new CultureInfo("ig"), toWords: toWords));
    }
}