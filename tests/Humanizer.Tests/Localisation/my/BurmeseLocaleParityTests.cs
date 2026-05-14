namespace Humanizer.Tests.Localisation.my;

[UseCulture("my")]
public class BurmeseLocaleParityTests
{
    static readonly CultureInfo My = new("my");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesBurmeseConjunction()
    {
        Assert.Equal("1 နှင့် 2", Pair.Humanize());
        Assert.Equal("1, 2 နှင့် 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ယခု")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "မနေ့က")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "ပြီးခဲ့သည့် 2 ရက်")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "မနက်ဖြန်")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 ရက်အတွင်း")]
    [InlineData(3, TimeUnit.Day, Tense.Past, "ပြီးခဲ့သည့် 3 ရက်")]
    [InlineData(3, TimeUnit.Day, Tense.Future, "3 ရက်အတွင်း")]
    public void DateHumanize_UsesBurmeseRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(My);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesBurmeseNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ဘယ်တော့မှ", date.Humanize(culture: My));
    }

    [Theory]
    [InlineData(TimeUnit.Minute, 1, "1 မိနစ်")]
    [InlineData(TimeUnit.Day, 2, "2 ရက်")]
    public void TimeSpanHumanize_UsesBurmeseDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(My);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ZeroUsesBurmesePhrases()
    {
        Assert.Equal("0 မီလီစက္ကန့်", TimeSpan.Zero.Humanize(culture: My));
        Assert.Equal("အချိန်မရှိ", TimeSpan.Zero.Humanize(culture: My, toWords: true));
    }

    [Fact]
    public void TimeSpanHumanize_ToWordsUsesBurmeseNumberWords()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(My);
        Assert.Equal("တစ် ရက်", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Theory]
    [InlineData(DataUnit.Bit, "ဘစ်")]
    [InlineData(DataUnit.Byte, "ဘိုက်")]
    [InlineData(DataUnit.Kilobyte, "ကီလိုဘိုက်")]
    [InlineData(DataUnit.Megabyte, "မီဂါဘိုက်")]
    public void DataUnitHumanize_UsesBurmeseNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(My);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void ByteSizeToFullWords_UsesBurmeseDataUnits()
    {
        Assert.Equal("1 ဘစ်", ByteSize.FromBits(1).ToFullWords(provider: My));
        Assert.Equal("1 ဘိုက်", ByteSize.FromBytes(1).ToFullWords(provider: My));
        Assert.Equal("2 ကီလိုဘိုက်", ByteSize.FromKilobytes(2).ToFullWords(provider: My));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "မီလီစက္ကန့်")]
    [InlineData(TimeUnit.Second, "စက္ကန့်")]
    [InlineData(TimeUnit.Minute, "မိနစ်")]
    [InlineData(TimeUnit.Hour, "နာရီ")]
    [InlineData(TimeUnit.Day, "ရက်")]
    [InlineData(TimeUnit.Week, "ပတ်")]
    [InlineData(TimeUnit.Month, "လ")]
    [InlineData(TimeUnit.Year, "နှစ်")]
    public void TimeUnitHumanize_UsesBurmeseUnitNames(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(My);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "သုည")]
    [InlineData(21, "နှစ်ဆယ့်တစ်")]
    [InlineData(105, "တစ်ရာ ငါး")]
    [InlineData(1234, "တစ်ထောင် နှစ်ရာ သုံးဆယ့်လေး")]
    [InlineData(12345678, "တစ်ကုဋေ နှစ်သန်း သုံးသိန်း လေးသောင်း ငါးထောင် ခြောက်ရာ ခုနစ်ဆယ့်ရှစ်")]
    [InlineData(-21, "အနုတ် နှစ်ဆယ့်တစ်")]
    public void NumberToWords_ProducesBurmeseCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(My));
    }

    [Fact]
    public void NumberToWords_MaximumValueRoundTripsThroughBurmeseParser()
    {
        const long number = 999_999_999;

        var words = number.ToWords(My);

        Assert.StartsWith("ကိုးဆယ့်ကိုးကုဋေ", words, StringComparison.Ordinal);
        Assert.Equal(number, words.ToNumber(My));
    }

    [Theory]
    [InlineData(1, "ပထမ")]
    [InlineData(2, "ဒုတိယ")]
    [InlineData(21, "နှစ်ဆယ့်တစ်မြောက်")]
    [InlineData(101, "တစ်ရာ တစ်မြောက်")]
    public void NumberToOrdinalWords_ProducesBurmeseOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(My));
    }



    [Theory]
    [InlineData(201, "နှစ်ရာ တစ်မြောက်")]
    [InlineData(1001, "တစ်ထောင် တစ်မြောက်")]
    public void WordsToNumber_ParsesGeneratedCompoundOrdinals(int number, string words)
    {
        Assert.Equal(words, number.ToOrdinalWords(My));
        Assert.Equal(number, words.ToNumber(My));
    }


    [Theory]
    [InlineData(300, "သုံးရာမြောက်")]
    [InlineData(1000, "တစ်ထောင်မြောက်")]
    [InlineData(999999999, "ကိုးဆယ့်ကိုးကုဋေ ကိုးသန်း ကိုးသိန်း ကိုးသောင်း ကိုးထောင် ကိုးရာ ကိုးဆယ့်ကိုးမြောက်")]
    public void WordsToNumber_ParsesGeneratedExactScaleOrdinals(int number, string words)
    {
        Assert.Equal(words, number.ToOrdinalWords(My));
        Assert.Equal(number, words.ToNumber(My));
    }

    [Theory]
    [InlineData("နှစ်ဆယ့်တစ်", 21)]
    [InlineData("တစ်ရာ ငါး", 105)]
    [InlineData("တစ်ထောင် နှစ်ရာ သုံးဆယ့်လေး", 1234)]
    [InlineData("တစ်ကုဋေ နှစ်သန်း သုံးသိန်း လေးသောင်း ငါးထောင် ခြောက်ရာ ခုနစ်ဆယ့်ရှစ်", 12345678)]
    [InlineData("အနုတ် နှစ်ဆယ့်တစ်", -21)]
    [InlineData("နှစ်ဆယ့်တစ်မြောက်", 21)]
    [InlineData("နှစ်ဆယ့်တစ်ခုမြောက်", 21)]
    [InlineData("တစ်ရာ တစ်ခုမြောက်", 101)]
    [InlineData("တစ်ထောင်ခုမြောက်", 1000)]
    [InlineData("21မြောက်", 21)]
    [InlineData("21 မြောက်", 21)]
    [InlineData("21\tမြောက်", 21)]
    [InlineData("21ခုမြောက်", 21)]
    [InlineData("21 ခုမြောက်", 21)]
    [InlineData("-1မြောက်", -1)]
    [InlineData("-1 မြောက်", -1)]
    [InlineData("-1ခုမြောက်", -1)]
    public void WordsToNumber_ParsesBurmeseCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(My));
        Assert.True(words.TryToNumber(out var parsed, My, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, "1 မြောက်")]
    [InlineData(21, "21 မြောက်")]
    [InlineData(-1, "-1 မြောက်")]
    public void Ordinalize_UsesBurmeseNumericSuffix(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(My));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(My));
    }

    [Theory]
    [InlineData(2022, 1, 25, "2022 ဇန်နဝါရီ 25")]
    [InlineData(2015, 1, 1, "2015 ဇန်နဝါရီ 1")]
    [InlineData(2015, 2, 3, "2015 ဖေဖော်ဝါရီ 3")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "2022 ဇန်နဝါရီ 25")]
    [InlineData(2015, 2, 3, "2015 ဖေဖော်ဝါရီ 3")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "နံနက် တစ်နာရီ ငါးမိနစ်")]
    [InlineData(13, 0, "နေ့လယ် တစ်နာရီ")]
    [InlineData(13, 23, "နေ့လယ် တစ်နာရီ နှစ်ဆယ့်သုံးမိနစ်")]
    [InlineData(18, 0, "ညနေ ခြောက်နာရီ")]
    [InlineData(21, 0, "ည ကိုးနာရီ")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("နေ့လယ် တစ်နာရီ နှစ်ဆယ့်ငါးမိနစ်", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "မြောက်")]
    [InlineData(45.0, "အရှေ့မြောက်")]
    [InlineData(90.0, "အရှေ့")]
    [InlineData(135.0, "အရှေ့တောင်")]
    [InlineData(180.0, "တောင်")]
    [InlineData(225.0, "အနောက်တောင်")]
    [InlineData(270.0, "အနောက်")]
    [InlineData(315.0, "အနောက်မြောက်")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "မြောက်")]
    [InlineData(45.0, "အရှေ့မြောက်")]
    [InlineData(90.0, "အရှေ့")]
    [InlineData(180.0, "တောင်")]
    [InlineData(270.0, "အနောက်")]
    public void Compass_AbbreviatedDirectionsUseBurmeseNames(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}