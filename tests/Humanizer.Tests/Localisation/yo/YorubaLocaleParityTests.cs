namespace Humanizer.Tests.Localisation.yo;

[UseCulture("yo")]
public class YorubaLocaleParityTests
{
    static readonly CultureInfo Yo = new("yo");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesYorubaConjunction()
    {
        Assert.Equal("1 àti 2", Pair.Humanize());
        Assert.Equal("1, 2 àti 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "àná")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ọ̀la")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ọjọ́ sẹ́yìn")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "ní ọjọ́ 2")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ní báyìí")]
    public void DateHumanize_UsesYorubaPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Yo);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesYorubaNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("rárá", date.Humanize(culture: Yo));
    }

    [Theory]
    [InlineData(TimeUnit.Day, 1, false, "ọjọ́ 1")]
    [InlineData(TimeUnit.Day, 2, false, "ọjọ́ 2")]
    [InlineData(TimeUnit.Day, 1, true, "ọjọ́ kan")]
    [InlineData(TimeUnit.Month, 2, false, "oṣù 2")]
    public void TimeSpanHumanize_UsesYorubaDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Yo);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void ToAge_UsesYorubaTemplate()
    {
        Assert.Equal("ọdún 1", TimeSpan.FromDays(366).ToAge(Yo));
    }

    [Theory]
    [InlineData(DataUnit.Byte, "báìtì")]
    [InlineData(DataUnit.Kilobyte, "kílóbáìtì")]
    [InlineData(DataUnit.Megabyte, "mẹ́gábáìtì")]
    public void DataUnitHumanize_UsesYorubaNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Yo);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "s")]
    [InlineData(TimeUnit.Minute, "min")]
    [InlineData(TimeUnit.Hour, "h")]
    [InlineData(TimeUnit.Day, "ọj")]
    public void TimeUnitHumanize_UsesYorubaLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Yo);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "odo")]
    [InlineData(1, "ọ̀kan")]
    [InlineData(5, "márùn-ún")]
    [InlineData(11, "mọ́kànlá")]
    [InlineData(21, "ọ̀kanlélógún")]
    [InlineData(99, "ọ̀kàndílọ́gọ́rùn-ún")]
    [InlineData(105, "ọgọ́rùn-ún lé márùn-ún")]
    [InlineData(1234, "ẹgbẹ̀rún kan igba lé mẹ́rìnlélọ́gbọ̀n")]
    public void NumberToWords_ProducesExpectedYorubaOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Yo));
    }

    [Theory]
    [InlineData(-5, "àìyọkúrò márùn-ún")]
    [InlineData(-1000, "àìyọkúrò ẹgbẹ̀rún kan")]
    public void NumberToWords_UsesYorubaNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Yo));
    }

    [Theory]
    [InlineData(1, "àkọ́kọ́")]
    [InlineData(2, "kejì")]
    [InlineData(11, "kọ́kànlá")]
    [InlineData(21, "kọ̀kanlélógún")]
    [InlineData(-1, "àìyọkúrò àkọ́kọ́")]
    public void NumberToOrdinalWords_ProducesExpectedYorubaOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Yo));
    }

    [Theory]
    [InlineData(21, "ọ̀kanlélógún")]
    [InlineData(99, "ọ̀kàndílọ́gọ́rùn-ún")]
    [InlineData(105, "ọgọ́rùn-ún lé márùn-ún")]
    [InlineData(1234, "ẹgbẹ̀rún kan igba lé mẹ́rìnlélọ́gbọ̀n")]
    [InlineData(123000000, "mílíọ̀nù ọgọ́rùn-ún lé mẹ́tàlélógún")]
    [InlineData(4_325_010_007_018, "tírílíọ̀nù mẹ́rin bílíọ̀nù ọ̀ọ́dúnrún lé márùndínlọ́gbọ̀n mílíọ̀nù mẹ́wàá ẹgbẹ̀rún méje lé méjìdínlógún")]
    [InlineData(999_999, "ẹgbẹ̀rún ẹ̀ẹ́dẹ́gbẹ̀rún lé ọ̀kàndílọ́gọ́rùn-ún ẹ̀ẹ́dẹ́gbẹ̀rún lé ọ̀kàndílọ́gọ́rùn-ún")]
    public void WordsToNumber_RoundTripsYorubaCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Yo));
        Assert.True(words.TryToNumber(out var parsed, Yo, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("àkọ́kọ́", 1)]
    [InlineData("kọ́kànlá", 11)]
    [InlineData("kọ̀kanlélógún", 21)]
    [InlineData("àìyọkúrò àkọ́kọ́", -1)]
    public void WordsToNumber_ParsesYorubaOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Yo));
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(21, "21")]
    public void Ordinalize_UsesYorubaNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 Oṣù Ṣẹ́rẹ́ 2022")]
    [InlineData(2015, 2, 3, "3 Oṣù Èrèlè 2015")]
    public void DateTimeToOrdinalWords_UsesYorubaDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 Oṣù Ṣẹ́rẹ́ 2022")]
    [InlineData(2015, 2, 3, "3 Oṣù Èrèlè 2015")]
    public void DateOnlyToOrdinalWords_UsesYorubaDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "aago kan àárọ̀")]
    [InlineData(1, 5, "aago kan kọjá ìṣẹ́jú márùn-ún àárọ̀")]
    [InlineData(13, 23, "aago kan kọjá ìṣẹ́jú mẹ́tàlélógún ọ̀sán")]
    [InlineData(20, 0, "aago mẹ́jọ ìrọ̀lẹ́")]
    [InlineData(23, 40, "aago mọ́kànlá kọjá ìṣẹ́jú ogójì alẹ́")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("aago kan kọjá ìṣẹ́jú márùndínlọ́gbọ̀n ọ̀sán", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "àríwá")]
    [InlineData(45.0, "àríwá-ìlà-oòrùn")]
    [InlineData(90.0, "ìlà-oòrùn")]
    [InlineData(135.0, "gúúsù-ìlà-oòrùn")]
    [InlineData(180.0, "gúúsù")]
    [InlineData(225.0, "gúúsù-ìwọ̀-oòrùn")]
    [InlineData(270.0, "ìwọ̀-oòrùn")]
    [InlineData(315.0, "àríwá-ìwọ̀-oòrùn")]
    public void FullDirections_UseYorubaNames(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "A")]
    [InlineData(45.0, "AÌ")]
    [InlineData(90.0, "Ì")]
    [InlineData(180.0, "G")]
    [InlineData(270.0, "Ìw")]
    public void AbbreviatedDirections_UseYorubaLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}