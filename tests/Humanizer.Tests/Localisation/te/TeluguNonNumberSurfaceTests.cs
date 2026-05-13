namespace Humanizer.Tests.Localisation.te;

[UseCulture("te")]
public class TeluguNonNumberSurfaceTests
{
    static readonly CultureInfo Te = new("te");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesTeluguConjunction()
    {
        Assert.Equal("1 మరియు 2", Pair.Humanize());
        Assert.Equal("1, 2 మరియు 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "నిన్న")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "రేపు")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 రోజులు క్రితం")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 రోజులు తర్వాత")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ఒక గంట క్రితం")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 గంటలు తర్వాత")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ఇప్పుడే")]
    public void RelativeDatePhrases_UseTeluguOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Te);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesTeluguNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ఎప్పుడూ లేదు", date.Humanize(culture: Te));
    }

    [Theory]
    [InlineData(1, "1 రోజు")]
    [InlineData(2, "2 రోజులు")]
    public void DurationPhrases_UseTeluguOutput(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: Te));
    }

    [Fact]
    public void DurationToWords_UsesTeluguSingleUnitPhrase()
    {
        Assert.Equal("ఒక రోజు", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Te));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "మిల్లీసెకను")]
    [InlineData(TimeUnit.Second, "సెకను")]
    [InlineData(TimeUnit.Minute, "నిమిషం")]
    [InlineData(TimeUnit.Hour, "గంట")]
    [InlineData(TimeUnit.Day, "రోజు")]
    [InlineData(TimeUnit.Week, "వారం")]
    [InlineData(TimeUnit.Month, "నెల")]
    [InlineData(TimeUnit.Year, "సంవత్సరం")]
    public void TimeUnitSymbols_UseTeluguLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Te);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "బిట్")]
    [InlineData(DataUnit.Byte, 1, "బైట్")]
    [InlineData(DataUnit.Byte, 2, "బైట్లు")]
    [InlineData(DataUnit.Kilobyte, 2, "కిలోబైట్లు")]
    public void DataUnitWords_UseTeluguLabels(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Te);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 జనవరి 2022")]
    [InlineData(2015, 2, 3, "3 ఫిబ్రవరి 2015")]
    [InlineData(2024, 12, 31, "31 డిసెంబర్ 2024")]
    public void DateTimeToOrdinalWords_UsesTeluguMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 జనవరి 2022")]
    [InlineData(2015, 2, 3, "3 ఫిబ్రవరి 2015")]
    public void DateOnlyToOrdinalWords_UsesTeluguMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "ఒంటి గంట ఐదు నిమిషాలు ఉదయం")]
    [InlineData(9, 0, "తొమ్మిది గంటలు ఉదయం")]
    [InlineData(12, 0, "పన్నెండు గంటలు మధ్యాహ్నం")]
    [InlineData(13, 23, "ఒంటి గంట ఇరవై మూడు నిమిషాలు మధ్యాహ్నం")]
    [InlineData(17, 0, "ఐదు గంటలు మధ్యాహ్నం")]
    [InlineData(18, 0, "ఆరు గంటలు సాయంత్రం")]
    [InlineData(21, 0, "తొమ్మిది గంటలు రాత్రి")]
    public void ClockNotation_UsesTeluguPhrases(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_UsesTeluguMinuteWords()
    {
        Assert.Equal("ఒంటి గంట ఇరవై ఐదు నిమిషాలు మధ్యాహ్నం", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ఉత్తరం")]
    [InlineData(45.0, "ఈశాన్యం")]
    [InlineData(90.0, "తూర్పు")]
    [InlineData(135.0, "ఆగ్నేయం")]
    [InlineData(180.0, "దక్షిణం")]
    [InlineData(225.0, "నైరుతి")]
    [InlineData(270.0, "పడమర")]
    [InlineData(315.0, "వాయవ్యం")]
    public void CompassDirections_UseTeluguLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Te));
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated, Te));
    }

    [Theory]
    [InlineData(1, "1వ")]
    [InlineData(21, "21వ")]
    public void NumericOrdinalizer_UsesTeluguSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Te));
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesStableTeluguSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize(Te));
    }
}