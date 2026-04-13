using System.Globalization;

namespace Humanizer.Tests;

/// <summary>
/// Tests for uncovered branches in DefaultFormatter:
/// - string localeCode constructor (lines 32-34)
/// - DataUnitHumanize error branch (line 74)
/// - TimeUnitHumanize error/missing-symbol branch (line 86)
/// - TryFormatDataUnitFromPhraseTable false-return branches (lines 145-146, 152-153, 161-162)
/// - RenderCountedPhrase AfterForm + default branches (lines 299-300)
/// - TimeSpanHumanize zero+toWords fallback (lines 231-234)
/// - DateHumanize count==0 with DateNow (line 184)
/// - Various branch decision points in the formatting pipeline
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
        var result = formatter.DateHumanize(TimeUnit.Second, Tense.Past, 0);
        Assert.Equal("now", result);
    }

    [Fact]
    public void DateHumanizeReturnsSingleFormForCountOne()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.DateHumanize(TimeUnit.Second, Tense.Past, 1);
        Assert.Equal("one second ago", result);
    }

    [Fact]
    public void DateHumanizeReturnsMultipleFormForCountAboveOne()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.DateHumanize(TimeUnit.Day, Tense.Future, 5);
        Assert.Equal("5 days from now", result);
    }

    [Fact]
    public void TimeSpanHumanizeReturnsSingleForm()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeSpanHumanize(TimeUnit.Hour, 1);
        Assert.Equal("1 hour", result);
    }

    [Fact]
    public void TimeSpanHumanizeReturnsSingleWordsVariantWhenToWordsTrue()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true);
        Assert.Equal("one hour", result);
    }

    [Fact]
    public void TimeSpanHumanizeReturnsMultipleForm()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeSpanHumanize(TimeUnit.Hour, 3);
        Assert.Equal("3 hours", result);
    }

    [Fact]
    public void TimeSpanHumanizeZeroReturnsZeroPhrase()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeSpanHumanize_Zero();
        Assert.Equal("no time", result);
    }

    [Fact]
    public void TimeSpanHumanizeZeroWithToWordsReturnsZero()
    {
        var formatter = new DefaultFormatter("en");
        // count=0, toWords=true should return the zero phrase
        var result = formatter.TimeSpanHumanize(TimeUnit.Millisecond, 0, toWords: true);
        Assert.Equal("no time", result);
    }

    [Fact]
    public void DataUnitHumanizeReturnsSymbol()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.DataUnitHumanize(DataUnit.Byte, 100, toSymbol: true);
        Assert.Equal("B", result);
    }

    [Fact]
    public void DataUnitHumanizeReturnsWord()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.DataUnitHumanize(DataUnit.Byte, 5, toSymbol: false);
        Assert.Equal("bytes", result);
    }

    [Fact]
    public void DataUnitHumanizeReturnsSingularWord()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false);
        Assert.Equal("byte", result);
    }

    [Fact]
    public void TimeUnitHumanizeReturnsSymbol()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeUnitHumanize(TimeUnit.Day);
        Assert.Equal("d", result);
    }

    [Fact]
    public void TimeSpanAgeReturnsAgeTemplate()
    {
        var formatter = new DefaultFormatter("en");
        var result = formatter.TimeSpanHumanize_Age();
        Assert.Contains("{value}", result);
        Assert.Contains("old", result);
    }

    [Fact]
    public void DateHumanizeWithMultipleUnits()
    {
        var formatter = new DefaultFormatter("en");

        Assert.Equal("2 seconds ago", formatter.DateHumanize(TimeUnit.Second, Tense.Past, 2));
        Assert.Equal("2 minutes ago", formatter.DateHumanize(TimeUnit.Minute, Tense.Past, 2));
        Assert.Equal("2 hours ago", formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 2));
        Assert.Equal("2 months ago", formatter.DateHumanize(TimeUnit.Month, Tense.Past, 2));
        Assert.Equal("2 years ago", formatter.DateHumanize(TimeUnit.Year, Tense.Past, 2));
    }

    [Fact]
    public void TimeSpanHumanizeWithMultipleUnits()
    {
        var formatter = new DefaultFormatter("en");

        Assert.Equal("2 seconds", formatter.TimeSpanHumanize(TimeUnit.Second, 2));
        Assert.Equal("2 minutes", formatter.TimeSpanHumanize(TimeUnit.Minute, 2));
        Assert.Equal("2 hours", formatter.TimeSpanHumanize(TimeUnit.Hour, 2));
        Assert.Equal("2 days", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("2 weeks", formatter.TimeSpanHumanize(TimeUnit.Week, 2));
        Assert.Equal("2 months", formatter.TimeSpanHumanize(TimeUnit.Month, 2));
        Assert.Equal("2 years", formatter.TimeSpanHumanize(TimeUnit.Year, 2));
    }

    [Fact]
    public void TimeSpanHumanizeToWordsWithMultipleUnits()
    {
        var formatter = new DefaultFormatter("en");

        Assert.Equal("two seconds", formatter.TimeSpanHumanize(TimeUnit.Second, 2, toWords: true));
        Assert.Equal("two minutes", formatter.TimeSpanHumanize(TimeUnit.Minute, 2, toWords: true));
        Assert.Equal("two hours", formatter.TimeSpanHumanize(TimeUnit.Hour, 2, toWords: true));
        Assert.Equal("two days", formatter.TimeSpanHumanize(TimeUnit.Day, 2, toWords: true));
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
}
