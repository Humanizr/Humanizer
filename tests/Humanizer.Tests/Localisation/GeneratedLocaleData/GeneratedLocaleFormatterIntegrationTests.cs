namespace Humanizer.Tests.Localisation;

public class GeneratedLocaleFormatterIntegrationTests
{
    [Fact]
    public void EnglishFormatterUsesGeneratedPhraseTablesForDateAndTime()
    {
        var formatter = Configurator.GetFormatter(new CultureInfo("en"));

        Assert.Equal("now", formatter.DateHumanize_Now());
        Assert.Equal("2 days ago", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 days from now", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("one day", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
        Assert.Equal("2 days", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("byte", formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));
        Assert.Equal("B", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: true));
    }

    [Fact]
    public void RussianFormatterUsesGeneratedPhraseTablesWithProfiledForms()
    {
        var formatter = Configurator.GetFormatter(new CultureInfo("ru"));

        Assert.Equal("через 2 дня", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 дня назад", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("два дня", formatter.TimeSpanHumanize(TimeUnit.Day, 2, toWords: true));
        Assert.Equal("д.", formatter.TimeUnitHumanize(TimeUnit.Day));
        Assert.Equal("Б", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: true));
    }

    [Fact]
    public void DanishFormatterCollapsesDuplicateFormsThroughGeneratedTables()
    {
        var formatter = Configurator.GetFormatter(new CultureInfo("da"));

        Assert.Equal("2 dage siden", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 dage", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("en dag", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }
}