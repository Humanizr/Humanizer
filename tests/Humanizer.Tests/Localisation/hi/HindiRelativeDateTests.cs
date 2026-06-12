namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "एक सेकंड पहले")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 सेकंड में")]
    [InlineData(1, TimeUnit.Minute, Tense.Past, "एक मिनट पहले")]
    [InlineData(2, TimeUnit.Minute, Tense.Future, "2 मिनट में")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "एक घंटा पहले")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 घंटे में")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "कल")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "कल")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 दिन पहले")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 दिन में")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "एक महीना पहले")]
    [InlineData(2, TimeUnit.Month, Tense.Future, "2 महीने में")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "एक साल पहले")]
    [InlineData(2, TimeUnit.Year, Tense.Future, "2 साल में")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "अभी")]
    public void DateHumanize_SingularAndPluralPerUnit(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("hi"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesHindiNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("कभी नहीं", date.Humanize(culture: new CultureInfo("hi")));
    }
}