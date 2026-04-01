using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class GeneratedFormatterPhraseTableTests
{
    [Fact]
    public void EnglishPhraseTableExposesCanonicalFormatterPhrases()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("en"));

        Assert.NotNull(table);
        Assert.Equal("now", table!.DateNow);
        Assert.Equal("never", table.DateNever);
        Assert.Equal("no time", table.TimeSpanZero);
        Assert.Equal("{value} old", table.TimeSpanAge);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var pastDay));
        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Future, out var futureDay));
        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Day, out var timeSpanDay));
        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitByte));
        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var timeUnitDay));

        Assert.Equal("yesterday", pastDay.Single);
        Assert.Null(pastDay.Template);
        Assert.Equal("day", pastDay.Multiple?.Forms.Singular);
        Assert.Equal("days", pastDay.Multiple?.Forms.Default);
        Assert.Equal(PhraseCountPlacement.BeforeForm, pastDay.Multiple?.CountPlacement);
        Assert.Null(pastDay.Multiple?.BeforeCountText);
        Assert.Equal("ago", pastDay.Multiple?.AfterCountText);

        Assert.Null(futureDay.Template);
        Assert.Equal("day", futureDay.Multiple?.Forms.Singular);
        Assert.Equal("days", futureDay.Multiple?.Forms.Default);
        Assert.Equal("from now", futureDay.Multiple?.AfterCountText);

        Assert.Equal("1 day", timeSpanDay.Single);
        Assert.Equal("one day", timeSpanDay.SingleWordsVariant);
        Assert.Equal("day", timeSpanDay.Multiple?.Forms.Singular);
        Assert.Equal("days", timeSpanDay.Multiple?.Forms.Default);

        Assert.Equal("byte", dataUnitByte.Forms?.Default);
        Assert.Equal("B", dataUnitByte.Symbol);
        Assert.Equal("d", timeUnitDay.Symbol);
    }

    [Fact]
    public void JapanesePhraseTableExposesCanonicalFormatterPhrases()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("ja"));

        Assert.NotNull(table);
        Assert.Equal("今", table!.DateNow);
        Assert.Equal("決して", table.DateNever);
        Assert.Equal("0 秒", table.TimeSpanZero);
        Assert.Equal("{value}", table.TimeSpanAge);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var pastDay));
        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Future, out var futureDay));
        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Day, out var timeSpanDay));
        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitByte));
        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var timeUnitDay));

        Assert.Equal("昨日", pastDay.Single);
        Assert.Equal("two", pastDay.Template?.Name);
        Assert.Equal("2 日前", pastDay.Template?.Template);
        Assert.Equal("{count} {unit}前", pastDay.Multiple?.Template?.Template);
        Assert.Equal("日", pastDay.Multiple?.Forms.Default);
        Assert.Equal(PhraseCountPlacement.BeforeForm, pastDay.Multiple?.CountPlacement);
        Assert.Null(pastDay.Multiple?.BeforeCountText);
        Assert.Null(pastDay.Multiple?.AfterCountText);

        Assert.Equal("日", futureDay.Multiple?.Forms.Default);
        Assert.Equal("{count} {unit}後", futureDay.Multiple?.Template?.Template);
        Assert.Null(futureDay.Multiple?.AfterCountText);

        Assert.Equal("1 日", timeSpanDay.Single);
        Assert.Equal("1 日", timeSpanDay.SingleWordsVariant);
        Assert.Equal("日", timeSpanDay.Multiple?.Forms.Default);

        Assert.Equal("バイト", dataUnitByte.Forms?.Default);
        Assert.Equal("B", dataUnitByte.Symbol);
        Assert.Equal("日", timeUnitDay.Symbol);
    }

    [Fact]
    public void RussianPhraseTablePreservesPrefixPlacementAndDistinctForms()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("ru"));

        Assert.NotNull(table);
        Assert.True(table!.TryGetDatePhrase(TimeUnit.Day, Tense.Future, out var futureDay));
        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Day, out var timeSpanDay));
        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitByte));
        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var timeUnitDay));

        Assert.Equal("через", futureDay.Multiple?.BeforeCountText);
        Assert.Null(futureDay.Multiple?.AfterCountText);
        Assert.Equal("день", futureDay.Multiple?.Forms.Singular);
        Assert.Equal("дня", futureDay.Multiple?.Forms.Dual);
        Assert.Equal("дней", futureDay.Multiple?.Forms.Default);

        Assert.Equal("один день", timeSpanDay.SingleWordsVariant);
        Assert.Equal("день", timeSpanDay.Multiple?.Forms.Singular);
        Assert.Equal("дня", timeSpanDay.Multiple?.Forms.Dual);
        Assert.Equal("дней", timeSpanDay.Multiple?.Forms.Default);
        Assert.Equal("Б", dataUnitByte.Symbol);
        Assert.Equal("д.", timeUnitDay.Symbol);
    }

    [Fact]
    public void SlovenianPhraseTableKeepsIrregularFormsDistinct()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("sl"));

        Assert.NotNull(table);
        Assert.True(table!.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var pastDay));
        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Day, out var timeSpanDay));
        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitByte));

        Assert.Equal("pred", pastDay.Multiple?.BeforeCountText);
        Assert.Equal("dnevi", pastDay.Multiple?.Forms.Default);
        Assert.Equal("dnevoma", pastDay.Multiple?.Forms.Singular);
        Assert.Null(pastDay.Multiple?.Forms.Paucal);

        Assert.Equal("dni", timeSpanDay.Multiple?.Forms.Default);
        Assert.Equal("dneva", timeSpanDay.Multiple?.Forms.Singular);
        Assert.Null(timeSpanDay.Multiple?.Forms.Paucal);

        Assert.Equal("bajt", dataUnitByte.Forms?.Singular);
        Assert.Equal("bajta", dataUnitByte.Forms?.Dual);
        Assert.Equal("bajti", dataUnitByte.Forms?.Paucal);
        Assert.Equal("bajtov", dataUnitByte.Forms?.Default);
        Assert.Equal("B", dataUnitByte.Symbol);
    }

    [Fact]
    public void DanishPhraseTableCollapsesExactDuplicateForms()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("da"));

        Assert.NotNull(table);
        Assert.True(table!.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var pastDay));
        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Day, out var timeSpanDay));
        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var timeUnitDay));

        Assert.Equal("dage", pastDay.Multiple?.Forms.Default);
        Assert.Null(pastDay.Multiple?.Forms.Singular);
        Assert.Null(pastDay.Multiple?.Forms.Dual);
        Assert.Null(pastDay.Multiple?.Forms.Paucal);
        Assert.Null(pastDay.Multiple?.Forms.Plural);
        Assert.Equal("siden", pastDay.Multiple?.AfterCountText);

        Assert.Equal("dage", timeSpanDay.Multiple?.Forms.Default);
        Assert.Null(timeSpanDay.Multiple?.Forms.Singular);
        Assert.Equal("en dag", timeSpanDay.SingleWordsVariant);
        Assert.Equal("dag", timeUnitDay.Symbol);
    }

    [Fact]
    public void LocalePhraseTableCatalogFallsBackToParentCulture()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("en-US"));

        Assert.NotNull(table);
        Assert.Equal("now", table!.DateNow);
    }
}
