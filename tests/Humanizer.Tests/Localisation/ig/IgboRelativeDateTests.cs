namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ụnyaahụ")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "echi")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ụbọchị gara aga")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "n'ime 2 ụbọchị")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "otu ọnwa gara aga")]
    [InlineData(2, TimeUnit.Month, Tense.Past, "2 ọnwa gara aga")]
    [InlineData(1, TimeUnit.Month, Tense.Future, "n'ime otu ọnwa")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ugbu a")]
    public void DateHumanize_UsesIgboPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    // Dictionary references for "never" include "ọ dịghị mgbe"; see https://www.learnentry.com/english-igbo/meaning/never/.
    [Fact]
    public void NullableDateHumanize_NullDateUsesIgboNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ọ dịghị mgbe", date.Humanize(culture: new CultureInfo("ig")));
    }
}