namespace Humanizer.Tests.Localisation.km;

[UseCulture("km")]
public class KhmerLocaleParityTests
{
#pragma warning disable IDE1006 // PR review requires camelCase private static readonly fields here.
    static readonly CultureInfo km = new("km");
    static readonly int[] pair = [1, 2];
    static readonly int[] triple = [1, 2, 3];
#pragma warning restore IDE1006

    public static IEnumerable<object[]> GluedScaleCountData
    {
        get
        {
            var counts = Enumerable.Range(1, 999).ToArray();
            (long Scale, string Suffix)[] scales =
            [
                (100, "រយ"),
                (1000, "ពាន់"),
                (10000, "ម៉ឺន"),
                (100000, "សែន"),
                (1000000, "លាន")
            ];

            foreach (var (scale, suffix) in scales)
            {
                foreach (var count in counts.Where(count => count < scale))
                {
                    yield return [count.ToWords(km).Replace(" ", string.Empty) + suffix, count * scale];
                }
            }
        }
    }

    [Fact]
    public void ListHumanize_UsesKhmerConjunction()
    {
        Assert.Equal("1 និង 2", pair.Humanize());
        Assert.Equal("1, 2 និង 3", triple.Humanize());
    }

    [Theory]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ឥឡូវនេះ")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ម្សិលមិញ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ថ្ងៃមុន")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ថ្ងៃស្អែក")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "ក្នុង 2 ថ្ងៃ")]
    public void DateHumanize_UsesKhmerRelativeDatePhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(km);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesKhmerNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("មិនដែល", date.Humanize(culture: km));
    }

    [Theory]
    [InlineData(TimeUnit.Minute, 1, "1 នាទី")]
    [InlineData(TimeUnit.Day, 2, "2 ថ្ងៃ")]
    public void TimeSpanHumanize_UsesKhmerDurationPhrases(TimeUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(km);
        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, count));
    }

    [Fact]
    public void TimeSpanHumanize_ZeroUsesKhmerPhrases()
    {
        Assert.Equal("0 មិល្លីវិនាទី", TimeSpan.Zero.Humanize(culture: km));
        Assert.Equal("គ្មានពេលវេលា", TimeSpan.Zero.Humanize(culture: km, toWords: true));
    }

    [Fact]
    public void TimeSpanHumanize_ToWordsUsesKhmerNumberWords()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(km);
        Assert.Equal("មួយ ថ្ងៃ", formatter.TimeSpanHumanize(TimeUnit.Day, 1, toWords: true));
    }

    [Theory]
    [InlineData(DataUnit.Bit, "ប៊ីត")]
    [InlineData(DataUnit.Byte, "បៃ")]
    [InlineData(DataUnit.Kilobyte, "គីឡូបៃ")]
    [InlineData(DataUnit.Megabyte, "មេហ្គាបៃ")]
    public void DataUnitHumanize_UsesKhmerNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(km);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "មិល្លីវិនាទី")]
    [InlineData(TimeUnit.Second, "វិនាទី")]
    [InlineData(TimeUnit.Minute, "នាទី")]
    [InlineData(TimeUnit.Hour, "ម៉ោង")]
    [InlineData(TimeUnit.Day, "ថ្ងៃ")]
    [InlineData(TimeUnit.Week, "សប្តាហ៍")]
    [InlineData(TimeUnit.Month, "ខែ")]
    [InlineData(TimeUnit.Year, "ឆ្នាំ")]
    public void TimeUnitHumanize_UsesKhmerUnitNames(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(km);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "សូន្យ")]
    [InlineData(21, "ម្ភៃមួយ")]
    [InlineData(105, "មួយ រយ ប្រាំ")]
    [InlineData(1234, "មួយ ពាន់ ពីរ រយ សាមសិបបួន")]
    [InlineData(12345678, "ដប់ពីរ លាន បី សែន បួន ម៉ឺន ប្រាំ ពាន់ ប្រាំមួយ រយ ចិតសិបប្រាំបី")]
    [InlineData(-21, "ដក ម្ភៃមួយ")]
    public void NumberToWords_ProducesKhmerCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(km));
    }

    [Fact]
    public void NumberToWords_MaximumValueRoundTripsThroughKhmerParser()
    {
        const long number = 999_999_999;

        var words = number.ToWords(km);

        Assert.StartsWith("ប្រាំបួន រយ កៅសិបប្រាំបួន លាន", words, StringComparison.Ordinal);
        Assert.Equal(number, words.ToNumber(km));
    }

    [Theory]
    [InlineData(1, "ទីមួយ")]
    [InlineData(2, "ទីពីរ")]
    [InlineData(4, "ទីបួន")]
    [InlineData(21, "ទីម្ភៃមួយ")]
    [InlineData(22, "ទីម្ភៃពីរ")]
    [InlineData(101, "ទីមួយរយ មួយ")]
    public void NumberToOrdinalWords_ProducesKhmerOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(km));
    }

    [Theory]
    [InlineData("ម្ភៃមួយ", 21)]
    [InlineData("មួយរយ ប្រាំ", 105)]
    [InlineData("មួយពាន់ ពីររយ សាមសិបបួន", 1234)]
    [InlineData("ដប់ពីរលាន បីសែន បួនម៉ឺន ប្រាំពាន់ ប្រាំមួយរយ ចិតសិបប្រាំបី", 12345678)]
    [InlineData("ដក ម្ភៃមួយ", -21)]
    [InlineData("ទីបួន", 4)]
    [InlineData("ទីម្ភៃមួយ", 21)]
    [InlineData("ទីម្ភៃពីរ", 22)]
    [InlineData("21ទី", 21)]
    [InlineData("21 ទី", 21)]
    public void WordsToNumber_ParsesKhmerCardinalsAndOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(km));
        Assert.True(words.TryToNumber(out var parsed, km, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ពីររយ", 200)]
    [InlineData("បីរយ", 300)]
    [InlineData("ប្រាំបួនរយ", 900)]
    [InlineData("ពីរពាន់", 2000)]
    [InlineData("បីពាន់", 3000)]
    [InlineData("ពីរម៉ឺន", 20000)]
    [InlineData("ពីរសែន", 200000)]
    [InlineData("ពីរលាន", 2000000)]
    [InlineData("ដប់ពីរលាន", 12000000)]
    [InlineData("កៅសិបប្រាំបួនលាន", 99000000)]
    [InlineData("ប្រាំបួនរយលាន", 900000000)]
    [InlineData("មួយរយប្រាំលាន", 105000000)]
    [InlineData("ប្រាំបួនរយ កៅសិបប្រាំបួនលាន", 999000000)]
    [InlineData("ប្រាំបួនរយកៅសិបប្រាំបួនពាន់លាន", 999000000000)]
    [InlineData("បីពាន់ បួនរយ ម្ភៃពីរ", 3422)]
    [InlineData("ដប់ពីរលាន បីសែន បួនម៉ឺន ប្រាំពាន់ ប្រាំមួយរយ ចិតសិបប្រាំបី", 12345678)]
    public void WordsToNumber_ParsesCommonGluedScaleTokens(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(km));
        Assert.True(words.TryToNumber(out var parsed, km, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [MemberData(nameof(GluedScaleCountData))]
    public void WordsToNumber_ParsesAllProductiveGluedScaleCounts(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(km));
        Assert.True(words.TryToNumber(out var parsed, km, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1, "1ទី")]
    [InlineData(21, "21ទី")]
    [InlineData(-1, "-1ទី")]
    public void Ordinalize_UsesKhmerNumericSuffixThatParsesBack(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(km));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(km));
        Assert.Equal(number, expected.ToNumber(km));
    }

    [Theory]
    [InlineData(200, "ពីរ រយ")]
    [InlineData(2000, "ពីរ ពាន់")]
    [InlineData(20000, "ពីរ ម៉ឺន")]
    [InlineData(200000, "ពីរ សែន")]
    [InlineData(2000000, "ពីរ លាន")]
    [InlineData(12000000, "ដប់ពីរ លាន")]
    public void NumberToWords_TypicalScaleSeparatedOutputsRoundTrip(long number, string expected)
    {
        var words = number.ToWords(km);

        Assert.Equal(expected, words);
        Assert.Equal(number, words.ToNumber(km));
        Assert.True(words.TryToNumber(out var parsed, km, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 មករា 2022")]
    [InlineData(2015, 1, 1, "1 មករា 2015")]
    [InlineData(2015, 2, 3, "3 កុម្ភៈ 2015")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 មករា 2022")]
    [InlineData(2015, 2, 3, "3 កុម្ភៈ 2015")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "ព្រឹក មួយ ម៉ោង ប្រាំ នាទី")]
    [InlineData(13, 0, "រសៀល មួយ ម៉ោង")]
    [InlineData(13, 23, "រសៀល មួយ ម៉ោង ម្ភៃបី នាទី")]
    [InlineData(18, 0, "ល្ងាច ប្រាំមួយ ម៉ោង")]
    [InlineData(21, 0, "យប់ ប្រាំបួន ម៉ោង")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("រសៀល មួយ ម៉ោង ម្ភៃប្រាំ នាទី", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ជើង")]
    [InlineData(45.0, "ឦសាន")]
    [InlineData(90.0, "កើត")]
    [InlineData(135.0, "អាគ្នេយ៍")]
    [InlineData(180.0, "ត្បូង")]
    [InlineData(225.0, "និរតី")]
    [InlineData(270.0, "លិច")]
    [InlineData(315.0, "ពាយព្យ")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "ជ")]
    [InlineData(90.0, "ក")]
    [InlineData(180.0, "ត")]
    [InlineData(270.0, "ល")]
    public void Compass_AbbreviatedDirectionsUseKhmerLabels(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}