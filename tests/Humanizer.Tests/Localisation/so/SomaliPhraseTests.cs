namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliPhraseTests
{
    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "shalay")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "berri")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 maalmood ka hor")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 maalmood kadib")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "1 bil ka hor")]
    [InlineData(2, TimeUnit.Month, Tense.Past, "2 bilood ka hor")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "hadda")]
    public void DateHumanize_UsesSomaliPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesSomaliNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("marnaba", date.Humanize(culture: new CultureInfo("so")));
    }

    [Theory]
    [InlineData(TimeUnit.Day, 1, "1 maalin")]
    [InlineData(TimeUnit.Day, 2, "2 maalmood")]
    [InlineData(TimeUnit.Month, 1, "1 bil")]
    [InlineData(TimeUnit.Month, 2, "2 bilood")]
    public void TimeSpanHumanize_UsesSomaliDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesSomaliSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal("hal maalin", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void ToAge_UsesSomaliTemplate()
    {
        Assert.Equal("1 sano", TimeSpan.FromDays(366).ToAge(new CultureInfo("so")));
    }
}