namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaDurationTests
{
    [Theory]
    [InlineData(TimeUnit.Day, 1, "kwana 1")]
    [InlineData(TimeUnit.Day, 2, "kwanaki 2")]
    [InlineData(TimeUnit.Month, 1, "wata 1")]
    [InlineData(TimeUnit.Month, 2, "watanni 2")]
    public void TimeSpanHumanize_UsesHausaDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesHausaSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal("kwana ɗaya", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void ToAge_UsesHausaTemplate()
    {
        Assert.Equal("shekara 1", TimeSpan.FromDays(366).ToAge(new CultureInfo("ha")));
    }
}