[UseCulture("en-US")]
public class MetricNumeralTests
{
    // Return a sequence of -24 -> 26
    public static IEnumerable<object[]> SymbolRange => Enumerable
        .Range(-24, 51)
        .Select(e => new object[]
        {
            e
        });

    public static TheoryData<string, string> LocalizedScaleWordData { get; } = new()
    {
        { "af", "1 miljard" },
        { "am", "1 ቢሊዮን" },
        { "ar", "1 مليار" },
        { "as", "1 G" },
        { "az", "1 milyard" },
        { "be", "1 мільярд" },
        { "bg", "1 милиард" },
        { "bn", "1 G" },
        { "bs", "1 milijarda" },
        { "ca", "1 G" },
        { "cs", "1 miliarda" },
        { "cy", "1 biliwn" },
        { "da", "1 milliard" },
        { "de", "1 Milliarde" },
        { "de-CH", "1 Milliarde" },
        { "de-LI", "1 Milliarde" },
        { "el", "1 δισεκατομμύριο" },
        { "en", "1 billion" },
        { "en-GB", "1 billion" },
        { "en-IN", "1 arab" },
        { "en-US", "1 billion" },
        { "es", "1 G" },
        { "et", "1 miljard" },
        { "eu", "1 mila milioi" },
        { "fa", "1 میلیارد" },
        { "fi", "1 miljardi" },
        { "fil", "1 bilyon" },
        { "fr", "1 milliard" },
        { "fr-BE", "1 milliard" },
        { "fr-CH", "1 milliard" },
        { "ga", "1 billiún" },
        { "gl", "1 G" },
        { "gu", "1 અબજ" },
        { "ha", "1 biliyan" },
        { "he", "1 מיליארד" },
        { "hi", "1 अरब" },
        { "hr", "1 milijarda" },
        { "hu", "1 milliárd" },
        { "hy", "1 միլիարդ" },
        { "id", "1 miliar" },
        { "ig", "1 ijeri" },
        { "is", "1 milljarður" },
        { "it", "1 miliardo" },
        { "ja", "1 G" },
        { "ka", "1 მილიარდი" },
        { "kk", "1 миллиард" },
        { "km", "1 G" },
        { "kn", "1 ಅರಬ್" },
        { "ko", "1 G" },
        { "kok", "1 अब्ज" },
        { "ku", "1 میلیارد" },
        { "ky", "1 миллиард" },
        { "lb", "1 Milliard" },
        { "lo", "1 ຕື້" },
        { "lt", "1 milijardas" },
        { "lv", "1 miljards" },
        { "mk", "1 милијарда" },
        { "ml", "1 G" },
        { "mn", "1 тэрбум" },
        { "mr", "1 अब्ज" },
        { "ms", "1 bilion" },
        { "mt", "1 biljun" },
        { "my", "1 G" },
        { "nb", "1 milliard" },
        { "ne", "1 अर्ब" },
        { "nl", "1 miljard" },
        { "nn", "1 milliard" },
        { "or", "1 ଅବ୍ଜ" },
        { "pa", "1 ਅਰਬ" },
        { "pa-Arab", "1 ارب" },
        { "pl", "1 miliard" },
        { "ps", "1 میلیارد" },
        { "pt", "1 mil milhões" },
        { "pt-BR", "1 bilhão" },
        { "ro", "1 miliard" },
        { "ru", "1 миллиард" },
        { "si", "1 G" },
        { "sk", "1 miliarda" },
        { "sl", "1 milijarda" },
        { "so", "1 bilyan" },
        { "sq", "1 miliar" },
        { "sr", "1 милијарда" },
        { "sr-Latn", "1 milijarda" },
        { "sv", "1 miljard" },
        { "sw", "1 bilioni" },
        { "ta", "1 G" },
        { "te", "1 అరబ్" },
        { "th", "1 G" },
        { "tk", "1 milliard" },
        { "tr", "1 milyar" },
        { "uk", "1 мільярд" },
        { "ur", "1 ارب" },
        { "ur-IN", "1 ارب" },
        { "ur-PK", "1 ارب" },
        { "uz-Cyrl-UZ", "1 миллиард" },
        { "uz-Latn-UZ", "1 milliard" },
        { "vi", "1 tỉ" },
        { "yo", "1 bílíọ̀nù" },
        { "zh-CN", "1 G" },
        { "zh-Hans", "1 G" },
        { "zh-Hant", "1 G" },
        { "zu-ZA", "1 ibhiliyoni" }
    };

    [Theory]
    [InlineData(0, "0")]
    [InlineData(123d, "123")]
    [InlineData(-123d, "-123")]
    [InlineData(1230d, "1.23k")]
    [InlineData(1000d, "1 k")]
    [InlineData(1000d, "1 kilo")]
    [InlineData(1E-3, "1milli")]
    public void FromMetric(double expected, string input) =>
        Assert.Equal(expected, input.FromMetric());

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("12yy")]
    [InlineData("-8e")]
    [InlineData("0.12c")]
    [InlineData("0.02l")]
    [InlineData("0.12kilkilo")]
    [InlineData("0.02alois")]
    public void FromMetricOnInvalid(string input) =>
        Assert.Throws<ArgumentException>(() => input.FromMetric());

    [Fact]
    public void FromMetricOnNull() =>
        Assert.Throws<ArgumentNullException>(() =>
            MetricNumeralExtensions.FromMetric(null!));

    [Theory]
    [MemberData(nameof(SymbolRange))]
    public void TestAllSymbols(int e)
    {
        var origin = Math.Pow(10, e);
        var to = origin.ToMetric();
        var from = to.FromMetric();

        var c = Equals(
            origin.ToString("0.##E+0", CultureInfo.InvariantCulture),
            from.ToString("0.##E+0", CultureInfo.InvariantCulture));

        Assert.True(c);
    }

    [Theory]
    [InlineData(-9)]
    [InlineData(-3)]
    [InlineData(-2)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(9)]
    public void TestAllSymbolsAsInt(int exponent)
    {
        var origin = Convert.ToInt32(Math.Pow(10, exponent));
        var isEquals = Equals(
            origin.ToString("0.##E+0", CultureInfo.InvariantCulture),
            origin
                .ToMetric()
                .FromMetric()
                .ToString("0.##E+0", CultureInfo.InvariantCulture));
        Assert.True(isEquals);
    }

    public static TheoryData<long, int?, string> GenerateLongToMetricTestCases()
    {
        // Dividing by 1.000...M removes all trailing zeros by changing the scale factor.
        static decimal RemoveTrailingZeroes(ref decimal value)
            => value /= 1.000000000000000000000000000000000M;

        static string FormatExpected(decimal value, string suffix) =>
            value.ToString(CultureInfo.InvariantCulture) + suffix;

        var data = new TheoryData<long, int?, string>();

        // 0-999
        foreach (var value in (long[])[0, 123])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 3, 20])
            {
                data.Add(value, decimals, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        // 1,000-999,999
        foreach (var value in (long[])[1245, 23456, 123456, 123056, 123999])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 20])
            {
                var scaledValue = value * 0.001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "k");

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000-999,999,999
        foreach (var value in (long[])[23456789, 123456785, 123050709])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 4, 5, 6, 20])
            {
                var scaledValue = value * 0.000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "M");

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000-999,999,999,999
        foreach (var value in (long[])[23456789165, 123456789123, 123050709020])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 7, 8, 9, 20])
            {
                var scaledValue = value * 0.000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "G");

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000,000-999,999,999,999,999
        foreach (var value in (long[])[23456789123456, 123456789123465, 123050709020406])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 10, 11, 12, 20])
            {
                var scaledValue = value * 0.000000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "T");

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000,000,000-999,999,999,999,999,999
        foreach (var value in (long[])[23456789123456789, 123456789123456785, 123050709020406080])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 13, 14, 15, 20])
            {
                var scaledValue = value * 0.000000000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "P");

                data.Add(value, decimals, expected);
            }
        }
        // 1,000,000,000,000,000,000-
        foreach (var value in (long[])[1_001_002_003_004_006_005, 2_305_079_902_040_799_020, long.MaxValue])
        {
            for (var decimalsValue = -1; decimalsValue <= 20; decimalsValue++)
            {
                var decimals = decimalsValue < 0 ? null : (int?)decimalsValue;

                var scaledValue = value * 0.000000000000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = FormatExpected(truncatedValue, "E");

                data.Add(value, decimals, expected);
            }
        }

        // Verify banker's rounding is used
        data.Add(2400, 0, "2k");
        data.Add(2500, 0, "2k");
        data.Add(2600, 0, "3k");
        data.Add(3400, 0, "3k");
        data.Add(3500, 0, "4k");
        data.Add(3600, 0, "4k");

        // Verify scale changes due to rounding are handled.
        data.Add(999500, 0, "1M");
        data.Add(999600000, 0, "1G");
        data.Add(999700000000, 0, "1T");
        data.Add(999500000000001, 0, "1P");
        data.Add(999500000000000000, 0, "1E");

        foreach (var positiveTestCase in data.ToList())
        {
            var negativeValue = -positiveTestCase.Data.Item1;
            var decimals = positiveTestCase.Data.Item2;
            var negativeExpected =
                negativeValue != 0
                ? "-" + positiveTestCase.Data.Item3
                : positiveTestCase.Data.Item3;

            data.Add(negativeValue, decimals, negativeExpected);
        }

        return data;
    }

    [Theory]
    [InlineData("999", 999)]
    [InlineData("1k", 1000)]
    [InlineData("999.5k", 999500)]
    [InlineData("1M", 1000000)]
    [InlineData("2.147483647G", int.MaxValue)]
    [InlineData("-2.147483648G", int.MinValue)]
    public void ToMetricUsesDefaultIntFormatting(string expected, int subject) =>
        Assert.Equal(expected, subject.ToMetric());

    [Theory]
    [MemberData(nameof(GenerateLongToMetricTestCases))]
    public void TestAllSymbolsAsLong(long subject, int? decimals, string expected) =>
        Assert.Equal(expected, subject.ToMetric(decimals: decimals));

    [Theory]
    [InlineData("1.3M", 1300000, null, null)]
    [InlineData("1.3million", 1300000, MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1.3 million", 1300000, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1.3 million", 1300000, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("0", 0d, null, null)]
    [InlineData("123", 123d, null, null)]
    [InlineData("-123", -123d, null, null)]
    [InlineData("1.23k", 1230d, null, null)]
    [InlineData("1 k", 1000d, MetricNumeralFormats.WithSpace, null)]
    [InlineData("1milli", 1E-3, MetricNumeralFormats.UseName, null)]
    [InlineData("1.23milli", 1.234E-3, MetricNumeralFormats.UseName, 2)]
    [InlineData("12.34k", 12345, null, 2)]
    [InlineData("12k", 12345, null, 0)]
    [InlineData("1M", 999500d, null, 0)]
    [InlineData("-3.9m", -3.91e-3, null, 1)]
    [InlineData("10 ", 10, MetricNumeralFormats.WithSpace, 0)]
    [InlineData("1.2", 1.23, null, 1)]
    [InlineData("1thousand", 1000d, MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1.23 thousand", 1230d, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1Y", 1E24, null, null)]
    [InlineData("1 yotta", 1E24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 septillion", 1E24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 quadrillion", 1E24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1Z", 1E21, null, null)]
    [InlineData("1 zetta", 1E21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 sextillion", 1E21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 trilliard", 1E21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1E", 1E18, null, null)]
    [InlineData("1 exa", 1E18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 quintillion", 1E18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 trillion", 1E18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1P", 1E15, null, null)]
    [InlineData("1 peta", 1E15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 quadrillion", 1E15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 billiard", 1E15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1T", 1E12, null, null)]
    [InlineData("1 tera", 1E12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 trillion", 1E12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 billion", 1E12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1G", 1E9, null, null)]
    [InlineData("1 giga", 1E9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 billion", 1E9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 milliard", 1E9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1M", 1E6, null, null)]
    [InlineData("1 mega", 1E6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 million", 1E6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 million", 1E6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1k", 1E3, null, null)]
    [InlineData("1 kilo", 1E3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 thousand", 1E3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 thousand", 1E3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1y", 1E-24, null, null)]
    [InlineData("1 yocto", 1E-24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 septillionth", 1E-24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 quadrillionth", 1E-24, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1z", 1E-21, null, null)]
    [InlineData("1 zepto", 1E-21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 sextillionth", 1E-21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 trilliardth", 1E-21, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1a", 1E-18, null, null)]
    [InlineData("1 atto", 1E-18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 quintillionth", 1E-18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 trillionth", 1E-18, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1f", 1E-15, null, null)]
    [InlineData("1 femto", 1E-15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 quadrillionth", 1E-15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 billiardth", 1E-15, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1p", 1E-12, null, null)]
    [InlineData("1 pico", 1E-12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 trillionth", 1E-12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 billionth", 1E-12, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1n", 1E-9, null, null)]
    [InlineData("1 nano", 1E-9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 billionth", 1E-9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 milliardth", 1E-9, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1μ", 1E-6, null, null)]
    [InlineData("1 micro", 1E-6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 millionth", 1E-6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 millionth", 1E-6, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1m", 1E-3, null, null)]
    [InlineData("1 milli", 1E-3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, null)]
    [InlineData("1 thousandth", 1E-3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, null)]
    [InlineData("1 thousandth", 1E-3, MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, null)]
    [InlineData("1k", 999.9, null, 0)]
    [InlineData("-1k", -999.9, null, 0)]
    [InlineData("1k", 999.99, null, 1)]
    [InlineData("999", 999.4, null, 0)]
    public void ToMetric(string expected, double input, MetricNumeralFormats? format, int? decimals) =>
        Assert.Equal(expected, input.ToMetric(format, decimals));

    [Fact]
    public void ToMetric_KeepTrailingZeros_IsOptIn()
    {
        Assert.Equal("1k", 1000.ToMetric(decimals: 2));
        Assert.Equal("1k", 1000.ToMetric(MetricNumeralFormats.KeepTrailingZeros));
        Assert.Equal("1.00k", 1000.ToMetric(MetricNumeralFormats.KeepTrailingZeros, decimals: 2));
    }

    [Theory]
    [InlineData("0.00", 0d)]
    [InlineData("123.00", 123d)]
    [InlineData("1.20k", 1200d)]
    [InlineData("-1.20k", -1200d)]
    [InlineData("1.00M", 999999d)]
    [InlineData("1.00", 0.999999d)]
    [InlineData("-1.00", -0.999999d)]
    public void ToMetric_KeepTrailingZeros_Double(string expected, double input) =>
        Assert.Equal(expected, input.ToMetric(MetricNumeralFormats.KeepTrailingZeros, decimals: 2));

    [Theory]
    [InlineData("0.00000", 0L, 5)]
    [InlineData("123.00000", 123L, 5)]
    [InlineData("1.00000k", 1000L, 5)]
    [InlineData("-1.20000k", -1200L, 5)]
    [InlineData("1.00M", 999999L, 2)]
    public void ToMetric_KeepTrailingZeros_Long(string expected, long input, int decimals) =>
        Assert.Equal(expected, input.ToMetric(MetricNumeralFormats.KeepTrailingZeros, decimals));

    [Fact]
    public void ToMetric_KeepTrailingZeros_ComposesWithSpaceForZero()
    {
        var formats = MetricNumeralFormats.KeepTrailingZeros | MetricNumeralFormats.WithSpace;

        Assert.Equal("0.00 ", 0L.ToMetric(formats, decimals: 2));
        Assert.Equal("0.00 ", 0d.ToMetric(formats, decimals: 2));
    }

    [Fact]
    public void ToMetric_KeepTrailingZeros_ComposesWithScaleWordAndCulture()
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("de-DE"), new("en-US"));

        Assert.Equal(
            "1,00 billion",
            1E9.ToMetric(
                MetricNumeralFormats.KeepTrailingZeros | MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord,
                decimals: 2));
    }

    [Theory]
    [InlineData(1E+27)]
    [InlineData(1E-27)]
    [InlineData(-1E+27)]
    [InlineData(-1E-27)]
    public void ToMetricOnInvalid(double input) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => input.ToMetric());

    [Theory]
    [InlineData(1E-3, "1 thousandth")]
    [InlineData(1E-18, "1 quintillionth")]
    [InlineData(1E-21, "1 z")]
    [InlineData(1E9, "1 billion")]
    [InlineData(1E12, "1 trillion")]
    public void ToMetric_UseScaleWord_ShortScale(double input, string expected) =>
        Assert.Equal(expected, input.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));

    [Theory]
    [InlineData(1E3, "1 tausend")]
    [InlineData(1E9, "1 Milliarde")]
    [InlineData(1_000_000_000.000001, "1 Milliarde")]
    [InlineData(1.5E9, "1.5 Milliarden")]
    [InlineData(2E9, "2 Milliarden")]
    public void ToMetric_UseScaleWord_UsesCurrentUiCultureAndDisplayedQuantity(double input, string expected)
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("en-US"), new("de-DE"));

        Assert.Equal(expected, input.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));
    }

    [Theory]
    [InlineData("hr", 2E9, "2 milijarde")]
    [InlineData("lt", 2E9, "2 milijardai")]
    [InlineData("ru", 2E9, "2 миллиарда")]
    [InlineData("sk", 2E9, "2 miliardy")]
    [InlineData("sl", 2E9, "2 milijardi")]
    [InlineData("sl", 3E9, "3 milijarde")]
    public void ToMetric_UseScaleWord_UsesLocaleAuthoredCountForm(string uiCulture, double input, string expected)
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("en-US"), new(uiCulture));

        Assert.Equal(expected, input.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));
    }

    [Fact]
    public void ToMetric_UseScaleWord_UsesLocaleAuthoredCountFormForLong()
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("en-US"), new("lt"));

        Assert.Equal(
            "2 milijardai",
            2_000_000_000L.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));
    }

    [Fact]
    public void ToMetric_UseScaleWord_UsesLocaleAuthoredInverseScaleWord()
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("en-US"), new("am"));

        Assert.Equal("1 ሺኛ", 1E-3.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));
    }

    [Theory]
    [MemberData(nameof(LocalizedScaleWordData))]
    public void ToMetric_UseScaleWord_UsesLocaleAuthoredScaleWordOrSymbol(string uiCulture, string expected)
    {
        using var _ = new Humanizer.Tests.Localisation.DistinctCultureSwap(new("en-US"), new(uiCulture));

        Assert.Equal(expected, 1E9.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseScaleWord));
    }
}

[UseCulture("de-DE")]
public class MetricNumeralTestDataTests
{
    [Fact]
    public void LongTestCaseExpectationsUseInvariantCulture()
    {
        var testCase = MetricNumeralTests
            .GenerateLongToMetricTestCases()
            .Single(x => x.Data.Item1 == 1245 && x.Data.Item2 == 2);

        Assert.Equal("1.24k", testCase.Data.Item3);
    }
}