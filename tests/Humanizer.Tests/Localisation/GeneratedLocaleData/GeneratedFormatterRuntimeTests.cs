using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class GeneratedFormatterRuntimeTests
{
    [Fact]
    public void DefaultFormatterUsesGeneratedPhraseTablesForEnglishHotPaths()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("en"));

        Assert.Equal("now", formatter.DateHumanize_Now());
        Assert.Equal("never", formatter.DateHumanize_Never());
        Assert.Equal("2 days ago", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 days", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("one day", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
        Assert.Equal("bytes", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("d", formatter.TimeUnitHumanize(TimeUnit.Day));
        Assert.Equal("1 year old", TimeSpan.FromDays(366).ToAge(new CultureInfo("en")));
    }

    [Fact]
    public void DefaultFormatterUsesGeneratedPhraseTablesForJapaneseHotPaths()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ja"));

        Assert.Equal("今", formatter.DateHumanize_Now());
        Assert.Equal("決して", formatter.DateHumanize_Never());
        Assert.Equal("2 日前", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 日後", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 日", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("1 日", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
        Assert.Equal("バイト", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("日", formatter.TimeUnitHumanize(TimeUnit.Day));
        Assert.Equal("1 年", TimeSpan.FromDays(366).ToAge(new CultureInfo("ja")));
    }

    [Fact]
    public void GeneratedPhraseTablesUseTamilOwnedLocaleData()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ta"));

        Assert.Equal("இப்போது", formatter.DateHumanize_Now());
        Assert.Equal("ஒருபோதும் இல்லை", formatter.DateHumanize_Never());
        Assert.Equal("2 நாட்கள் முன்", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 நாட்களில்", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 நாட்கள்", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("ஒரு நாள்", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
        Assert.Equal("பைட்கள்", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("நாள்", formatter.TimeUnitHumanize(TimeUnit.Day));
    }

    [Fact]
    public void GeneratedPhraseTablesUseZuluOwnedLocaleData()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("zu-ZA"));

        Assert.Equal("manje", formatter.DateHumanize_Now());
        Assert.Equal("akakaze", formatter.DateHumanize_Never());
        Assert.Equal("2 izinsuku ezedlule", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 izinsuku ezizayo", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("2 izinsuku", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("1 usuku", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
        Assert.Equal("ama-bytes", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("usuku", formatter.TimeUnitHumanize(TimeUnit.Day));
    }

    [Fact]
    public void ProfiledFormatterUsesGeneratedPhraseTablesForRussianGrammarSensitivePhrases()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ru"));

        Assert.Equal("через 2 дня", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("через 5 дней", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 5));
        Assert.Equal("2 дня", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("один день", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void LegacyPatternFallbackStillPreservesFrenchExactDayForms()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("fr"));

        Assert.Equal("avant-hier", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("après-demain", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
    }

    [Fact]
    public void ProfiledFormatterUsesCollapsedSingularFallbackForDualAndPaucalFamilies()
    {
        var slovenianFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sl"));
        var arabicFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ar"));

        Assert.Equal("pred 2 dnevoma", slovenianFormatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("čez 2 uri", slovenianFormatter.DateHumanize(TimeUnit.Hour, Tense.Future, 2));
        Assert.Equal("منذ يومين", arabicFormatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("في غضون يومين من الآن", arabicFormatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
    }

    [Fact]
    public void GeneratedPhraseTablesPreserveSlovenianPaucalAndZeroBehaviors()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sl"));

        Assert.Equal("pred 3 dnevi", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 3));
        Assert.Equal("pred 4 urami", formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 4));
        Assert.Equal("3 dni", formatter.TimeSpanHumanize(TimeUnit.Day, 3));
        Assert.Equal("4 minute", formatter.TimeSpanHumanize(TimeUnit.Minute, 4));
        Assert.Equal("0 milisekund", formatter.TimeSpanHumanize(TimeUnit.Millisecond, 0));
        Assert.Equal("nič časa", formatter.TimeSpanHumanize(TimeUnit.Millisecond, 0, toWords: true));
    }

    [Fact]
    public void YamlAuthoredJapanesePhrasesPreserveRelativeDateAndAgeFormatting()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ja"));

        Assert.Equal("2 日前", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 日後", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));
        Assert.Equal("0 秒", formatter.TimeSpanHumanize(TimeUnit.Millisecond, 0, toWords: true));
        Assert.Equal("1 年", TimeSpan.FromDays(366).ToAge(new CultureInfo("ja")));
    }

    [Fact]
    public void ImportedPhraseTablesPreservePaucalMultiWordAndZeroDateBehaviors()
    {
        var polishFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("pl"));
        var vietnameseFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("vi"));
        var malteseFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mt"));
        var koreanFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ko-KR"));
        var englishFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("en"));

        Assert.Equal("za 2 godziny", polishFormatter.DateHumanize(TimeUnit.Hour, Tense.Future, 2));
        Assert.Equal("2 lata", polishFormatter.TimeSpanHumanize(TimeUnit.Year, 2));
        Assert.Equal("2 mili giây", vietnameseFormatter.TimeSpanHumanize(TimeUnit.Millisecond, 2));
        Assert.Equal("bytes", malteseFormatter.DataUnitHumanize(DataUnit.Byte, 10, toSymbol: false));
        Assert.Equal("2개월", koreanFormatter.TimeSpanHumanize(TimeUnit.Month, 2));
        Assert.Equal("now", englishFormatter.DateHumanize(TimeUnit.Millisecond, Tense.Future, 0));
    }

    [Fact]
    public void GeneratedPhraseTablesPreserveRomanianInsertedPrepositions()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ro-RO"));

        Assert.Equal("acum 59 de secunde", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 59));
        Assert.Equal("peste 21 de ore", formatter.DateHumanize(TimeUnit.Hour, Tense.Future, 21));
        Assert.Equal("21 de secunde", formatter.TimeSpanHumanize(TimeUnit.Second, 21));
        Assert.Equal("0 de milisecunde", TimeSpan.Zero.Humanize(culture: new CultureInfo("ro-RO")));
        Assert.Equal("0 secunde", TimeSpan.Zero.Humanize(toWords: true, culture: new CultureInfo("ro-RO")));
    }

    [Fact]
    public void GeneratedPhraseTablesHonorProfiledExactCountFormsForMalteseAndFrench()
    {
        var malteseFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mt"));
        var frenchFormatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("fr"));

        Assert.Equal("sagħtejn ilu", malteseFormatter.DateHumanize(TimeUnit.Hour, Tense.Past, 2));
        Assert.Equal("xahrejn", malteseFormatter.TimeSpanHumanize(TimeUnit.Month, 2));
        Assert.Equal("0 heure", TimeSpan.Zero.Humanize(minUnit: TimeUnit.Hour, culture: new CultureInfo("fr")));
        Assert.Equal("0 milliseconde", TimeSpan.Zero.Humanize(minUnit: TimeUnit.Millisecond, culture: new CultureInfo("fr")));
    }

    [Fact]
    public void GeneratedPhraseTablesFallBackToEnglishForUnknownCultures()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("eo"));

        Assert.Equal("now", formatter.DateHumanize_Now());
        Assert.Equal("never", formatter.DateHumanize_Never());
        Assert.Equal("2 days ago", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 days", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("bytes", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("d", formatter.TimeUnitHumanize(TimeUnit.Day));
    }

}
