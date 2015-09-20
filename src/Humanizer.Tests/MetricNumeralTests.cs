using System;
using System.Diagnostics;
using System.Globalization;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
        public class MetricNumeralTests
        {
                [Theory]
                [InlineData("0", 0d, false)]
                [InlineData("123", 123d, false)]
                [InlineData("-123", (-123d), false)]
                [InlineData("1.23k", 1230d, false)]
                [InlineData("1 k", 1000d, true)]
                public void ToMetric(string expected, double input, bool isSplitedBySpace = false)
                {
                        Assert.Equal(expected, input.ToMetric(isSplitedBySpace));
                }

                [Theory]
                [InlineData(0, "0")]
                [InlineData(123d, "123")]
                [InlineData(-123d, "-123")]
                [InlineData(1230d, "1.23k")]
                [InlineData(1000d, "1 k")]
                public void FromMetric(double expected, string input)
                {
                        Assert.Equal(expected, input.FromMetric());
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
        }
}