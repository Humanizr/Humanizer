namespace Humanizer.Tests.Localisation.mn;

[UseCulture("mn")]
public class MongolianPhraseTests
{
    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "өчигдөр")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "маргааш")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 хоногийн өмнө")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 хоногийн дараа")]
    [InlineData(2, TimeUnit.Hour, Tense.Past, "2 цагийн өмнө")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "одоо")]
    public void DateHumanize_UsesMongolianPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mn"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesMongolianNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("хэзээ ч үгүй", date.Humanize(culture: new CultureInfo("mn")));
    }

    [Theory]
    [InlineData(TimeUnit.Day, 1, "1 өдөр")]
    [InlineData(TimeUnit.Day, 2, "2 өдөр")]
    [InlineData(TimeUnit.Month, 2, "2 сар")]
    public void TimeSpanHumanize_UsesMongolianDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mn"));
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ToWords_UsesMongolianSingleWordForm()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mn"));
        Assert.Equal("нэг өдөр", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Fact]
    public void ToAge_UsesMongolianTemplate()
    {
        Assert.Equal("1 жил настай", TimeSpan.FromDays(366).ToAge(new CultureInfo("mn")));
    }

    [Theory]
    [InlineData(DataUnit.Byte, "байт")]
    [InlineData(DataUnit.Kilobyte, "килобайт")]
    [InlineData(DataUnit.Megabyte, "мегабайт")]
    public void DataUnitHumanize_UsesMongolianNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mn"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "сек")]
    [InlineData(TimeUnit.Minute, "мин")]
    [InlineData(TimeUnit.Hour, "цаг")]
    [InlineData(TimeUnit.Day, "өдөр")]
    public void TimeUnitHumanize_UsesMongolianLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("mn"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}