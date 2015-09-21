﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Configuration;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
        public class MetricNumeralTests
        {
                [Theory]
                [InlineData("0", 0d, false, true)]
                [InlineData("123", 123d, false, true)]
                [InlineData("-123", (-123d), false, true)]
                [InlineData("1.23k", 1230d, false, true)]
                [InlineData("1 k", 1000d, true, true)]
                [InlineData("1 kilo", 1000d, true, false)]
                [InlineData("1milli", 1E-3, false, false)]
                public void ToMetric(string expected, double input,
                        bool isSplitedBySpace, bool useSymbol)
                {
                        Assert.Equal(expected, input.ToMetric(isSplitedBySpace, useSymbol));
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


                [Fact]
                public void TestAllSymbols()
                {
                        var b = true;
                        for (var i = -24; i < 27; i++)
                        {
                                var origin = Math.Pow(10, i);
                                var to = origin.ToMetric();
                                var from = to.FromMetric();

                                var c = Equals(
                                        origin.ToString("0.##E+0", CultureInfo.InvariantCulture),
                                        from.ToString("0.##E+0", CultureInfo.InvariantCulture));
                                if (!c)
                                        Debugger.Break();

                                b &= c;
                        }
                        Assert.True(b);
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
                                origin.ToMetric().FromMetric().ToString("0.##E+0", CultureInfo.InvariantCulture));
                        if (!isEquals)
                                Debugger.Break();
                        Assert.True(isEquals);
                }
        }
}