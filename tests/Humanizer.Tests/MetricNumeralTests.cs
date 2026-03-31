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

        var data = new TheoryData<long, int?, string>();

        // 0-999
        foreach (var value in (long[])[0, 123])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 3, 20])
            {
                data.Add(value, decimals, value.ToString());
            }
        }

        // Verify banker's rounding is used
        data.Add(2400, 0, "2k");
        data.Add(2500, 0, "2k");
        data.Add(2600, 0, "3k");
        data.Add(3400, 0, "3k");
        data.Add(3500, 0, "4k");
        data.Add(3600, 0, "4k");

        // 1,000-999,999
        foreach (var value in (long[])[23456, 123456, 123056, 123999])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 20])
            {
                var scaledValue = value * 0.001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = truncatedValue + "k";

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000-999,999,999
        foreach (var value in (long[])[23456789, 123456789, 123050709])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 4, 5, 6, 20])
            {
                var scaledValue = value * 0.000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = truncatedValue + "M";

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000-999,999,999,999
        foreach (var value in (long[])[23456789123, 123456789123, 123050709020])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 7, 8, 9, 20])
            {
                var scaledValue = value * 0.000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = truncatedValue + "G";

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000,000-999,999,999,999,999
        foreach (var value in (long[])[23456789123456, 123456789123456, 123050709020406])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 10, 11, 12, 20])
            {
                var scaledValue = value * 0.000000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = truncatedValue + "T";

                data.Add(value, decimals, expected);
            }
        }

        // 1,000,000,000,000,000-999,999,999,999,999,999
        foreach (var value in (long[])[23456789123456789, 123456789123456789, 123050709020406080])
        {
            foreach (var decimals in (int?[])[null, 0, 1, 2, 3, 13, 14, 15, 20])
            {
                var scaledValue = value * 0.000000000000001M;

                var truncatedValue =
                    decimals.HasValue
                    ? Math.Round(scaledValue, decimals.Value)
                    : scaledValue;

                RemoveTrailingZeroes(ref truncatedValue);

                var expected = truncatedValue + "P";

                data.Add(value, decimals, expected);
            }
        }
        // 1,000,000,000,000,000,000-
        foreach (var value in (long[])[1_001_002_003_004_005_006, 2_305_079_902_040_799_020, long.MaxValue])
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

                var expected = truncatedValue + "E";

                data.Add(value, decimals, expected);
            }
        }

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
    public void ToMetric(string expected, double input, MetricNumeralFormats? format, int? decimals) =>
        Assert.Equal(expected, input.ToMetric(format, decimals));

    [Theory]
    [InlineData(1E+27)]
    [InlineData(1E-27)]
    [InlineData(-1E+27)]
    [InlineData(-1E-27)]
    public void ToMetricOnInvalid(double input) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => input.ToMetric());
}