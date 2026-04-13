using System.Globalization;

namespace Humanizer.Tests;

/// <summary>
/// Tests for DefaultFormatter covering branches via the string locale constructor,
/// generated phrase table pipeline, and error/fallback paths:
/// - string localeCode constructor (line 32-34)
/// - DateHumanize count==0 DateNow branch (line 184-187)
/// - DateHumanize single form for count==1 (line 200-204)
/// - DateHumanize exact-two template branch via Arabic/Maltese (line 206-212)
/// - DateHumanize multiple form with various counts (line 219-225)
/// - DateHumanize error throw when ShouldUseDatePhraseTable returns false (lines 46-48, 195-197)
/// - TimeSpanHumanize zero+toWords fallback (line 231-234)
/// - TimeSpanHumanize SingleWordsVariant path (line 249)
/// - TimeSpanHumanize multiple form rendering (line 263-269)
/// - TimeSpanHumanize error throw when ShouldUseTimeSpanPhraseTable returns false (lines 55-58, 242-244)
/// - DataUnitHumanize symbol and word paths (lines 149-178)
/// - TimeUnitHumanize symbol path for all TimeUnit values (line 80-84)
/// - RenderCountedPhrase with PhraseCountPlacement.None via Romanian (line 282-284)
/// </summary>
[UseCulture("en-US")]
public class DefaultFormatterTests
{
    [Fact]
    public void StringConstructorCreatesFormatterForLocaleCode()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("now", formatter.DateHumanize_Now());
        Assert.Equal("never", formatter.DateHumanize_Never());
    }

    [Fact]
    public void StringConstructorCreatesFormatterForJapanese()
    {
        var formatter = new DefaultFormatter("ja");
        Assert.Equal("今", formatter.DateHumanize_Now());
    }

    [Fact]
    public void DateHumanizeReturnsNowForZeroCount()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("now", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 0));
        Assert.Equal("now", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 0));
    }

    [Fact]
    public void DateHumanizeReturnsSingleFormForCountOne()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("one second ago", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 1));
        Assert.Equal("one second from now", formatter.DateHumanize(TimeUnit.Second, Tense.Future, 1));
        Assert.Equal("yesterday", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal("tomorrow", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 1));
    }

    [Fact]
    public void DateHumanizeReturnsMultipleFormForCountAboveOne()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("5 days from now", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 5));
        Assert.Equal("3 hours ago", formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 3));
        Assert.Equal("10 minutes from now", formatter.DateHumanize(TimeUnit.Minute, Tense.Future, 10));
    }

    [Fact]
    public void DateHumanizeExactTwoTemplateForMaltese()
    {
        // Maltese has "two" template entries (e.g., "saghtejn ilu" for 2 hours past)
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mt"));
        Assert.Equal("sagħtejn ilu", formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 2));
    }

    [Fact]
    public void DateHumanizeExactTwoTemplateForArabic()
    {
        // Arabic has "two" template entries for dual forms
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ar"));
        Assert.Equal("منذ يومين", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
    }

    [Fact]
    public void DateHumanizeNoneCountPlacementViaRomanian()
    {
        // Romanian uses PhraseCountPlacement.None for some counted phrases,
        // exercising the RenderCountedPhrase None branch (line 282-284)
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ro-RO"));
        Assert.Equal("acum 59 de secunde", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 59));
        Assert.Equal("peste 21 de ore", formatter.DateHumanize(TimeUnit.Hour, Tense.Future, 21));
    }

    [Fact]
    public void TimeSpanHumanizeReturnsSingleForm()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("1 hour", formatter.TimeSpanHumanize(TimeUnit.Hour, 1));
        Assert.Equal("1 day", formatter.TimeSpanHumanize(TimeUnit.Day, 1));
    }

    [Fact]
    public void TimeSpanHumanizeReturnsSingleWordsVariantWhenToWordsTrue()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("one hour", formatter.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true));
        Assert.Equal("one day", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void TimeSpanHumanizeReturnsMultipleForm()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("3 hours", formatter.TimeSpanHumanize(TimeUnit.Hour, 3));
        Assert.Equal("5 days", formatter.TimeSpanHumanize(TimeUnit.Day, 5));
    }

    [Fact]
    public void TimeSpanHumanizeZeroReturnsZeroPhrase()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("no time", formatter.TimeSpanHumanize_Zero());
    }

    [Fact]
    public void TimeSpanHumanizeZeroWithToWordsReturnsZero()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("no time", formatter.TimeSpanHumanize(TimeUnit.Millisecond, 0, toWords: true));
    }

    [Fact]
    public void TimeSpanHumanizeToWordsMultipleForm()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("two seconds", formatter.TimeSpanHumanize(TimeUnit.Second, 2, toWords: true));
        Assert.Equal("three minutes", formatter.TimeSpanHumanize(TimeUnit.Minute, 3, toWords: true));
        Assert.Equal("two hours", formatter.TimeSpanHumanize(TimeUnit.Hour, 2, toWords: true));
        Assert.Equal("two days", formatter.TimeSpanHumanize(TimeUnit.Day, 2, toWords: true));
    }

    [Fact]
    public void DataUnitHumanizeReturnsSymbol()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("B", formatter.DataUnitHumanize(DataUnit.Byte, 100, toSymbol: true));
    }

    [Fact]
    public void DataUnitHumanizeReturnsPluralWord()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("bytes", formatter.DataUnitHumanize(DataUnit.Byte, 5, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanizeReturnsSingularWord()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("byte", formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanizeAllUnitsSymbol()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("b", formatter.DataUnitHumanize(DataUnit.Bit, 100, toSymbol: true));
        Assert.Equal("B", formatter.DataUnitHumanize(DataUnit.Byte, 100, toSymbol: true));
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 100, toSymbol: true));
        Assert.Equal("MB", formatter.DataUnitHumanize(DataUnit.Megabyte, 100, toSymbol: true));
        Assert.Equal("GB", formatter.DataUnitHumanize(DataUnit.Gigabyte, 100, toSymbol: true));
        Assert.Equal("TB", formatter.DataUnitHumanize(DataUnit.Terabyte, 100, toSymbol: true));
    }

    [Fact]
    public void DataUnitHumanizeAllUnitsWord()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("bits", formatter.DataUnitHumanize(DataUnit.Bit, 2, toSymbol: false));
        Assert.Equal("bytes", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("kilobytes", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: false));
        Assert.Equal("megabytes", formatter.DataUnitHumanize(DataUnit.Megabyte, 2, toSymbol: false));
        Assert.Equal("gigabytes", formatter.DataUnitHumanize(DataUnit.Gigabyte, 2, toSymbol: false));
        Assert.Equal("terabytes", formatter.DataUnitHumanize(DataUnit.Terabyte, 2, toSymbol: false));
    }

    [Fact]
    public void TimeUnitHumanizeAllUnits()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("ms", formatter.TimeUnitHumanize(TimeUnit.Millisecond));
        Assert.Equal("s", formatter.TimeUnitHumanize(TimeUnit.Second));
        Assert.Equal("min", formatter.TimeUnitHumanize(TimeUnit.Minute));
        Assert.Equal("h", formatter.TimeUnitHumanize(TimeUnit.Hour));
        Assert.Equal("d", formatter.TimeUnitHumanize(TimeUnit.Day));
        Assert.Equal("week", formatter.TimeUnitHumanize(TimeUnit.Week));
        Assert.Equal("mo", formatter.TimeUnitHumanize(TimeUnit.Month));
        Assert.Equal("y", formatter.TimeUnitHumanize(TimeUnit.Year));
    }

    [Fact]
    public void TimeSpanAgeReturnsAgeTemplate()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("{value} old", formatter.TimeSpanHumanize_Age());
    }

    [Fact]
    public void DateHumanizeThrowsWhenPhraseTableRejected()
    {
        var formatter = new RejectingFormatter("en");
        var ex = Assert.Throws<InvalidOperationException>(
            () => formatter.DateHumanize(TimeUnit.Day, Tense.Past, 5));
        Assert.Contains("Missing generated relative-date phrase", ex.Message);
        Assert.Contains("Day", ex.Message);
    }

    [Fact]
    public void TimeSpanHumanizeThrowsWhenPhraseTableRejected()
    {
        var formatter = new RejectingFormatter("en");
        var ex = Assert.Throws<InvalidOperationException>(
            () => formatter.TimeSpanHumanize(TimeUnit.Hour, 3));
        Assert.Contains("Missing generated time-span phrase", ex.Message);
        Assert.Contains("Hour", ex.Message);
    }

    [Fact]
    public void DateHumanizeWithAllTimeUnits()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("2 milliseconds ago", formatter.DateHumanize(TimeUnit.Millisecond, Tense.Past, 2));
        Assert.Equal("2 seconds ago", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 2));
        Assert.Equal("2 minutes ago", formatter.DateHumanize(TimeUnit.Minute, Tense.Past, 2));
        Assert.Equal("2 hours ago", formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 2));
        Assert.Equal("2 days ago", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 weeks ago", formatter.DateHumanize(TimeUnit.Week, Tense.Past, 2));
        Assert.Equal("2 months ago", formatter.DateHumanize(TimeUnit.Month, Tense.Past, 2));
        Assert.Equal("2 years ago", formatter.DateHumanize(TimeUnit.Year, Tense.Past, 2));
    }

    [Fact]
    public void TimeSpanHumanizeWithAllTimeUnits()
    {
        var formatter = new DefaultFormatter("en");
        Assert.Equal("2 milliseconds", formatter.TimeSpanHumanize(TimeUnit.Millisecond, 2));
        Assert.Equal("2 seconds", formatter.TimeSpanHumanize(TimeUnit.Second, 2));
        Assert.Equal("2 minutes", formatter.TimeSpanHumanize(TimeUnit.Minute, 2));
        Assert.Equal("2 hours", formatter.TimeSpanHumanize(TimeUnit.Hour, 2));
        Assert.Equal("2 days", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("2 weeks", formatter.TimeSpanHumanize(TimeUnit.Week, 2));
        Assert.Equal("2 months", formatter.TimeSpanHumanize(TimeUnit.Month, 2));
        Assert.Equal("2 years", formatter.TimeSpanHumanize(TimeUnit.Year, 2));
    }

    /// <summary>
    /// Test-only subclass that overrides ShouldUseDatePhraseTable and ShouldUseTimeSpanPhraseTable
    /// to return false, forcing TryFormat* to fail and triggering the error throw branches
    /// in DateHumanize (line 46-48) and TimeSpanHumanize (line 55-58).
    /// </summary>
    sealed class RejectingFormatter(string localeCode) : DefaultFormatter(localeCode)
    {
        internal override bool ShouldUseDatePhraseTable(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
            false;

        internal override bool ShouldUseTimeSpanPhraseTable(TimeUnit unit, int count, bool toWords, LocalizedTimeSpanPhrase phrase) =>
            false;
    }
}
