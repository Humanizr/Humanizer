namespace Humanizer.Tests.Localisation.ml;

[UseCulture("ml")]
public class MalayalamNonNumberSurfaceTests
{
    static readonly CultureInfo Ml = new("ml");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesMalayalamDelimiter()
    {
        Assert.Equal("1, 2", Pair.Humanize());
        Assert.Equal("1, 2, 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ഇന്നലെ")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "നാളെ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ദിവസം മുമ്പ്")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 ദിവസത്തിന് ശേഷം")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ഒരു മണിക്കൂർ മുമ്പ്")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 മണിക്കൂറിന് ശേഷം")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ഇപ്പോൾ")]
    public void RelativeDatePhrases_UseMalayalamOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Ml);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesMalayalamNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ഒരിക്കലും", date.Humanize(culture: Ml));
    }

    [Theory]
    [InlineData(1, "1 ദിവസം")]
    [InlineData(2, "2 ദിവസം")]
    public void DurationPhrases_UseMalayalamOutput(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: Ml));
    }

    [Fact]
    public void DurationToWords_UsesMalayalamSingleUnitPhrase()
    {
        Assert.Equal("ഒരു ദിവസം", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Ml));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "മില്ലിസെക്കൻഡ്")]
    [InlineData(TimeUnit.Second, "സെക്കൻഡ്")]
    [InlineData(TimeUnit.Minute, "മിനിറ്റ്")]
    [InlineData(TimeUnit.Hour, "മണിക്കൂർ")]
    [InlineData(TimeUnit.Day, "ദിവസം")]
    [InlineData(TimeUnit.Week, "ആഴ്ച")]
    [InlineData(TimeUnit.Month, "മാസം")]
    [InlineData(TimeUnit.Year, "വർഷം")]
    public void TimeUnitSymbols_UseMalayalamLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Ml);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "ബിറ്റ്")]
    [InlineData(DataUnit.Byte, 1, "ബൈറ്റ്")]
    [InlineData(DataUnit.Byte, 2, "ബൈറ്റ്")]
    [InlineData(DataUnit.Kilobyte, 2, "കിലോബൈറ്റ്")]
    public void DataUnitWords_UseMalayalamLabels(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Ml);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 ജനുവരി 2022")]
    [InlineData(2015, 2, 3, "3 ഫെബ്രുവരി 2015")]
    [InlineData(2024, 12, 31, "31 ഡിസംബർ 2024")]
    public void DateTimeToOrdinalWords_UsesMalayalamMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ജനുവരി 2022")]
    [InlineData(2015, 2, 3, "3 ഫെബ്രുവരി 2015")]
    public void DateOnlyToOrdinalWords_UsesMalayalamMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "പുലർച്ചെ ഒരു മണി അഞ്ച് മിനിറ്റ്")]
    [InlineData(9, 0, "രാവിലെ ഒൻപത് മണി")]
    [InlineData(12, 0, "ഉച്ചയ്ക്ക് പന്ത്രണ്ട് മണി")]
    [InlineData(13, 23, "ഉച്ചയ്ക്ക് ഒരു മണി ഇരുപത്തിമൂന്ന് മിനിറ്റ്")]
    [InlineData(17, 0, "വൈകുന്നേരം അഞ്ച് മണി")]
    [InlineData(21, 0, "രാത്രി ഒൻപത് മണി")]
    public void ClockNotation_UsesMalayalamPhrases(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_UsesMalayalamMinuteWords()
    {
        Assert.Equal("ഉച്ചയ്ക്ക് ഒരു മണി ഇരുപത്തിയഞ്ച് മിനിറ്റ്", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "വടക്ക്")]
    [InlineData(45.0, "വടക്കുകിഴക്ക്")]
    [InlineData(90.0, "കിഴക്ക്")]
    [InlineData(135.0, "തെക്കുകിഴക്ക്")]
    [InlineData(180.0, "തെക്ക്")]
    [InlineData(225.0, "തെക്കുപടിഞ്ഞാറ്")]
    [InlineData(270.0, "പടിഞ്ഞാറ്")]
    [InlineData(315.0, "വടക്കുപടിഞ്ഞാറ്")]
    public void CompassDirections_UseMalayalamLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, Ml));
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated, Ml));
    }

    [Theory]
    [InlineData(1, "1-ാം")]
    [InlineData(21, "21-ാം")]
    public void NumericOrdinalizer_UsesMalayalamSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Ml));
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesStableMalayalamSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize(Ml));
    }
}