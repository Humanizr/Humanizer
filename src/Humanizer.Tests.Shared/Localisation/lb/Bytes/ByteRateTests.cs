namespace Humanizer.Tests.Localisation.lb.Bytes;

[UseCulture("lb-LU")]
public class ByteRateTests
{
    [Theory]
    [InlineData(400, 1, "400 B/s")]
    [InlineData(4 * 1000, 1, "4 KB/s")]
    [InlineData(4 * 1000 * 1000, 1, "4 MB/s")]
    [InlineData(4 * 2 * 1000 * 1000, 2, "4 MB/s")]
    [InlineData(4 * 1000, 0.1, "40 KB/s")]
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
    [InlineData(1, 60 * 60, TimeUnit.Hour, "1 MB/h")]
    [InlineData(10, 1, TimeUnit.Second, "10 MB/s")]
    [InlineData(10, 60, TimeUnit.Minute, "10 MB/min")]
    [InlineData(10, 60 * 60, TimeUnit.Hour, "10 MB/h")]
    [InlineData(1, 10 * 1, TimeUnit.Second, "100 KB/s")]
    [InlineData(1, 10 * 60, TimeUnit.Minute, "100 KB/min")]
    [InlineData(1, 10 * 60 * 60, TimeUnit.Hour, "100 KB/h")]
    public void TimeUnitTests(long megabytes, double measurementIntervalSeconds, TimeUnit displayInterval, string expectedValue)
    {
        var size = ByteSize.FromMegabytes(megabytes);
        var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);

        var rate = size.Per(measurementInterval);
        var text = rate.Humanize(displayInterval);

        Assert.Equal(expectedValue, text);
    }

    [Theory]
    [InlineData(19854651984, 1, TimeUnit.Second, null, "19,85 GB/s")]
    [InlineData(19854651984, 1, TimeUnit.Second, "#.##", "19,85 GB/s")]
    [InlineData(19854651984, 1, TimeUnit.Second, "#.## GiB", "18,49 GiB/s")]
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
    public void ThrowsOnUnsupportedData(TimeUnit units)
    {
        var dummyRate = ByteSize.FromBits(1).Per(TimeSpan.FromSeconds(1));

        Assert.Throws<NotSupportedException>(() =>
        {
            dummyRate.Humanize(units);
        });
    }
}
