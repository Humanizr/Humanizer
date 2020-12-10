using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class MetricNumeralTests
    {
        // Return a sequence of -24 -> 26
        public static IEnumerable<object[]> SymbolRange => Enumerable.Range(-24, 51).Select(e => new object[] { e });

        [Theory]
        [InlineData(0, "0")]
        [InlineData(123d, "123")]
        [InlineData(-123d, "-123")]
        [InlineData(1230d, "1.23k")]
        [InlineData(1000d, "1 k")]
        [InlineData(1000d, "1 kilo")]
        [InlineData(1E-3, "1milli")]
        public void FromMetric(double expected, string input)
        {
            Assert.Equal(expected, input.FromMetric());
        }

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
        public void FromMetricOnInvalid(string input)
        {
            Assert.Throws<ArgumentException>(() => input.FromMetric());
        }

        [Fact]
        public void FromMetricOnNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                                                 MetricNumeralExtensions.FromMetric(null));
        }

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
                origin.ToMetric()
                      .FromMetric()
                      .ToString("0.##E+0", CultureInfo.InvariantCulture));
            if (!isEquals)
            {
                Debugger.Break();
            }

            Assert.True(isEquals);
        }

        [Theory]
        [InlineData("0", 0d, false, true, null)]
        [InlineData("123", 123d, false, true, null)]
        [InlineData("-123", (-123d), false, true, null)]
        [InlineData("1.23k", 1230d, false, true, null)]
        [InlineData("1 k", 1000d, true, true, null)]
        [InlineData("1 kilo", 1000d, true, false, null)]
        [InlineData("1milli", 1E-3, false, false, null)]
        [InlineData("1.23milli", 1.234E-3, false, false, 2)]
        [InlineData("12.34k", 12345, false, true, 2)]
        [InlineData("12k", 12345, false, true, 0)]
        [InlineData("-3.9m", -3.91e-3, false, true, 1)]
        [InlineData("10 ", 10, true, false, 0)]
        [InlineData("1.2", 1.23, false, false, 1)]
        public void ToMetricObsolete(string expected, double input, bool hasSpace, bool useSymbol, int? decimals)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal(expected, input.ToMetric(hasSpace, useSymbol, decimals));
#pragma warning restore CS0618 // Type or member is obsolete
        }

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
        public void ToMetric(string expected, double input, MetricNumeralFormats? format, int? decimals)
        {
            Assert.Equal(expected, input.ToMetric(format, decimals));
        }

        [Theory]
        [InlineData(1E+27)]
        [InlineData(1E-27)]
        [InlineData(-1E+27)]
        [InlineData(-1E-27)]
        public void ToMetricOnInvalid(double input)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => input.ToMetric());
        }
    }
}
