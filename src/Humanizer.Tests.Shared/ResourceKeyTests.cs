using System.Collections.Generic;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public class ResourceKeyTests
    {
        [Theory]
        [MemberData(nameof(DateHumanizeResourceKeys))]
        public void DateHumanizeKeysGeneration(int instance, string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(TimeSpanHumanizeResourceKeys))]
        public void TimeSpanHumanizeKeysGeneration(int instance, string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(DateHumanizeResourceKeys))]
        public void DateHumanizeKeysExistence(int instance, string expectedResourceKey, string generatedResourceKey)
        {
            Assert.NotNull(Resources.GetResource(generatedResourceKey));
        }

        [Theory]
        [MemberData(nameof(TimeSpanHumanizeResourceKeys))]
        public void TimeSpanHumanizeKeysExistence(int instance, string expectedResourceKey, string generatedResourceKey)
        {
            Assert.NotNull(Resources.GetResource(generatedResourceKey));
        }

        public static IEnumerable<object[]> DateHumanizeResourceKeys
        {
            get
            {
                return new[] {
                    new object[]{0, "DateHumanize_SingleSecondAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past) },
                    new object[]{0, "DateHumanize_SingleMinuteAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past) },
                    new object[]{0, "DateHumanize_SingleHourAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past) },
                    new object[]{0, "DateHumanize_SingleDayAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past) },
                    new object[]{0, "DateHumanize_SingleMonthAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past) },
                    new object[]{0, "DateHumanize_SingleYearAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past) },
                    new object[]{0, "DateHumanize_MultipleSecondsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_MultipleMinutesAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_MultipleHoursAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_MultipleDaysAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_MultipleMonthsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_MultipleYearsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past, count: 10) },
                    new object[]{0, "DateHumanize_SingleSecondFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_SingleMinuteFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_SingleHourFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_SingleDayFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_SingleMonthFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_SingleYearFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{0, "DateHumanize_MultipleSecondsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{0, "DateHumanize_MultipleMinutesFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{0, "DateHumanize_MultipleHoursFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{0, "DateHumanize_MultipleDaysFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{0, "DateHumanize_MultipleMonthsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{0, "DateHumanize_MultipleYearsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, timeUnitTense: Tense.Future, count: 10) },

                    new object[]{0, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Millisecond, Tense.Past, count: 0) },
                    new object[]{1, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past, count: 0) },
                    new object[]{2, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past, count: 0) },
                    new object[]{3, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past, count: 0) },
                    new object[]{4, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past, count: 0) },
                    new object[]{5, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Week, Tense.Past, count: 0) },
                    new object[]{6, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past, count: 0) },
                    new object[]{7, "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past, count: 0) },
                    new object[]{8, "DateHumanize_Now", ResourceKeys.DateHumanize.Now }
                };
            }
        }

        public static IEnumerable<object[]> TimeSpanHumanizeResourceKeys
        {
            get
            {
                return new[] {
                    new object[]{0, "TimeSpanHumanize_SingleMillisecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond) },
                    new object[]{0, "TimeSpanHumanize_SingleSecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second) },
                    new object[]{0, "TimeSpanHumanize_SingleMinute", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute) },
                    new object[]{0, "TimeSpanHumanize_SingleHour", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour) },
                    new object[]{0, "TimeSpanHumanize_SingleDay", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day) },
                    new object[]{0, "TimeSpanHumanize_SingleWeek", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week) },
                    new object[]{0, "TimeSpanHumanize_SingleMonth", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month) },
                    new object[]{0, "TimeSpanHumanize_SingleYear", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year) },
                    new object[]{0, "TimeSpanHumanize_MultipleMilliseconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleSeconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleMinutes", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleHours", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleDays", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleWeeks", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleMonths", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 10) },
                    new object[]{0, "TimeSpanHumanize_MultipleYears", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 10) },

                    new object[]{0, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 0, true) },
                    new object[]{1, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 0, true) },
                    new object[]{2, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 0, true) },
                    new object[]{3, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 0, true) },
                    new object[]{4, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 0, true) },
                    new object[]{5, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 0, true) },
                    new object[]{6, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 0, true) },
                    new object[]{7, "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 0, true) },

                    new object[]{1, "TimeSpanHumanize_MultipleMilliseconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 0) },
                    new object[]{2, "TimeSpanHumanize_MultipleSeconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 0) },
                    new object[]{3, "TimeSpanHumanize_MultipleMinutes", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 0) },
                    new object[]{4, "TimeSpanHumanize_MultipleHours", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 0) },
                    new object[]{5, "TimeSpanHumanize_MultipleDays", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 0) },
                    new object[]{6, "TimeSpanHumanize_MultipleWeeks", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 0) },
                    new object[]{7, "TimeSpanHumanize_MultipleMonths", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 0) },
                    new object[]{8, "TimeSpanHumanize_MultipleYears", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 0) }
                };
            }
        }
    }
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
}
