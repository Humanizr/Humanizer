namespace Humanizer.Tests.Localisation.gu;

[UseCulture("gu")]
public class GujaratiNonNumberSurfaceTests
{
    static readonly CultureInfo Gu = new("gu");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesGujaratiConjunction()
    {
        Assert.Equal("1 અને 2", Pair.Humanize());
        Assert.Equal("1, 2 અને 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ગઈકાલે")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "આવતીકાલે")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 દિવસ પહેલાં")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 દિવસમાં")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "એક કલાક પહેલાં")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 કલાકમાં")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "હમણાં")]
    public void RelativeDatePhrases_UseGujaratiOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Gu);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesGujaratiNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ક્યારેય નહીં", date.Humanize(culture: Gu));
    }

    [Theory]
    [InlineData(1, "1 દિવસ")]
    [InlineData(2, "2 દિવસ")]
    public void DurationPhrases_UseGujaratiOutput(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: Gu));
    }

    [Fact]
    public void DurationToWords_UsesGujaratiSingleUnitPhrase()
    {
        Assert.Equal("એક દિવસ", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Gu));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "મિલિસેકંડ")]
    [InlineData(TimeUnit.Second, "સેકંડ")]
    [InlineData(TimeUnit.Minute, "મિનિટ")]
    [InlineData(TimeUnit.Hour, "કલાક")]
    [InlineData(TimeUnit.Day, "દિવસ")]
    [InlineData(TimeUnit.Week, "અઠવાડિયું")]
    [InlineData(TimeUnit.Month, "મહિનો")]
    [InlineData(TimeUnit.Year, "વર્ષ")]
    public void TimeUnitSymbols_UseGujaratiLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Gu);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "બિટ")]
    [InlineData(DataUnit.Byte, 1, "બાઇટ")]
    [InlineData(DataUnit.Byte, 2, "બાઇટ")]
    [InlineData(DataUnit.Kilobyte, 2, "કિલોબાઇટ")]
    public void DataUnitWords_UseGujaratiLabels(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Gu);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 જાન્યુઆરી, 2022")]
    [InlineData(2015, 2, 3, "3 ફેબ્રુઆરી, 2015")]
    [InlineData(2024, 12, 31, "31 ડિસેમ્બર, 2024")]
    public void DateTimeToOrdinalWords_UsesGujaratiMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 જાન્યુઆરી, 2022")]
    [InlineData(2015, 2, 3, "3 ફેબ્રુઆરી, 2015")]
    public void DateOnlyToOrdinalWords_UsesGujaratiMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "સવારે એક વાગીને પાંચ મિનિટ")]
    [InlineData(12, 0, "બપોરે બાર વાગ્યે")]
    [InlineData(13, 23, "બપોરે એક વાગીને તેવીસ મિનિટ")]
    [InlineData(18, 0, "સાંજે છ વાગ્યે")]
    [InlineData(21, 0, "રાત્રે નવ વાગ્યે")]
    public void ClockNotation_UsesGujaratiPhrases(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_UsesGujaratiMinuteWords()
    {
        Assert.Equal("બપોરે એક વાગીને પચ્ચીસ મિનિટ", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ઉત્તર")]
    [InlineData(45.0, "ઉત્તર-પૂર્વ")]
    [InlineData(90.0, "પૂર્વ")]
    [InlineData(135.0, "દક્ષિણ-પૂર્વ")]
    [InlineData(180.0, "દક્ષિણ")]
    [InlineData(225.0, "દક્ષિણ-પશ્ચિમ")]
    [InlineData(270.0, "પશ્ચિમ")]
    [InlineData(315.0, "ઉત્તર-પશ્ચિમ")]
    public void CompassDirections_UseGujaratiLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Gu));
    }

    [Theory]
    [InlineData(0.0, "ઉ")]
    [InlineData(45.0, "ઉ-પૂ")]
    [InlineData(90.0, "પૂ")]
    [InlineData(180.0, "દ")]
    [InlineData(270.0, "પ")]
    public void CompassAbbreviations_UseGujaratiLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated, Gu));
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesGujaratiSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize(Gu));
    }
}