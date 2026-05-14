namespace Humanizer.Tests.Localisation.mr;

[UseCulture("mr")]
public class MarathiLocaleTests
{
    static readonly CultureInfo Mr = new("mr");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesMarathiConjunction()
    {
        Assert.Equal("1 आणि 2", Pair.Humanize());
        Assert.Equal("1, 2 आणि 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "एक सेकंद पूर्वी")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 सेकंद नंतर")]
    [InlineData(1, TimeUnit.Minute, Tense.Past, "एक मिनिट पूर्वी")]
    [InlineData(2, TimeUnit.Minute, Tense.Future, "2 मिनिटे नंतर")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "एक तास पूर्वी")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 तास नंतर")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "काल")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "उद्या")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 दिवस पूर्वी")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 दिवस नंतर")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "एक महिना पूर्वी")]
    [InlineData(2, TimeUnit.Month, Tense.Future, "2 महिने नंतर")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "एक वर्ष पूर्वी")]
    [InlineData(2, TimeUnit.Year, Tense.Future, "2 वर्षे नंतर")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "आत्ता")]
    public void DateHumanize_UsesMarathiPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Mr);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesMarathiNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("कधीच नाही", date.Humanize(culture: Mr));
    }

    [Theory]
    [InlineData(1, "1 मिलिसेकंद")]
    [InlineData(2, "2 मिलिसेकंद")]
    public void Milliseconds(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(culture: Mr));

    [Theory]
    [InlineData(1, "1 मिनिट")]
    [InlineData(2, "2 मिनिटे")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(culture: Mr));

    [Theory]
    [InlineData(1, "1 तास")]
    [InlineData(2, "2 तास")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(culture: Mr));

    [Fact]
    public void MultiPart_UsesMarathiCollectionFormatter()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: Mr, collectionSeparator: null);
        Assert.Equal("2 आठवडे, 1 दिवस आणि 1 तास", result);
    }

    [Fact]
    public void ToWords_UsesMarathiWords()
    {
        Assert.Equal("एक तास", TimeSpan.FromHours(1).Humanize(toWords: true, culture: Mr));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "बिट")]
    [InlineData(DataUnit.Byte, 1, "बाइट")]
    [InlineData(DataUnit.Byte, 2, "बाइट")]
    [InlineData(DataUnit.Kilobyte, 2, "किलोबाइट")]
    [InlineData(DataUnit.Megabyte, 2, "मेगाबाइट")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Mr);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "मिलिसेकंद")]
    [InlineData(TimeUnit.Second, "सेकंद")]
    [InlineData(TimeUnit.Minute, "मिनिट")]
    [InlineData(TimeUnit.Hour, "तास")]
    [InlineData(TimeUnit.Day, "दिवस")]
    [InlineData(TimeUnit.Week, "आठवडा")]
    [InlineData(TimeUnit.Month, "महिना")]
    [InlineData(TimeUnit.Year, "वर्ष")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Mr);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "शून्य")]
    [InlineData(5, "पाच")]
    [InlineData(21, "एकवीस")]
    [InlineData(79, "एकोणऐंशी")]
    [InlineData(100, "एकशे")]
    [InlineData(101, "एकशे एक")]
    [InlineData(999, "नऊशे नव्याण्णव")]
    [InlineData(1000, "एक हजार")]
    [InlineData(99999, "नव्याण्णव हजार नऊशे नव्याण्णव")]
    [InlineData(100000, "एक लाख")]
    [InlineData(1234567, "बारा लाख चौतीस हजार पाचशे सदुसष्ट")]
    [InlineData(10000000, "एक कोटी")]
    [InlineData(12345678, "एक कोटी तेवीस लाख पंचेचाळीस हजार सहाशे अठ्ठ्याहत्तर")]
    [InlineData(1000000000, "एक अब्ज")]
    [InlineData(100000000000, "एक खर्व")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Mr));
    }

    [Theory]
    [InlineData(-5, "ऋणात्मक पाच")]
    [InlineData(-100000, "ऋणात्मक एक लाख")]
    public void NumberToWords_UsesMarathiNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Mr));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "पहिला")]
    [InlineData(5, GrammaticalGender.Masculine, "पाचवा")]
    [InlineData(20, GrammaticalGender.Masculine, "विसावा")]
    [InlineData(21, GrammaticalGender.Masculine, "एकविसावा")]
    [InlineData(32, GrammaticalGender.Masculine, "बत्तीसावा")]
    [InlineData(100, GrammaticalGender.Masculine, "शंभरावा")]
    [InlineData(101, GrammaticalGender.Masculine, "एकशे एकवा")]
    [InlineData(1, GrammaticalGender.Feminine, "पहिली")]
    [InlineData(5, GrammaticalGender.Feminine, "पाचवी")]
    [InlineData(20, GrammaticalGender.Feminine, "विसावी")]
    [InlineData(21, GrammaticalGender.Feminine, "एकविसावी")]
    [InlineData(32, GrammaticalGender.Feminine, "बत्तीसावी")]
    [InlineData(100, GrammaticalGender.Feminine, "शंभरावी")]
    [InlineData(101, GrammaticalGender.Feminine, "एकशे एकवी")]
    [InlineData(1, GrammaticalGender.Neuter, "पहिले")]
    [InlineData(5, GrammaticalGender.Neuter, "पाचवे")]
    [InlineData(20, GrammaticalGender.Neuter, "विसावे")]
    [InlineData(21, GrammaticalGender.Neuter, "एकविसावे")]
    [InlineData(32, GrammaticalGender.Neuter, "बत्तीसावे")]
    [InlineData(100, GrammaticalGender.Neuter, "शंभरावे")]
    [InlineData(101, GrammaticalGender.Neuter, "एकशे एकवे")]
    public void ToOrdinalWords_UsesRealMarathiGenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Mr));
    }

    [Theory]
    [InlineData(20, GrammaticalGender.Masculine, "विसावा")]
    [InlineData(20, GrammaticalGender.Feminine, "विसावी")]
    [InlineData(20, GrammaticalGender.Neuter, "विसावे")]
    [InlineData(21, GrammaticalGender.Masculine, "एकविसावा")]
    [InlineData(21, GrammaticalGender.Feminine, "एकविसावी")]
    [InlineData(21, GrammaticalGender.Neuter, "एकविसावे")]
    [InlineData(32, GrammaticalGender.Masculine, "बत्तीसावा")]
    [InlineData(32, GrammaticalGender.Feminine, "बत्तीसावी")]
    [InlineData(32, GrammaticalGender.Neuter, "बत्तीसावे")]
    public void Ordinalize_Int_UsesRealMarathiGenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Mr));
    }

    [Theory]
    [InlineData("एकवीस", 21)]
    [InlineData("एक कोटी तेवीस लाख पंचेचाळीस हजार सहाशे अठ्ठ्याहत्तर", 12345678)]
    [InlineData("मायनस पाच", -5)]
    [InlineData("विसावा", 20)]
    [InlineData("विसावी", 20)]
    [InlineData("विसावे", 20)]
    [InlineData("बत्तीसावा", 32)]
    [InlineData("बत्तीसावी", 32)]
    [InlineData("बत्तीसावे", 32)]
    [InlineData("एकविसावी", 21)]
    [InlineData("एकविसावे", 21)]
    public void WordsToNumber_ParsesMarathiWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Mr));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 जानेवारी 2022")]
    [InlineData(2015, 1, 1, "1 जानेवारी 2015")]
    [InlineData(2015, 2, 3, "3 फेब्रुवारी 2015")]
    [InlineData(2021, 10, 31, "31 ऑक्टोबर 2021")]
    [InlineData(2024, 12, 31, "31 डिसेंबर 2024")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 जानेवारी 2022")]
    [InlineData(2015, 2, 3, "3 फेब्रुवारी 2015")]
    [InlineData(2021, 10, 31, "31 ऑक्टोबर 2021")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "पहाटे एक वाजले")]
    [InlineData(1, 5, "पहाटे एक वाजून पाच मिनिटे")]
    [InlineData(12, 0, "दुपारी बारा वाजले")]
    [InlineData(13, 23, "दुपारी एक वाजून तेवीस मिनिटे")]
    [InlineData(18, 0, "संध्याकाळी सहा वाजले")]
    [InlineData(21, 0, "रात्री नऊ वाजले")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("दुपारी एक वाजून पंचवीस मिनिटे", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "उत्तर")]
    [InlineData(45.0, "ईशान्य")]
    [InlineData(90.0, "पूर्व")]
    [InlineData(135.0, "आग्नेय")]
    [InlineData(180.0, "दक्षिण")]
    [InlineData(225.0, "नैऋत्य")]
    [InlineData(270.0, "पश्चिम")]
    [InlineData(315.0, "वायव्य")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "उ")]
    [InlineData(45.0, "ई")]
    [InlineData(90.0, "पू")]
    [InlineData(180.0, "द")]
    [InlineData(270.0, "प")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }

    [Fact]
    public void NumberFormatting_UsesStableMarathiSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}