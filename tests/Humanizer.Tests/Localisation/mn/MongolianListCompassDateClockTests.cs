namespace Humanizer.Tests.Localisation.mn;

[UseCulture("mn")]
public class MongolianListCompassDateClockTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesBolon()
    {
        Assert.Equal("1 болон 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndBolon()
    {
        Assert.Equal("1, 2 болон 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(0.0, "хойд")]
    [InlineData(45.0, "зүүн хойд")]
    [InlineData(90.0, "зүүн")]
    [InlineData(180.0, "өмнөд")]
    [InlineData(270.0, "баруун")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "Х")]
    [InlineData(90.0, "З")]
    [InlineData(180.0, "Ө")]
    [InlineData(270.0, "Б")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }

    [Theory]
    [InlineData(2022, 1, 25, "2022 оны нэгдүгээр сарын 25")]
    [InlineData(2015, 2, 3, "2015 оны хоёрдугаар сарын 3")]
    public void DateTimeToOrdinalWords_UsesMongolianDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "2022 оны нэгдүгээр сарын 25")]
    [InlineData(2015, 2, 3, "2015 оны хоёрдугаар сарын 3")]
    public void DateOnlyToOrdinalWords_UsesMongolianDatePattern(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 5, "нэг цаг таван минут")]
    [InlineData(13, 23, "арван гурван цаг хорин гурван минут")]
    [InlineData(20, 0, "хорин цаг")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("арван гурван цаг хорин таван минут", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif
}