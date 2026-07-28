public class TimeSpanDehumanizeTests
{
    [Theory]
    [InlineData("50ms", 500000)]
    [InlineData("1.5s", 15000000)]
    [InlineData(".5m", 300000000)]
    [InlineData("2h", 72000000000)]
    [InlineData("1d", 864000000000)]
    [InlineData("1w", 6048000000000)]
    [InlineData("1h30m15.25s", 54152500000)]
    [InlineData("  - 1 h 30 m  ", -54000000000)]
    [InlineData("+1h", 36000000000)]
    [InlineData("1s 2h 1s", 72020000000)]
    [InlineData("-922337203685477.5808ms", long.MinValue)]
    public void ParsesCompactDurations(string input, long expectedTicks) =>
        Assert.Equal(TimeSpan.FromTicks(expectedTicks), input.DehumanizeTimeSpan());

    [Theory]
    [InlineData("15:10", 546000000000)]
    [InlineData("00:15:10", 9100000000)]
    [InlineData("1.02:03:04.0050060", 937840050060)]
    public void UsesStandardTimeSpanInterpretation(string input, long expectedTicks) =>
        Assert.Equal(TimeSpan.FromTicks(expectedTicks), input.DehumanizeTimeSpan());

    [Fact]
    public void StandardTimeSpanRoundTrips()
    {
        TimeSpan[] values =
        [
            TimeSpan.Zero,
            new(1, 2, 3, 4, 5, 6),
            TimeSpan.FromTicks(-123456789),
            TimeSpan.MaxValue,
            TimeSpan.MinValue
        ];

        foreach (var value in values)
        {
            var text = value.ToString("c", CultureInfo.InvariantCulture);
            Assert.Equal(value, text.DehumanizeTimeSpan());
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1.00000001ms")]
    [InlineData("1,5h")]
    [InlineData("1e3s")]
    [InlineData("1M")]
    [InlineData("1mo")]
    [InlineData("1y")]
    [InlineData("1 hour")]
    [InlineData("one hour")]
    [InlineData("1h, 2m")]
    [InlineData("1h-2m")]
    [InlineData("--1h")]
    [InlineData("9223372036854775808s")]
    [InlineData("922337203685477.5808ms")]
    [InlineData("0.00000010000000000000000000001s")]
    [InlineData("0.00000000000000000000000000001ms")]
    [InlineData("922337203685.47758070000000000001s")]
    [InlineData("-922337203685.47758080000000000001s")]
    [InlineData("922337203685.4775807s 0.0000000000000000000000000001s")]
    [InlineData("-922337203685.4775808s 0.0000000000000000000000000001s")]
    [InlineData("999999.999999999994708994709w")]
    [InlineData("-999999.999999999994708994709w")]
    [InlineData("9999999.99999999997337962963d")]
    public void RejectsUnsupportedDurations(string input)
    {
        Assert.False(input.TryDehumanizeTimeSpan(out var result));
        Assert.Equal(default, result);
        Assert.Throws<FormatException>(() => input.DehumanizeTimeSpan());
    }

    [Fact]
    public void TryReturnsFalseForNull()
    {
        Assert.False(((string?)null).TryDehumanizeTimeSpan(out var result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void ThrowingApiRejectsNull() =>
        Assert.Throws<ArgumentNullException>(() => ((string)null!).DehumanizeTimeSpan());

    [Fact]
    [UseCulture("de-DE")]
    public void CompactDecimalsAreInvariant()
    {
        Assert.Equal(TimeSpan.FromMinutes(90), "1.5h".DehumanizeTimeSpan());
        Assert.False("1,5h".TryDehumanizeTimeSpan(out _));
    }
}