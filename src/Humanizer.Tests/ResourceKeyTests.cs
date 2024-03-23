#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
public class ResourceKeyTests
{
    [Theory]
    [MemberData(nameof(TimeSpanHumanizeResourceKeys))]
    public void TimeSpanHumanizeKeysGeneration(int instance, string expected, string actual) =>
        Assert.Equal(expected, actual);

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeResourceKeys))]
    public void TimeSpanHumanizeKeysExistence(int instance, string expectedResourceKey, string generatedResourceKey) =>
        Assert.NotNull(Resources.GetResource(generatedResourceKey));


    public static IEnumerable<object[]> TimeSpanHumanizeResourceKeys =>
        new[] {
            [0, "TimeSpanHumanize_SingleMillisecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond)],
            [0, "TimeSpanHumanize_SingleSecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second)],
            [0, "TimeSpanHumanize_SingleMinute", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute)],
            [0, "TimeSpanHumanize_SingleHour", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour)],
            [0, "TimeSpanHumanize_SingleDay", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day)],
            [0, "TimeSpanHumanize_SingleWeek", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week)],
            [0, "TimeSpanHumanize_SingleMonth", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month)],
            [0, "TimeSpanHumanize_SingleYear", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year)],
            [0, "TimeSpanHumanize_MultipleMilliseconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 10)],
            [0, "TimeSpanHumanize_MultipleSeconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 10)],
            [0, "TimeSpanHumanize_MultipleMinutes", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 10)],
            [0, "TimeSpanHumanize_MultipleHours", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 10)],
            [0, "TimeSpanHumanize_MultipleDays", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 10)],
            [0, "TimeSpanHumanize_MultipleWeeks", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 10)],
            [0, "TimeSpanHumanize_MultipleMonths", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 10)],
            [0, "TimeSpanHumanize_MultipleYears", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 10)],
            [0, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 0, true)],
            [1, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 0, true)],
            [2, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 0, true)],
            [3, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 0, true)],
            [4, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 0, true)],
            [5, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 0, true)],
            [6, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 0, true)],
            [7, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 0, true)],
            [1, "TimeSpanHumanize_MultipleMilliseconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 0)],
            [2, "TimeSpanHumanize_MultipleSeconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 0)],
            [3, "TimeSpanHumanize_MultipleMinutes", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 0)],
            [4, "TimeSpanHumanize_MultipleHours", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 0)],
            [5, "TimeSpanHumanize_MultipleDays", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 0)],
            [6, "TimeSpanHumanize_MultipleWeeks", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 0)],
            [7, "TimeSpanHumanize_MultipleMonths", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 0)],
            new object[]{8, "TimeSpanHumanize_MultipleYears", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 0) }
        };
}
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters