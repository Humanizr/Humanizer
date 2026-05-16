namespace Humanizer.Tests.Localisation.@as;

[UseCulture("as")]
public class AssameseLocaleParityTests
{
    static readonly CultureInfo As = new("as");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void CollectionHumanize_UsesAssameseConjunction()
    {
        Assert.Equal("1 আৰু 2", Pair.Humanize());
        Assert.Equal("1, 2 আৰু 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "কালি")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "কাইলৈ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 দিন আগতে")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 দিন পাছত")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "এক ঘণ্টা আগতে")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 ঘণ্টা পাছত")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "এতিয়া")]
    public void RelativeDatePhrases_UseAssameseOutput(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(As);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesAssameseNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("কেতিয়াও নহয়", date.Humanize(culture: As));
    }

    [Theory]
    [InlineData(1, "এক দিন")]
    [InlineData(2, "2 দিন")]
    public void DurationPhrases_UseAssameseOutput(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: As));
    }

    [Fact]
    public void DurationToWords_UsesAssameseSingleUnitPhrase()
    {
        Assert.Equal("এক দিন", TimeSpan.FromDays(1).Humanize(toWords: true, culture: As));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "মিলিছেকেণ্ড")]
    [InlineData(TimeUnit.Second, "ছেকেণ্ড")]
    [InlineData(TimeUnit.Minute, "মিনিট")]
    [InlineData(TimeUnit.Hour, "ঘণ্টা")]
    [InlineData(TimeUnit.Day, "দিন")]
    [InlineData(TimeUnit.Week, "সপ্তাহ")]
    [InlineData(TimeUnit.Month, "মাহ")]
    [InlineData(TimeUnit.Year, "বছৰ")]
    public void TimeUnitSymbols_UseAssameseLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(As);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "বিট")]
    [InlineData(DataUnit.Byte, 1, "বাইট")]
    [InlineData(DataUnit.Byte, 2, "বাইট")]
    [InlineData(DataUnit.Kilobyte, 2, "কিলোবাইট")]
    public void DataUnitWords_UseAssameseLabels(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(As);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 জানুৱাৰী 2022")]
    [InlineData(2015, 2, 3, "3 ফেব্ৰুৱাৰী 2015")]
    [InlineData(2024, 12, 31, "31 ডিচেম্বৰ 2024")]
    public void DateTimeToOrdinalWords_UsesAssameseMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 জানুৱাৰী 2022")]
    [InlineData(2015, 2, 3, "3 ফেব্ৰুৱাৰী 2015")]
    public void DateOnlyToOrdinalWords_UsesAssameseMonthNames(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "ৰাতি এটা বাজি পাঁচ মিনিট")]
    [InlineData(12, 0, "দুপৰীয়া বাৰ বজা")]
    [InlineData(13, 23, "দুপৰীয়া এটা বাজি তেইছ মিনিট")]
    [InlineData(18, 0, "দুপৰীয়া ছয় বজা")]
    [InlineData(21, 0, "ৰাতি ন বজা")]
    public void ClockNotation_UsesAssamesePhrases(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_UsesAssameseMinuteWords()
    {
        Assert.Equal("দুপৰীয়া এটা বাজি পঁচিশ মিনিট", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "উত্তৰ")]
    [InlineData(45.0, "উত্তৰ-পূৱ")]
    [InlineData(90.0, "পূৱ")]
    [InlineData(135.0, "দক্ষিণ-পূৱ")]
    [InlineData(180.0, "দক্ষিণ")]
    [InlineData(225.0, "দক্ষিণ-পশ্চিম")]
    [InlineData(270.0, "পশ্চিম")]
    [InlineData(315.0, "উত্তৰ-পশ্চিম")]
    public void CompassDirections_UseAssameseLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full, As));
    }

    [Theory]
    [InlineData(0.0, "উ")]
    [InlineData(45.0, "উ-পূ")]
    [InlineData(90.0, "পূ")]
    [InlineData(180.0, "দ")]
    [InlineData(270.0, "প")]
    public void CompassAbbreviations_UseAssameseLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated, As));
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesAssameseSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize(As));
    }
}