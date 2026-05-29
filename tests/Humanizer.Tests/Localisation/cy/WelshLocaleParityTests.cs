namespace Humanizer.Tests.Localisation.cy;

[UseCulture("cy")]
public class WelshLocaleParityTests
{
    static readonly CultureInfo Cy = new("cy");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesWelshConjunction()
    {
        Assert.Equal("1 a 2", Pair.Humanize());
        Assert.Equal("1, 2 a 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ddoe")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "yfory")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 diwrnod yn ôl")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "mewn 2 diwrnod")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "nawr")]
    public void DateHumanize_UsesWelshPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Cy);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesWelshNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("byth", date.Humanize(culture: Cy));
    }

    [Theory]
    [InlineData(TimeUnit.Day, 1, false, "1 diwrnod")]
    [InlineData(TimeUnit.Day, 1, true, "un diwrnod")]
    [InlineData(TimeUnit.Day, 2, false, "2 diwrnod")]
    [InlineData(TimeUnit.Month, 2, false, "2 mis")]
    public void TimeSpanHumanize_UsesWelshDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Cy);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void ToAge_UsesWelshTemplate()
    {
        Assert.Equal("1 flwyddyn oed", TimeSpan.FromDays(366).ToAge(Cy));
    }

    [Theory]
    [InlineData(DataUnit.Byte, "beit")]
    [InlineData(DataUnit.Kilobyte, "cilobeit")]
    [InlineData(DataUnit.Megabyte, "megabeit")]
    public void DataUnitHumanize_UsesWelshNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Cy);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "e")]
    [InlineData(TimeUnit.Minute, "mun")]
    [InlineData(TimeUnit.Hour, "awr")]
    [InlineData(TimeUnit.Day, "diw")]
    public void TimeUnitHumanize_UsesWelshLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Cy);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "sero")]
    [InlineData(1, "un")]
    [InlineData(11, "un deg un")]
    [InlineData(21, "dau ddeg un")]
    [InlineData(99, "naw deg naw")]
    [InlineData(100, "cant")]
    [InlineData(101, "cant ac un")]
    [InlineData(105, "cant a pump")]
    [InlineData(108, "cant ac wyth")]
    [InlineData(121, "cant a dau ddeg un")]
    [InlineData(1000, "mil")]
    [InlineData(1001, "mil ac un")]
    [InlineData(2021, "dwy fil a dau ddeg un")]
    [InlineData(1000001L, "miliwn ac un")]
    [InlineData(1001000001L, "biliwn miliwn ac un")]
    [InlineData(2000000L, "dwy filiwn")]
    [InlineData(2000000000L, "dau filiwn")]
    [InlineData(2000000000000L, "dau driliwn")]
    [InlineData(3000000000000L, "tri thriliwn")]
    [InlineData(6000000000000L, "chwe thriliwn")]
    [InlineData(2000000000000000L, "dau gwadriliwn")]
    [InlineData(3000000000000000L, "tri chwadriliwn")]
    [InlineData(6000000000000000L, "chwe chwadriliwn")]
    [InlineData(2000000000000000000L, "dau gwintiliwn")]
    [InlineData(3000000000000000000L, "tri chwintiliwn")]
    [InlineData(6000000000000000000L, "chwe chwintiliwn")]
    public void NumberToWords_ProducesExpectedWelshOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Cy));
    }

    [Theory]
    [InlineData(-5, "minws pump")]
    [InlineData(-1000, "minws mil")]
    public void NumberToWords_UsesWelshNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Cy));
    }

    [Theory]
    [InlineData(1, "cyntaf")]
    [InlineData(2, "ail")]
    [InlineData(21, "unfed ar hugain")]
    [InlineData(-1, "minws cyntaf")]
    public void NumberToOrdinalWords_ProducesExpectedWelshOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Cy));
    }

    [Theory]
    [InlineData(21, "dau ddeg un")]
    [InlineData(101, "cant ac un")]
    [InlineData(105, "cant a pump")]
    [InlineData(108, "cant ac wyth")]
    [InlineData(121, "cant a dau ddeg un")]
    [InlineData(1001, "mil ac un")]
    [InlineData(2021, "dwy fil a dau ddeg un")]
    [InlineData(1000001L, "miliwn ac un")]
    [InlineData(1001000001L, "biliwn miliwn ac un")]
    [InlineData(2000000L, "dwy filiwn")]
    [InlineData(2000000000L, "dau filiwn")]
    [InlineData(2000000000000L, "dau driliwn")]
    [InlineData(3000000000000L, "tri thriliwn")]
    [InlineData(6000000000000L, "chwe thriliwn")]
    [InlineData(2000000000000000L, "dau gwadriliwn")]
    [InlineData(3000000000000000L, "tri chwadriliwn")]
    [InlineData(6000000000000000L, "chwe chwadriliwn")]
    [InlineData(2000000000000000000L, "dau gwintiliwn")]
    [InlineData(3000000000000000000L, "tri chwintiliwn")]
    [InlineData(6000000000000000000L, "chwe chwintiliwn")]
    public void WordsToNumber_RoundTripsWelshCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Cy));
        Assert.True(words.TryToNumber(out var parsed, Cy, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("cyntaf", 1)]
    [InlineData("unfed ar hugain", 21)]
    [InlineData("minws cyntaf", -1)]
    public void WordsToNumber_ParsesWelshOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Cy));
    }

    [Theory]
    [InlineData(1, "1af")]
    [InlineData(2, "2il")]
    [InlineData(3, "3ydd")]
    [InlineData(11, "11af")]
    [InlineData(20, "20fed")]
    public void Ordinalize_UsesWelshNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Cy));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(Cy));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 Ionawr 2022")]
    [InlineData(2015, 2, 3, "3 Chwefror 2015")]
    public void DateTimeToOrdinalWords_UsesWelshDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Ionawr 2022")]
    [InlineData(2015, 2, 3, "3 Chwefror 2015")]
    public void DateOnlyToOrdinalWords_UsesWelshDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "un o’r gloch y bore")]
    [InlineData(1, 5, "pump munud wedi un y bore")]
    [InlineData(13, 23, "dau ddeg tri munud wedi un y prynhawn")]
    [InlineData(20, 0, "wyth o’r gloch yr hwyr")]
    [InlineData(23, 40, "pedwar deg munud wedi un deg un y nos")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("dau ddeg pump munud wedi un y prynhawn", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "gogledd")]
    [InlineData(45.0, "gogledd-ddwyrain")]
    [InlineData(90.0, "dwyrain")]
    [InlineData(135.0, "de-ddwyrain")]
    [InlineData(180.0, "de")]
    [InlineData(225.0, "de-orllewin")]
    [InlineData(270.0, "gorllewin")]
    [InlineData(315.0, "gogledd-orllewin")]
    public void FullDirections_UseWelshNames(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "G")]
    [InlineData(45.0, "GDd")]
    [InlineData(90.0, "D")]
    [InlineData(112.5, "DDeDd")]
    [InlineData(135.0, "DeDd")]
    [InlineData(157.5, "DeDeDd")]
    [InlineData(180.0, "De")]
    [InlineData(270.0, "Go")]
    public void AbbreviatedDirections_UseWelshLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}