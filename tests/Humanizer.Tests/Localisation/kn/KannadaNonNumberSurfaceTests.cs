namespace Humanizer.Tests.Localisation.kn;

[UseCulture("kn")]
public class KannadaNonNumberSurfaceTests
{
    static readonly CultureInfo Kn = new("kn");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesKannadaConjunction()
    {
        Assert.Equal("1 ಮತ್ತು 2", Pair.Humanize());
        Assert.Equal("1, 2 ಮತ್ತು 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ನಿನ್ನೆ")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ನಾಳೆ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ದಿನಗಳ ಹಿಂದೆ")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 ದಿನಗಳಲ್ಲಿ")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ಒಂದು ಗಂಟೆ ಹಿಂದೆ")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 ಗಂಟೆಗಳಲ್ಲಿ")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ಈಗ")]
    public void RelativeDatePhrases_UseKannadaOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kn);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesKannadaNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ಎಂದಿಗೂ ಇಲ್ಲ", date.Humanize(culture: Kn));
    }

    [Theory]
    [InlineData(1, "1 ದಿನ")]
    [InlineData(2, "2 ದಿನಗಳು")]
    public void DurationPhrases_UseKannadaOutput(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: Kn));
    }

    [Fact]
    public void DurationToWords_UsesKannadaSingleUnitPhrase()
    {
        Assert.Equal("ಒಂದು ದಿನ", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Kn));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "ಮಿಲಿಸೆಕೆಂಡ್")]
    [InlineData(TimeUnit.Second, "ಸೆಕೆಂಡ್")]
    [InlineData(TimeUnit.Minute, "ನಿಮಿಷ")]
    [InlineData(TimeUnit.Hour, "ಗಂಟೆ")]
    [InlineData(TimeUnit.Day, "ದಿನ")]
    [InlineData(TimeUnit.Week, "ವಾರ")]
    [InlineData(TimeUnit.Month, "ತಿಂಗಳು")]
    [InlineData(TimeUnit.Year, "ವರ್ಷ")]
    public void TimeUnitSymbols_UseKannadaLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kn);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "ಬಿಟ್")]
    [InlineData(DataUnit.Byte, 1, "ಬೈಟ್")]
    [InlineData(DataUnit.Byte, 2, "ಬೈಟ್ಗಳು")]
    [InlineData(DataUnit.Kilobyte, 2, "ಕಿಲೋಬೈಟ್ಗಳು")]
    public void DataUnitWords_UseKannadaLabels(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kn);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 ಜನವರಿ 2022")]
    [InlineData(2015, 2, 3, "3 ಫೆಬ್ರವರಿ 2015")]
    [InlineData(2024, 12, 31, "31 ಡಿಸೆಂಬರ್ 2024")]
    public void DateTimeToOrdinalWords_UsesKannadaMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ಜನವರಿ 2022")]
    [InlineData(2015, 2, 3, "3 ಫೆಬ್ರವರಿ 2015")]
    public void DateOnlyToOrdinalWords_UsesKannadaMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "ಬೆಳಿಗ್ಗೆ ಒಂದು ಗಂಟೆ ಐದು ನಿಮಿಷಗಳು")]
    [InlineData(9, 0, "ಬೆಳಿಗ್ಗೆ ಒಂಬತ್ತು ಗಂಟೆ")]
    [InlineData(12, 0, "ಮಧ್ಯಾಹ್ನ ಹನ್ನೆರಡು ಗಂಟೆ")]
    [InlineData(13, 23, "ಮಧ್ಯಾಹ್ನ ಒಂದು ಗಂಟೆ ಇಪ್ಪತ್ತಮೂರು ನಿಮಿಷಗಳು")]
    [InlineData(18, 0, "ಸಂಜೆ ಆರು ಗಂಟೆ")]
    [InlineData(21, 0, "ರಾತ್ರಿ ಒಂಬತ್ತು ಗಂಟೆ")]
    public void ClockNotation_UsesKannadaPhrases(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_UsesKannadaMinuteWords()
    {
        Assert.Equal("ಮಧ್ಯಾಹ್ನ ಒಂದು ಗಂಟೆ ಇಪ್ಪತ್ತೈದು ನಿಮಿಷಗಳು", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ಉತ್ತರ")]
    [InlineData(45.0, "ಈಶಾನ್ಯ")]
    [InlineData(90.0, "ಪೂರ್ವ")]
    [InlineData(135.0, "ಆಗ್ನೇಯ")]
    [InlineData(180.0, "ದಕ್ಷಿಣ")]
    [InlineData(225.0, "ನೈರುತ್ಯ")]
    [InlineData(270.0, "ಪಶ್ಚಿಮ")]
    [InlineData(315.0, "ವಾಯುವ್ಯ")]
    public void CompassDirections_UseKannadaLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Kn));
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated, Kn));
    }

    [Theory]
    [InlineData(1, "1ನೇ")]
    [InlineData(21, "21ನೇ")]
    public void NumericOrdinalizer_UsesKannadaSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Kn));
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesStableKannadaSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize(Kn));
    }
}