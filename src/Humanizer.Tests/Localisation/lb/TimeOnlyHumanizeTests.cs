#if NET6_0_OR_GREATER

namespace lb;

[UseCulture("lb-LU")]
public class TimeOnlyHumanizeTests
{
    [Fact]
    public void DefaultStrategy_SameTime()
    {
        var inputTime = new TimeOnly(13, 07, 05);
        var baseTime = new TimeOnly(13, 07, 05);

        const string expectedResult = "elo";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_HoursApart()
    {
        var inputTime = new TimeOnly(3, 08, 05);
        var baseTime = new TimeOnly(1, 08, 05);

        const string expectedResult = "an 2 Stonnen";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_HoursAgo()
    {
        var inputTime = new TimeOnly(13, 07, 02);
        var baseTime = new TimeOnly(17, 07, 05);

        const string expectedResult = "viru 4 Stonnen";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void PrecisionStrategy_NextDay()
    {
        var inputTime = new TimeOnly(18, 10, 49);
        var baseTime = new TimeOnly(13, 07, 04);

        const string expectedResult = "a 5 Stonnen";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Never()
    {
        TimeOnly? never = null;
        Assert.Equal("ni", never.Humanize());
    }

    [Fact]
    public void Nullable_ExpectSame()
    {
        TimeOnly? never = new TimeOnly(23, 12, 7);
        Assert.Equal(never.Value.Humanize(), never.Humanize());
    }
}

#endif
