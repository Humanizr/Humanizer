namespace fiFI;

[UseCulture("fi-FI")]
public class TimeSpanHumanizeTests
{
    [Fact]
    public void OneDay() =>
        Assert.Equal("päivä", TimeSpan.FromDays(1).Humanize());

    [Fact]
    public void OneHour() =>
        Assert.Equal("tunti", TimeSpan.FromHours(1).Humanize());

    [Fact]
    public void NoTime() =>
        Assert.Equal("nyt", TimeSpan.Zero.Humanize(toWords: true));

    [Theory]
    [InlineData(14, "2 viikkoa vanha")]
    [InlineData(366, "vuosi vanha")]
    public void Age(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).ToAge());
}
