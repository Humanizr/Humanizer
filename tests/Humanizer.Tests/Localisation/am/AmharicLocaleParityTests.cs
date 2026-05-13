namespace Humanizer.Tests.Localisation.am;

[UseCulture("am")]
public class AmharicLocaleParityTests
{
    static readonly CultureInfo Am = new("am");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesAmharicConjunction()
    {
        Assert.Equal("1 እና 2", Pair.Humanize());
        Assert.Equal("1, 2 እና 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ትናንት")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ነገ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "ከ 2 ቀናት በፊት")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "ከ 2 ቀናት በኋላ")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ከ 1 ሰዓት በፊት")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "ከ 2 ሰዓታት በኋላ")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "አሁን")]
    public void DateHumanize_UsesAmharicPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Am);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesAmharicNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ፈጽሞ", date.Humanize(culture: Am));
    }

    [Theory]
    [InlineData(1, "1 ሰዓት")]
    [InlineData(2, "2 ሰዓታት")]
    public void Duration_UsesAmharicUnitForms(int hours, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(culture: Am));
    }

    [Fact]
    public void Duration_ToWordsUsesAmharicNumberWords()
    {
        Assert.Equal("አንድ ቀን", TimeSpan.FromDays(1).Humanize(toWords: true, culture: Am));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "ቢት")]
    [InlineData(DataUnit.Byte, 1, "ባይት")]
    [InlineData(DataUnit.Byte, 2, "ባይቶች")]
    [InlineData(DataUnit.Kilobyte, 2, "ኪሎባይቶች")]
    [InlineData(DataUnit.Megabyte, 2, "ሜጋባይቶች")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Am);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "ሚሊሰከንድ")]
    [InlineData(TimeUnit.Second, "ሰከንድ")]
    [InlineData(TimeUnit.Minute, "ደቂቃ")]
    [InlineData(TimeUnit.Hour, "ሰዓት")]
    [InlineData(TimeUnit.Day, "ቀን")]
    [InlineData(TimeUnit.Week, "ሳምንት")]
    [InlineData(TimeUnit.Month, "ወር")]
    [InlineData(TimeUnit.Year, "ዓመት")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Am);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "ዜሮ")]
    [InlineData(21, "ሃያ አንድ")]
    [InlineData(1234567, "አንድ ሚሊዮን ሁለት መቶ ሰላሳ አራት ሺህ አምስት መቶ ስልሳ ሰባት")]
    [InlineData(1234567890, "አንድ ቢሊዮን ሁለት መቶ ሰላሳ አራት ሚሊዮን አምስት መቶ ስልሳ ሰባት ሺህ ስምንት መቶ ዘጠና")]
    public void NumberToWords_ProducesAmharicCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Am));
    }

    [Theory]
    [InlineData(1, "አንደኛ")]
    [InlineData(21, "ሃያ አንደኛ")]
    [InlineData(1000, "አንድ ሺኛ")]
    public void ToOrdinalWords_ProducesAmharicOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Am));
    }

    [Theory]
    [InlineData("ሃያ አንድ", 21)]
    [InlineData("አንድ መቶ አምስት", 105)]
    [InlineData("አንድ ሺህ አንድ", 1001)]
    [InlineData("ሃያ አንደኛ", 21)]
    [InlineData("1ኛ", 1)]
    [InlineData("23ኛ", 23)]
    [InlineData("-1ኛ", -1)]
    [InlineData("አንድ መቶኛ", 100)]
    [InlineData("ሁለት መቶኛ", 200)]
    [InlineData("አንድ ኳድሪሊዮንኛ", 1_000_000_000_000_000)]
    [InlineData("አንድ ኩዊንቲሊዮንኛ", 1_000_000_000_000_000_000)]
    public void WordsToNumber_ParsesAmharicCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Am));
    }

    [Theory]
    [InlineData(1, "1ኛ")]
    [InlineData(23, "23ኛ")]
    [InlineData(-1, "-1ኛ")]
    public void Ordinalize_UsesAmharicNumericSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Am));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 ጃንዋሪ 2022")]
    [InlineData(2015, 2, 3, "3 ፌብሩዋሪ 2015")]
    [InlineData(2024, 12, 31, "31 ዲሴምበር 2024")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ጃንዋሪ 2022")]
    [InlineData(2015, 2, 3, "3 ፌብሩዋሪ 2015")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "ከሌሊቱ ሰባት ሰዓት አምስት ደቂቃ")]
    [InlineData(7, 0, "ከጠዋቱ አንድ ሰዓት")]
    [InlineData(13, 0, "ከቀኑ ሰባት ሰዓት")]
    [InlineData(13, 23, "ከቀኑ ሰባት ሰዓት ሃያ ሶስት ደቂቃ")]
    [InlineData(18, 0, "ከምሽቱ አስራ ሁለት ሰዓት")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("ከቀኑ ሰባት ሰዓት ሃያ አምስት ደቂቃ", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ሰሜን")]
    [InlineData(45.0, "ሰሜን ምስራቅ")]
    [InlineData(90.0, "ምስራቅ")]
    [InlineData(180.0, "ደቡብ")]
    [InlineData(270.0, "ምዕራብ")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "ሰ")]
    [InlineData(45.0, "ሰም")]
    [InlineData(90.0, "ም")]
    [InlineData(180.0, "ደ")]
    [InlineData(270.0, "ምዕ")]
    public void Compass_AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}