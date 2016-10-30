using System;
using Humanizer.Bytes;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    [UseCulture("en")]
    public class ByteRateTests
    {
        [Theory]
        [InlineData(400, 1, "400 B/s")]
        [InlineData(4 * 1000, 1, "4 kB/s")]
        [InlineData(4 * 1000 * 1000, 1, "4 MB/s")]
        [InlineData(4 * 2 * 1000 * 1000, 2, "4 MB/s")]
        [InlineData(4 * 1000, 0.1, "40 kB/s")]
        [InlineData(15 * 60 * 1000 * 1000, 60, "15 MB/s")]
        public void HumanizesRates(long inputBytes, double perSeconds, string expectedValue)
        {
            var size = new ByteSize(inputBytes);
            var interval = TimeSpan.FromSeconds(perSeconds);

            var rate = size.Per(interval).Humanize();

            Assert.Equal(expectedValue, rate);
        }

        [Theory]
        [InlineData(1, 1, TimeUnit.Second, "1 MB/s")]
        [InlineData(1, 60, TimeUnit.Minute, "1 MB/min")]
        [InlineData(1, 60 * 60, TimeUnit.Hour, "1 MB/hour")]
        [InlineData(10, 1, TimeUnit.Second, "10 MB/s")]
        [InlineData(10, 60, TimeUnit.Minute, "10 MB/min")]
        [InlineData(10, 60 * 60, TimeUnit.Hour, "10 MB/hour")]
        [InlineData(1, 10 * 1, TimeUnit.Second, "100 kB/s")]
        [InlineData(1, 10 * 60, TimeUnit.Minute, "100 kB/min")]
        [InlineData(1, 10 * 60 * 60, TimeUnit.Hour, "100 kB/hour")]
        public void TimeUnitTests(long megabytes, double measurementIntervalSeconds, TimeUnit displayInterval, string expectedValue)
        {
            var size = ByteSize.FromMegabytes(megabytes);
            var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);

            var rate = size.Per(measurementInterval);
            var text = rate.Humanize(displayInterval);

            Assert.Equal(expectedValue, text);
        }

        [Theory]
        [InlineData(19854651984, 1, TimeUnit.Second, null, "19.854651984 GB/s")]
        [InlineData(19854651984, 1, TimeUnit.Second, "#.##", "19.85 GB/s")]
        public void FormattedTimeUnitTests(long bytes, int measurementIntervalSeconds, TimeUnit displayInterval, string format, string expectedValue)
        {
            var size = ByteSize.FromBytes(bytes);
            var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);
            var rate = size.Per(measurementInterval);
            var text = rate.Humanize(format, displayInterval);

            Assert.Equal(expectedValue, text);
        }



        [Theory]
        [InlineData(TimeUnit.Millisecond)]
        [InlineData(TimeUnit.Day)]
        [InlineData(TimeUnit.Month)]
        [InlineData(TimeUnit.Week)]
        [InlineData(TimeUnit.Year)]
        public void ThowsOnUnsupportedData(TimeUnit units)
        {
            var dummyRate = ByteSize.FromBits(1).Per(TimeSpan.FromSeconds(1));

            Assert.Throws<NotSupportedException>(() =>
            {
                dummyRate.Humanize(units);
            });
        }

    }
}
