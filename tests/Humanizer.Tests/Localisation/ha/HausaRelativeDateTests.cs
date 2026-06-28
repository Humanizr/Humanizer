namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "jiya")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "gobe")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 kwanaki da suka wuce")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "nan da kwanaki 2")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "wata 1 da ya wuce")]
    [InlineData(2, TimeUnit.Month, Tense.Past, "2 watanni da suka wuce")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "yanzu")]
    public void DateHumanize_UsesHausaPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesHausaNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ba a taɓa ba", date.Humanize(culture: new CultureInfo("ha")));
    }
}