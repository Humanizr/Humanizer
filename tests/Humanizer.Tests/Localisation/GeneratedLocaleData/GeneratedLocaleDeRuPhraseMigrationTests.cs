namespace Humanizer.Tests.Localisation;

public class GeneratedLocaleDeRuPhraseMigrationTests
{
    [Fact]
    public void GermanPhraseTableUsesYamlOwnedPhrases()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("de"));

        Assert.NotNull(table);
        Assert.Equal("jetzt", table!.DateNow);
        Assert.Equal("{value} alt", table.TimeSpanAge);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var pastDay));
        Assert.Equal("two", pastDay.Template?.Name);
        Assert.Equal("vorgestern", pastDay.Template?.Template);
        Assert.Equal("vor", pastDay.Multiple?.BeforeCountText);
        Assert.Equal("Tagen", pastDay.Multiple?.Forms.Default);

        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitByte));
        Assert.Equal("Byte", dataUnitByte.Forms?.Default);
        Assert.Equal("{unit}", dataUnitByte.Template?.Template);
    }

    [Fact]
    public void RussianPhraseTableUsesCompactGrammarAwareYamlForms()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("ru"));

        Assert.NotNull(table);
        Assert.Equal("{value}", table!.TimeSpanAge);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Future, out var futureDay));
        Assert.Equal("two", futureDay.Template?.Name);
        Assert.Equal("через два дня", futureDay.Template?.Template);
        Assert.Equal("через", futureDay.Multiple?.BeforeCountText);
        Assert.Equal("день", futureDay.Multiple?.Forms.Singular);
        Assert.Equal("дня", futureDay.Multiple?.Forms.Dual);
        Assert.Equal("дней", futureDay.Multiple?.Forms.Default);

        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Minute, out var timeSpanMinute));
        Assert.Equal("одна минута", timeSpanMinute.SingleWordsVariant);
        Assert.Equal("минута", timeSpanMinute.Multiple?.Forms.Singular);
        Assert.Equal("минуты", timeSpanMinute.Multiple?.Forms.Dual);
        Assert.Equal("минут", timeSpanMinute.Multiple?.Forms.Default);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Week, Tense.Future, out var futureWeek));
        Assert.Equal("неделю", futureWeek.Multiple?.Forms.Singular);
        Assert.Equal("недель", futureWeek.Multiple?.Forms.Default);
        Assert.Equal("недели", futureWeek.Multiple?.Forms.Dual);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Week, Tense.Past, out var pastWeek));
        Assert.Equal("неделю назад", pastWeek.Single);
        Assert.Equal("неделю", pastWeek.Multiple?.Forms.Singular);
        Assert.Equal("недель", pastWeek.Multiple?.Forms.Default);
        Assert.Equal("недели", pastWeek.Multiple?.Forms.Dual);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Millisecond, Tense.Past, out var pastMillisecond));
        Assert.Equal("миллисекунду назад", pastMillisecond.Single);
        Assert.Equal("миллисекунду", pastMillisecond.Multiple?.Forms.Singular);
        Assert.Equal("миллисекунд", pastMillisecond.Multiple?.Forms.Default);
        Assert.Equal("миллисекунды", pastMillisecond.Multiple?.Forms.Dual);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Millisecond, Tense.Future, out var futureMillisecond));
        Assert.Equal("через миллисекунду", futureMillisecond.Single);
        Assert.Equal("миллисекунду", futureMillisecond.Multiple?.Forms.Singular);
        Assert.Equal("миллисекунд", futureMillisecond.Multiple?.Forms.Default);
        Assert.Equal("миллисекунды", futureMillisecond.Multiple?.Forms.Dual);
    }

    [Theory]
    [InlineData("de-CH")]
    [InlineData("de-LI")]
    public void ChildGermanLocalesInheritParentPhraseTables(string localeCode)
    {
        Assert.Equal("in 2 Tagen", Configurator.GetFormatter(new CultureInfo(localeCode)).DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("4 Tage alt", TimeSpan.FromDays(4).ToAge(new CultureInfo(localeCode)));
    }
}