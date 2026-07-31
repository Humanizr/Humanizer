[UseCulture("en")]
public class ByteRateTests
{
    [Theory]
    [InlineData(400, 1, "400 B/s")]
    [InlineData(4 * 1024, 1, "4 KB/s")]
    [InlineData(4 * 1024 * 1024, 1, "4 MB/s")]
    [InlineData(4 * 2 * 1024 * 1024, 2, "4 MB/s")]
    [InlineData(4 * 1024, 0.1, "40 KB/s")]
    [InlineData(15 * 60 * 1024 * 1024, 60, "15 MB/s")]
    public void HumanizesRates(long inputBytes, double perSeconds, string expectedValue)
    {
        var size = new ByteSize(inputBytes);
        var interval = TimeSpan.FromSeconds(perSeconds);

        var rate = size
            .Per(interval)
            .Humanize();

        Assert.Equal(expectedValue, rate);
    }

    [Theory]
    [InlineData(1, 1, TimeUnit.Second, "1 MB/s")]
    [InlineData(1, 60, TimeUnit.Minute, "1 MB/min")]
    [InlineData(1, 60 * 60, TimeUnit.Hour, "1 MB/h")]
    [InlineData(10, 1, TimeUnit.Second, "10 MB/s")]
    [InlineData(10, 60, TimeUnit.Minute, "10 MB/min")]
    [InlineData(10, 60 * 60, TimeUnit.Hour, "10 MB/h")]
    [InlineData(1, 10 * 1, TimeUnit.Second, "102.4 KB/s")]
    [InlineData(1, 10 * 60, TimeUnit.Minute, "102.4 KB/min")]
    [InlineData(1, 10 * 60 * 60, TimeUnit.Hour, "102.4 KB/h")]
    public void TimeUnitTests(long megabytes, double measurementIntervalSeconds, TimeUnit displayInterval, string expectedValue)
    {
        var size = ByteSize.FromMegabytes(megabytes);
        var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);

        var rate = size.Per(measurementInterval);
        var text = rate.Humanize(displayInterval);

        Assert.Equal(expectedValue, text);
    }

    [Theory]
    [InlineData(19854651984, 1, TimeUnit.Second, null, "18.49 GB/s")]
    [InlineData(19854651984, 1, TimeUnit.Second, "#.##", "18.49 GB/s")]
    public void FormattedTimeUnitTests(long bytes, int measurementIntervalSeconds, TimeUnit displayInterval, string? format, string expectedValue)
    {
        var size = ByteSize.FromBytes(bytes);
        var measurementInterval = TimeSpan.FromSeconds(measurementIntervalSeconds);
        var rate = size.Per(measurementInterval);
        var text = rate.Humanize(format, displayInterval);

        Assert.Equal(expectedValue, text);
    }

    [Fact]
    public void ToStringReturnsHumanizedRate() =>
        Assert.Equal("400 B/s", ByteSize.FromBytes(400).Per(TimeSpan.FromSeconds(1)).ToString());

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void ExplicitUnitSystemsPreserveExactIdentityAndIntegralBitScaling(ByteSizeUnitSystem unitSystem)
    {
        var exactSize = ByteSize.ParseWithUnitSystem(
            "9007199254740993 b",
            unitSystem,
            CultureInfo.InvariantCulture);

        Assert.Equal(
            "9007199254740993 b/s",
            exactSize
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(unitSystem, "0 b"));
        Assert.Equal(
            "4503599627370497 b/s",
            exactSize
                .Per(TimeSpan.FromSeconds(2))
                .HumanizeWithUnitSystem(unitSystem, "0 b"));
        Assert.Equal(
            $"{long.MaxValue} b/s",
            ByteSize.ParseWithUnitSystem($"{long.MaxValue} b", unitSystem, CultureInfo.InvariantCulture)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(unitSystem, "0 b"));
        Assert.Equal(
            $"{long.MinValue} b/s",
            ByteSize.MinValue
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(unitSystem, "0 b"));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void ExplicitBitRatesPreserveExactDirectInputs(ByteSizeUnitSystem unitSystem)
    {
        var size = ByteSize.FromBits(-1);
        var rate = size.Per(TimeSpan.FromSeconds(0.5));

        Assert.Equal(-1, size.Bits);
        Assert.Equal("-2 b/s", rate.HumanizeWithUnitSystem(unitSystem, "0 b"));
        Assert.Equal("-0.25 B/s", rate.HumanizeWithUnitSystem(unitSystem, "0.## B"));
        Assert.Equal(
            "0 b/s",
            size.Per(TimeSpan.FromSeconds(2)).HumanizeWithUnitSystem(unitSystem));
    }

    [Theory]
    [InlineData(-9007199254740993, 2, "-4503599627370496 b/s")]
    [InlineData(9007199254740993, -2, "-4503599627370496 b/s")]
    [InlineData(-9007199254740993, -2, "4503599627370497 b/s")]
    public void ExplicitUnitSystemsApplySignedRationalCeiling(long bits, int intervalSeconds, string expected) =>
        Assert.Equal(
            expected,
            ByteSize.ParseWithUnitSystem(
                    $"{bits} b",
                    ByteSizeUnitSystem.DecimalSi,
                    CultureInfo.InvariantCulture)
                .Per(TimeSpan.FromSeconds(intervalSeconds))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, "0 b"));

    [Fact]
    public void ExplicitUnitSystemsPreserveLegacyFallbackForUndefinedOrOutOfRangeRates()
    {
        verifyFallback(ByteSize.FromBits(1), TimeSpan.Zero);
        verifyFallback(ByteSize.MaxValue, TimeSpan.FromTicks(1));

        static void verifyFallback(ByteSize size, TimeSpan interval)
        {
            var displayInterval = TimeSpan.FromSeconds(1);
            var scaledBytes = size.Bytes / interval.TotalSeconds * displayInterval.TotalSeconds;
            var expected = new ByteSize(scaledBytes)
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, "0 b") + "/s";

            Assert.Equal(
                expected,
                size.Per(interval).HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, "0 b"));
        }
    }

    [Theory]
    [InlineData(400, 10, 800, 20, 0)]
    [InlineData(400, 10, 800, 10, -1)]
    [InlineData(800, 10, 400, 10, 1)]
    public void ComparesNormalizedRates(
        long leftBytes,
        int leftIntervalSeconds,
        long rightBytes,
        int rightIntervalSeconds,
        int expected)
    {
        var left = ByteSize.FromBytes(leftBytes).Per(TimeSpan.FromSeconds(leftIntervalSeconds));
        var right = ByteSize.FromBytes(rightBytes).Per(TimeSpan.FromSeconds(rightIntervalSeconds));

        Assert.Equal(expected, left.CompareTo(right));
    }

    [Fact]
    public void EqualsNormalizedRates()
    {
        var left = ByteSize.FromBytes(400).Per(TimeSpan.FromSeconds(10));
        var right = ByteSize.FromBytes(800).Per(TimeSpan.FromSeconds(20));

        Assert.Equal(left, right);
        Assert.True(left.Equals((object)right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void EqualityAndComparisonDistinguishRuntimeTypes()
    {
        var rate = ByteSize.FromBytes(400).Per(TimeSpan.FromSeconds(10));
        var derivedRate = new DerivedByteRate(ByteSize.FromBytes(800), TimeSpan.FromSeconds(20));
        var equivalentDerivedRate = new DerivedByteRate(ByteSize.FromBytes(400), TimeSpan.FromSeconds(10));

        Assert.False(rate.Equals(derivedRate));
        Assert.False(derivedRate.Equals(rate));
        Assert.False(EqualityComparer<ByteRate>.Default.Equals(rate, derivedRate));
        Assert.False(EqualityComparer<ByteRate>.Default.Equals(derivedRate, rate));
        Assert.False(rate.Equals((object)derivedRate));
        Assert.False(derivedRate.Equals((object)rate));
        var comparison = rate.CompareTo(derivedRate);
        Assert.NotEqual(0, comparison);
        Assert.Equal(-Math.Sign(comparison), Math.Sign(derivedRate.CompareTo(rate)));
        Assert.Equal(comparison, ((IComparable)rate).CompareTo(derivedRate));
        Assert.Equal(2, new SortedSet<ByteRate> { rate, derivedRate }.Count);
        Assert.True(derivedRate.Equals(equivalentDerivedRate));
        Assert.True(derivedRate.Equals((object)equivalentDerivedRate));
        Assert.Equal(0, derivedRate.CompareTo(equivalentDerivedRate));
    }

    [Fact]
    public void DoesNotEqualDifferentOrNullRates()
    {
        var rate = ByteSize.FromBytes(400).Per(TimeSpan.FromSeconds(10));
        var other = ByteSize.FromBytes(800).Per(TimeSpan.FromSeconds(10));

        Assert.NotEqual(rate, other);
        Assert.False(rate.Equals(null));
        Assert.False(rate.Equals((object?)null));
        Assert.False(rate.Equals(new object()));
    }

    [Fact]
    public void UntypedComparisonRejectsOtherTypes()
    {
        var rate = ByteSize.FromBytes(400).Per(TimeSpan.FromSeconds(10));

        Assert.Equal(1, rate.CompareTo(null));
        Assert.Equal(1, ((IComparable)rate).CompareTo(null));
        Assert.Throws<ArgumentException>(() => ((IComparable)rate).CompareTo(40d));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond)]
    [InlineData(TimeUnit.Day)]
    [InlineData(TimeUnit.Month)]
    [InlineData(TimeUnit.Week)]
    [InlineData(TimeUnit.Year)]
    public void ThrowsOnUnsupportedData(TimeUnit units)
    {
        var dummyRate = ByteSize
            .FromBits(1)
            .Per(TimeSpan.FromSeconds(1));

        Assert.Throws<NotSupportedException>(() => dummyRate.Humanize(units));
    }

    sealed class DerivedByteRate(ByteSize size, TimeSpan interval) : ByteRate(size, interval);
}