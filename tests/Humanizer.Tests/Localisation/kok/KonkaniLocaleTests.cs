namespace Humanizer.Tests.Localisation.kok;

[UseCulture("kok")]
public class KonkaniLocaleTests
{
    static readonly CultureInfo Kok = new("kok");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesKonkaniConjunction()
    {
        Assert.Equal("1 आनी 2", Pair.Humanize());
        Assert.Equal("1, 2 आनी 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "एक सेकंद आदीं")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 सेकंद उपरांत")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "एक वर आदीं")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 वरां उपरांत")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "काल")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "फाल्यां")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 दीस आदीं")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 दीस उपरांत")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "आतां")]
    public void DateHumanize_UsesKonkaniPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kok);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesKonkaniNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("केन्नाच ना", date.Humanize(culture: Kok));
    }

    [Theory]
    [InlineData(1, "1 मिनिट")]
    [InlineData(2, "2 मिनिटां")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(culture: Kok));

    [Theory]
    [InlineData(1, "1 वर")]
    [InlineData(2, "2 वरां")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(culture: Kok));

    [Fact]
    public void MultiPart_UsesKonkaniCollectionFormatter()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: Kok, collectionSeparator: null);
        Assert.Equal("2 सप्तकां, 1 दीस आनी 1 वर", result);
    }

    [Fact]
    public void ToWords_UsesKonkaniWords()
    {
        Assert.Equal("एक वर", TimeSpan.FromHours(1).Humanize(toWords: true, culture: Kok));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "बिट")]
    [InlineData(DataUnit.Byte, 1, "बाइट")]
    [InlineData(DataUnit.Byte, 2, "बाइट")]
    [InlineData(DataUnit.Kilobyte, 2, "किलोबाइट")]
    [InlineData(DataUnit.Megabyte, 2, "मेगाबाइट")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kok);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "मिलिसेकंद")]
    [InlineData(TimeUnit.Second, "सेकंद")]
    [InlineData(TimeUnit.Minute, "मिनिट")]
    [InlineData(TimeUnit.Hour, "वर")]
    [InlineData(TimeUnit.Day, "दीस")]
    [InlineData(TimeUnit.Week, "सप्तक")]
    [InlineData(TimeUnit.Month, "म्हयनो")]
    [InlineData(TimeUnit.Year, "वर्स")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Kok);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "शून्य")]
    [InlineData(5, "पांच")]
    [InlineData(21, "एकवीस")]
    [InlineData(79, "एकोणअंयशीं")]
    [InlineData(100, "शंबर")]
    [InlineData(101, "शंबर एक")]
    [InlineData(999, "णवशें णव्याण्णव")]
    [InlineData(1000, "एक हजार")]
    [InlineData(1234567, "बारा लाख चवतीस हजार पांचशें सातसठ")]
    [InlineData(10000000, "एक कोटी")]
    [InlineData(12345678, "एक कोटी तेवीस लाख पंचेचाळीस हजार सशें अठ्ठयात्तर")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Kok));
    }

    [Theory]
    [InlineData(-5, "ऋणात्मक पांच")]
    [InlineData(-100000, "ऋणात्मक एक लाख")]
    public void NumberToWords_UsesKonkaniNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Kok));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "पयलो")]
    [InlineData(5, GrammaticalGender.Masculine, "पांचवो")]
    [InlineData(19, GrammaticalGender.Masculine, "एकोणीसावो")]
    [InlineData(21, GrammaticalGender.Masculine, "एकविसावो")]
    [InlineData(30, GrammaticalGender.Feminine, "तिसावी")]
    [InlineData(32, GrammaticalGender.Feminine, "बत्तीसावी")]
    [InlineData(21, GrammaticalGender.Neuter, "एकविसावें")]
    public void ToOrdinalWords_UsesKonkaniGenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Kok));
    }

    [Theory]
    [InlineData("एकवीस", 21)]
    [InlineData("एक कोटी तेवीस लाख पंचेचाळीस हजार सशें अठ्ठयात्तर", 12345678)]
    [InlineData("मायनस पांच", -5)]
    [InlineData("एकविसावो", 21)]
    [InlineData("एकविसावी", 21)]
    public void WordsToNumber_ParsesKonkaniWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Kok));
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
    [InlineData(1, 0, "पहाटे एक वरां")]
    [InlineData(1, 5, "पहाटे एक वर जावन पांच मिनिटां")]
    [InlineData(12, 0, "दनपारां बारा वरां")]
    [InlineData(13, 23, "दनपारां एक वर जावन तेवीस मिनिटां")]
    [InlineData(18, 0, "सांजे स वरां")]
    [InlineData(21, 0, "रातीं णव वरां")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("दनपारां एक वर जावन पंचवीस मिनिटां", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
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
    public void NumberFormatting_UsesStableKonkaniSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}