#if NET6_0_OR_GREATER

[UseCulture("en-US")]
public class TimeOnlyHumanizeTests
{
    [Fact]
    public void DefaultStrategy_SameTime()
    {
        var inputTime = new TimeOnly(13, 07, 05);
        var baseTime = new TimeOnly(13, 07, 05);

        const string expectedResult = "now";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_HoursApart()
    {
        var inputTime = new TimeOnly(13, 08, 05);
        var baseTime = new TimeOnly(1, 08, 05);

        const string expectedResult = "12 hours from now";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task StrategiesAreIsolatedAcrossParallelCultures()
    {
        using var strategiesReady = new Barrier(2);

        var defaultResult = Task.Run(() => Humanize(
            new("en-US"),
            new("fr"),
            new DefaultTimeOnlyHumanizeStrategy()));
        var precisionResult = Task.Run(() => Humanize(
            new("fr"),
            new("is"),
            new PrecisionTimeOnlyHumanizeStrategy(0.5)));

        var results = await Task.WhenAll(defaultResult, precisionResult);

        Assert.Equal("12 hours from now", results[0]);
        Assert.Equal("demain", results[1]);

        string Humanize(
            CultureInfo currentCulture,
            CultureInfo currentUICulture,
            ITimeOnlyHumanizeStrategy strategy)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentUICulture;
                if (!strategiesReady.SignalAndWait(TimeSpan.FromSeconds(30)))
                {
                    throw new TimeoutException("Parallel strategies did not synchronize.");
                }

                return strategy.Humanize(new(13, 08, 05), new(1, 08, 05), null);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }

    [Fact]
    public void DefaultStrategy_HoursAgo()
    {
        var inputTime = new TimeOnly(13, 07, 02);
        var baseTime = new TimeOnly(17, 07, 05);

        const string expectedResult = "4 hours ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void PrecisionStrategy_NextDay()
    {
        var inputTime = new TimeOnly(18, 10, 49);
        var baseTime = new TimeOnly(13, 07, 04);

        const string expectedResult = "5 hours from now";
        var actualResult = new PrecisionTimeOnlyHumanizeStrategy(0.75)
            .Humanize(inputTime, baseTime, CultureInfo.CurrentUICulture);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void Never()
    {
        TimeOnly? never = null;
        Assert.Equal("never", never.Humanize());
    }

    [Fact]
    public void Nullable_ExpectSame()
    {
        TimeOnly? never = new TimeOnly(23, 12, 7);

        Assert.Equal(never.Value.Humanize(), never.Humanize());
    }
}

#endif