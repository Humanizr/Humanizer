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

    [Fact]
    public void DehumanizeTimeSpanParsesNegativeMinutesAndSecondsWhenConfigured()
    {
        var options = new TimeSpanDehumanizeOptions
        {
            ColonFormat = TimeSpanDehumanizeColonFormat.MinutesSeconds,
        };

        Assert.Equal(new TimeSpan(0, -18, -1), "-18:01".DehumanizeTimeSpan(options));
    }

    [Fact]
    public void DehumanizeTimeSpanParsesThreeHoursEighteenMinutesFromWordPhrases()
    {
        Assert.Equal(ThreeHoursEighteenMinutes, "3 hours 18 minutes".DehumanizeTimeSpan());
        Assert.Equal(ThreeHoursEighteenMinutes, "3 hours, 18 minutes".DehumanizeTimeSpan());
        Assert.Equal(ThreeHoursEighteenMinutes, "3 hours and 18 minutes".DehumanizeTimeSpan());
    }

    [Theory]
    [InlineData(0, 3, 18, 0, 2)]
    [InlineData(0, 0, 12, 30, 2)]
    [InlineData(0, 2, 0, 0, 1)]
    [InlineData(14, 0, 0, 0, 1)]
    public void DehumanizeTimeSpanRoundTripsHumanizeOutput(int days, int hours, int minutes, int seconds, int precision)
    {
        var original = new TimeSpan(days, hours, minutes, seconds);
        var humanized = original.Humanize(precision: precision);

        Assert.Equal(original, humanized.DehumanizeTimeSpan());
    }

    [Fact]
    public void TryDehumanizeTimeSpanReturnsFalseForOverflowInput()
    {
        Assert.False("10675200d".TryDehumanizeTimeSpan(out _));
    }

    [Fact]
    public void DehumanizeTimeSpanParsesNegativeColonFormatViaTryParseFallback()
    {
        Assert.Equal(new TimeSpan(-3, -18, 0), "-3:18:00".DehumanizeTimeSpan());
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
