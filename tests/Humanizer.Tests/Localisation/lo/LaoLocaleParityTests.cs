namespace Humanizer.Tests.Localisation.lo;

[UseCulture("lo")]
public class LaoLocaleParityTests
{
    static readonly CultureInfo Lo = new("lo");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesLaoConjunction()
    {
        Assert.Equal("1 ແລະ 2", Pair.Humanize());
        Assert.Equal("1, 2 ແລະ 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ມື້ວານ")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ມື້ອື່ນ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ມື້ ກ່ອນ")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "ໃນອີກ 2 ມື້")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ໜຶ່ງຊົ່ວໂມງກ່ອນ")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "ໃນອີກ 2 ຊົ່ວໂມງ")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ດຽວນີ້")]
    public void DateHumanize_UsesLaoPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Lo);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_UsesLaoNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ບໍ່ເຄີຍ", date.Humanize(culture: Lo));
    }

    [Theory]
    [InlineData(TimeUnit.Hour, 1, false, "1 ຊົ່ວໂມງ")]
    [InlineData(TimeUnit.Hour, 1, true, "ໜຶ່ງ ຊົ່ວໂມງ")]
    [InlineData(TimeUnit.Day, 2, false, "2 ມື້")]
    public void TimeSpanHumanize_UsesLaoDurationPhrases(TimeUnit unit, int count, bool toWords, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Lo);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count, toWords: toWords));
    }

    [Fact]
    public void ToAge_UsesLaoTemplate()
    {
        Assert.Equal("1 ປີ", TimeSpan.FromDays(366).ToAge(Lo));
    }

    [Theory]
    [InlineData(DataUnit.Byte, "ໄບຕ໌")]
    [InlineData(DataUnit.Kilobyte, "ກິໂລໄບຕ໌")]
    [InlineData(DataUnit.Megabyte, "ເມກະໄບຕ໌")]
    public void DataUnitHumanize_UsesLaoNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Lo);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanize_UsesSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Lo);
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: true));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "ວິນາທີ")]
    [InlineData(TimeUnit.Minute, "ນາທີ")]
    [InlineData(TimeUnit.Hour, "ຊົ່ວໂມງ")]
    [InlineData(TimeUnit.Day, "ມື້")]
    public void TimeUnitHumanize_UsesLaoLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Lo);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "ສູນ")]
    [InlineData(1, "ໜຶ່ງ")]
    [InlineData(11, "ສິບເອັດ")]
    [InlineData(20, "ຊາວ")]
    [InlineData(21, "ຊາວເອັດ")]
    [InlineData(99, "ເກົ້າສິບເກົ້າ")]
    [InlineData(100, "ໜຶ່ງ ຮ້ອຍ")]
    [InlineData(105, "ໜຶ່ງ ຮ້ອຍ ຫ້າ")]
    [InlineData(1234, "ໜຶ່ງ ພັນ ສອງ ຮ້ອຍ ສາມສິບສີ່")]
    [InlineData(1234567, "ໜຶ່ງ ລ້ານ ສອງ ແສນ ສາມ ໝື່ນ ສີ່ ພັນ ຫ້າ ຮ້ອຍ ຫົກສິບເຈັດ")]
    public void NumberToWords_ProducesExpectedLaoCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Lo));
    }

    [Theory]
    [InlineData(1, "ທີໜຶ່ງ")]
    [InlineData(2, "ທີສອງ")]
    [InlineData(6, "ທີຫົກ")]
    [InlineData(21, "ທີຊາວເອັດ")]
    [InlineData(22, "ທີຊາວສອງ")]
    [InlineData(100, "ທີໜຶ່ງ ຮ້ອຍ")]
    [InlineData(101, "ທີໜຶ່ງ ຮ້ອຍ ໜຶ່ງ")]
    [InlineData(-1, "ລົບ ທີໜຶ່ງ")]
    [InlineData(-22, "ລົບ ທີຊາວສອງ")]
    public void NumberToOrdinalWords_ProducesExpectedLaoOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Lo));
    }

    [Fact]
    public void NumberToOrdinalWords_HandlesMinimumIntegerMagnitude()
    {
        var words = int.MinValue.ToOrdinalWords(Lo);

        Assert.StartsWith("ລົບ ທີສອງ ຕື້", words);
        Assert.DoesNotContain("ລົບ ທີລົບ", words);
        Assert.Equal(int.MinValue, words.ToNumber(Lo));
    }

    [Theory]
    [InlineData("ສິບເອັດ", 11)]
    [InlineData("ຊາວເອັດ", 21)]
    [InlineData("ໜຶ່ງ ຮ້ອຍ ຫ້າ", 105)]
    [InlineData("ໜຶ່ງ ພັນ ສອງ ຮ້ອຍ ສາມສິບສີ່", 1234)]
    [InlineData("ລົບ ຊາວເອັດ", -21)]
    public void WordsToNumber_ParsesLaoCardinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Lo));
        Assert.True(words.TryToNumber(out var parsed, Lo, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1_001_000_000_000_000_000L)]
    [InlineData(1_234_567_890_123_456_789L)]
    public void WordsToNumber_RoundTripsHighLaoMagnitudes(long number)
    {
        var words = number.ToWords(Lo);

        Assert.Equal(number, words.ToNumber(Lo));
        Assert.True(words.TryToNumber(out var parsed, Lo, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ທີໜຶ່ງ", 1)]
    [InlineData("ທີຫົກ", 6)]
    [InlineData("ທີຊາວເອັດ", 21)]
    [InlineData("ທີຊາວສອງ", 22)]
    [InlineData("ທີໜຶ່ງ ຮ້ອຍ ໜຶ່ງ", 101)]
    [InlineData("ລົບ ທີຊາວເອັດ", -21)]
    [InlineData("ລົບ ທີຊາວສອງ", -22)]
    public void WordsToNumber_ParsesLaoOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Lo));
    }

    [Theory]
    [InlineData(1, "ທີ 1")]
    [InlineData(2, "ທີ 2")]
    [InlineData(21, "ທີ 21")]
    public void Ordinalize_UsesLaoNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 ມັງກອນ 2022")]
    [InlineData(2015, 2, 3, "3 ກຸມພາ 2015")]
    public void DateTimeToOrdinalWords_UsesLaoDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ມັງກອນ 2022")]
    [InlineData(2015, 2, 3, "3 ກຸມພາ 2015")]
    public void DateOnlyToOrdinalWords_UsesLaoDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "1 ໂມງ 5 ນາທີ")]
    [InlineData(13, 23, "13 ໂມງ 23 ນາທີ")]
    [InlineData(13, 0, "13 ໂມງ")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("13 ໂມງ 25 ນາທີ", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ເໜືອ")]
    [InlineData(45.0, "ຕາເວັນອອກສຽງເໜືອ")]
    [InlineData(90.0, "ຕາເວັນອອກ")]
    [InlineData(135.0, "ຕາເວັນອອກສຽງໃຕ້")]
    [InlineData(180.0, "ໃຕ້")]
    [InlineData(225.0, "ຕາເວັນຕົກສຽງໃຕ້")]
    [InlineData(270.0, "ຕາເວັນຕົກ")]
    [InlineData(315.0, "ຕາເວັນຕົກສຽງເໜືອ")]
    public void Compass_UsesLaoFullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Fact]
    public void Compass_UsesLaoAbbreviatedDirections()
    {
        Assert.Equal("ເໜືອ", 0.0.ToHeading(HeadingStyle.Abbreviated));
    }

    [Fact]
    public void NumberFormatting_UsesStableLaoSeparators()
    {
        Assert.Equal("1,95 KB", 2000.Bytes().Humanize());
        Assert.Equal("-1,2k", (-1234L).ToMetric(decimals: 1));
    }
}