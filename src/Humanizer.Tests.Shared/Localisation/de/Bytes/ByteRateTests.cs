using System;

using Humanizer.Bytes;
using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.de.Bytes
{
    [UseCulture("de-DE")]
    public class ByteRateTests
    {
        [Theory]
        [InlineData(400, 1, "400 B/s")]
        [InlineData(4 * 1024, 1, "4 kB/s")]
        [InlineData(4 * 1024 * 1024, 1, "4 MB/s")]
        [InlineData(4 * 2 * 1024 * 1024, 2, "4 MB/s")]
        [InlineData(4 * 1024, 0.1, "40 kB/s")]
        [InlineData(15 * 60 * 1024 * 1024, 60, "15 MB/s")]
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
        [InlineData(1, 60 * 60, TimeUnit.Hour, "1 MB/h")]
        [InlineData(10, 1, TimeUnit.Second, "10 MB/s")]
        [InlineData(10, 60, TimeUnit.Minute, "10 MB/min")]
        [InlineData(10, 60 * 60, TimeUnit.Hour, "10 MB/h")]
        [InlineData(1, 10 * 1, TimeUnit.Second, "102,4 kB/s")]
        [InlineData(1, 10 * 60, TimeUnit.Minute, "102,4 kB/min")]
        [InlineData(1, 10 * 60 * 60, TimeUnit.Hour, "102,4 kB/h")]
        public void TimeUnitTests(long megabytes, double measurementIntervalSeconds, TimeUnit displayInterval, string expectedValue)
        {
            var size = ByteSize.FromMegabytes(megabytes);
            var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);

            var rate = size.Per(measurementInterval);
            var text = rate.Humanize(displayInterval);

            Assert.Equal(expectedValue, text);
        }

        [Theory]
        [InlineData(19854651984, 1, TimeUnit.Second, null, "18,49 GB/s")]
        [InlineData(19854651984, 1, TimeUnit.Second, "#.##", "18,49 GB/s")]
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
