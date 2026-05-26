[UseCulture("en-US")]
public class TimeSpanDehumanizeTests
{
    static readonly TimeSpan ThreeHoursEighteenMinutes = new(3, 18, 0);

    [Theory]
    [InlineData("3h18m")]
    [InlineData("3h 18m")]
    [InlineData("3.3hrs")]
    [InlineData("3:18:00")]
    [InlineData("3:18")]
    [InlineData("12m 30s")]
    [InlineData("1000s")]
    [InlineData("6d")]
    public void TryDehumanizeTimeSpanParsesCompactFormats(string input)
    {
        Assert.True(input.TryDehumanizeTimeSpan(out var parsed));
        Assert.True(parsed > TimeSpan.Zero);
    }

    [Fact]
    public void DehumanizeTimeSpanParsesThreeHoursEighteenMinutesFromCompactUnits()
    {
        Assert.Equal(ThreeHoursEighteenMinutes, "3h18m".DehumanizeTimeSpan());
        Assert.Equal(ThreeHoursEighteenMinutes, "3h 18m".DehumanizeTimeSpan());
    }

    [Fact]
    public void DehumanizeTimeSpanParsesFractionalHours()
    {
        Assert.Equal(ThreeHoursEighteenMinutes, "3.3hrs".DehumanizeTimeSpan());
    }

    [Fact]
    public void DehumanizeTimeSpanParsesCompoundMinutesAndSeconds()
    {
        Assert.Equal(new TimeSpan(0, 12, 30), "12m 30s".DehumanizeTimeSpan());
    }

    [Fact]
    public void DehumanizeTimeSpanParsesThreeHoursEighteenMinutesFromColonFormats()
    {
        Assert.Equal(ThreeHoursEighteenMinutes, "3:18:00".DehumanizeTimeSpan());
        Assert.Equal(ThreeHoursEighteenMinutes, "3:18".DehumanizeTimeSpan());
    }

    [Fact]
    public void DehumanizeTimeSpanParsesMinutesAndSecondsWhenConfigured()
    {
        var options = new TimeSpanDehumanizeOptions
        {
            ColonFormat = TimeSpanDehumanizeColonFormat.MinutesSeconds,
        };

        Assert.Equal(new TimeSpan(0, 18, 1), "18:01".DehumanizeTimeSpan(options));
    }

    [Theory]
    [InlineData("10675200d")]
    public void TryDehumanizeTimeSpanReturnsFalseForOverflowInput(string input)
    {
        Assert.False(input.TryDehumanizeTimeSpan(out _));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("not-a-duration")]
    [InlineData("3x")]
    public void TryDehumanizeTimeSpanReturnsFalseForInvalidInput(string input)
    {
        Assert.False(input.TryDehumanizeTimeSpan(out _));
    }

    [Fact]
    public void DehumanizeTimeSpanThrowsForInvalidInput()
    {
        Assert.Throws<FormatException>(() => "not-a-duration".DehumanizeTimeSpan());
    }

    [Fact]
    public void DehumanizeTimeSpanThrowsForNullInput()
    {
        Assert.Throws<ArgumentNullException>(() => TimeSpanDehumanizeExtensions.DehumanizeTimeSpan(null!));
    }
}
