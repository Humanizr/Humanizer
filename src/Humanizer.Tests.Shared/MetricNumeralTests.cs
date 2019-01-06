﻿using System;
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
        [InlineData("0",         0d,          false, true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("123",       123d,        false, true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("-123",      (-123d),     false, true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1.23k",     1230d,       false, true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1 k",       1000d,       true,  true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1 kilo",    1000d,       true,  false, null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1milli",    1E-3,        false, false, null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1.23milli", 1.234E-3,    false, false, 2,    MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("12.34k",    12345,       false, true,  2,    MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("12k",       12345,       false, true,  0,    MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("-3.9m",     -3.91e-3,    false, true,  1,    MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("-3.9m",     -3.91e-3,    false, true,  1,    MetricPrefix.Mega,      MetricPrefix.Undefined)]
        [InlineData("1M",        1_000_000,   false, true,  null, MetricPrefix.Giga,      MetricPrefix.Undefined)]
        [InlineData("1000k",     1_000_000,   false, true,  null, MetricPrefix.Kilo,      MetricPrefix.Undefined)]
        [InlineData("1234.56k",  1_234_560,   false, true,  null, MetricPrefix.Kilo,      MetricPrefix.Undefined)]
        [InlineData("1m",        1E-3,        false, true,  null, MetricPrefix.Undefined, MetricPrefix.Undefined)]
        [InlineData("1000μ",     1E-3,        false, true,  null, MetricPrefix.Micro,     MetricPrefix.Undefined)]
        [InlineData("1000",      1000,        false, true,  null, MetricPrefix.None,      MetricPrefix.Undefined)]            
        public void ToMetric(string expected, double input, bool hasSpace, bool useSymbol, int? decimals, MetricPrefix maxPrefix, MetricPrefix minPrefix)
        {
            Assert.Equal(expected, input.ToMetric(hasSpace, useSymbol, decimals, maxPrefix, minPrefix));
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
