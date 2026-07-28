namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "ایک سیکنڈ پہلے")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 سیکنڈ میں")]
    [InlineData(1, TimeUnit.Minute, Tense.Past, "ایک منٹ پہلے")]
    [InlineData(2, TimeUnit.Minute, Tense.Future, "2 منٹ میں")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ایک گھنٹہ پہلے")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 گھنٹے میں")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "گزشتہ کل")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "آئندہ کل")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 دن پہلے")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 دنوں میں")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "ایک مہینہ پہلے")]
    [InlineData(2, TimeUnit.Month, Tense.Future, "2 مہینے میں")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "ایک سال پہلے")]
    [InlineData(2, TimeUnit.Year, Tense.Future, "2 سال میں")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ابھی")]
    public void DateHumanize_SingularAndPluralPerUnit(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ur"));
        var result = formatter.DateHumanize(unit, tense, count);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    /// <summary>
    /// Verifies stem distinctions between singular (one) and plural (other) counts.
    /// </summary>
    [Fact]
    public void StemDistinctions_SingularVsPlural()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ur"));

        // گھنٹہ (singular) vs گھنٹے (plural)
        var hourSingular = formatter.DateHumanize(TimeUnit.Hour, Tense.Past, 1);
        var hourPlural = formatter.DateHumanize(TimeUnit.Hour, Tense.Future, 2);
        Assert.Contains("گھنٹہ", hourSingular);
        Assert.Contains("گھنٹے", hourPlural);
        UrduBidiControlSweep.AssertNoBidiControls(hourSingular);
        UrduBidiControlSweep.AssertNoBidiControls(hourPlural);

        // دن (direct case, past) vs دنوں (oblique case, future)
        var dayPastDirect = formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2);
        var dayFutureOblique = formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2);
        Assert.Contains("دن", dayPastDirect);
        Assert.Contains("دنوں", dayFutureOblique);
        UrduBidiControlSweep.AssertNoBidiControls(dayPastDirect);
        UrduBidiControlSweep.AssertNoBidiControls(dayFutureOblique);

        // مہینہ (singular) vs مہینے (plural)
        var monthSingular = formatter.DateHumanize(TimeUnit.Month, Tense.Past, 1);
        var monthPlural = formatter.DateHumanize(TimeUnit.Month, Tense.Future, 2);
        Assert.Contains("مہینہ", monthSingular);
        Assert.Contains("مہینے", monthPlural);
        UrduBidiControlSweep.AssertNoBidiControls(monthSingular);
        UrduBidiControlSweep.AssertNoBidiControls(monthPlural);
    }
}