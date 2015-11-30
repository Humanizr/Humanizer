using System.Collections.Generic;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests
{
    public class ResourceKeyTests
    {
        [Theory]
        [MemberData("DateHumanizeResourceKeys")]
        public void DateHumanizeKeysGeneration(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData("TimeSpanHumanizeResourceKeys")]
        public void TimeSpanHumanizeKeysGeneration(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData("DateHumanizeResourceKeys")]
        public void DateHumanizeKeysExistence(string expectedResourceKey, string generatedResourceKey)
        {
            Assert.NotNull(Resources.GetResource(generatedResourceKey));
        }

        [Theory]
        [MemberData("TimeSpanHumanizeResourceKeys")]
        public void TimeSpanHumanizeKeysExistence(string expectedResourceKey, string generatedResourceKey)
        {
            Assert.NotNull(Resources.GetResource(generatedResourceKey));
        }

        public static IEnumerable<object[]> DateHumanizeResourceKeys
        {
            get
            {
                return new[] {
                    new object[]{ "DateHumanize_SingleSecondAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past) },
                    new object[]{ "DateHumanize_SingleMinuteAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past) },
                    new object[]{ "DateHumanize_SingleHourAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past) },
                    new object[]{ "DateHumanize_SingleDayAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past) },
                    new object[]{ "DateHumanize_SingleMonthAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past) },
                    new object[]{ "DateHumanize_SingleYearAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past) },
                    new object[]{ "DateHumanize_MultipleSecondsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past, count: 10) },
                    new object[]{ "DateHumanize_MultipleMinutesAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past, count: 10) },
                    new object[]{ "DateHumanize_MultipleHoursAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past, count: 10) },
                    new object[]{ "DateHumanize_MultipleDaysAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past, count: 10) },
                    new object[]{ "DateHumanize_MultipleMonthsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past, count: 10) },
                    new object[]{ "DateHumanize_MultipleYearsAgo", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past, count: 10) },

                    new object[]{ "DateHumanize_SingleSecondFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_SingleMinuteFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_SingleHourFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_SingleDayFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_SingleMonthFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_SingleYearFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, timeUnitTense: Tense.Future, count: 1) },
                    new object[]{ "DateHumanize_MultipleSecondsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{ "DateHumanize_MultipleMinutesFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{ "DateHumanize_MultipleHoursFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{ "DateHumanize_MultipleDaysFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{ "DateHumanize_MultipleMonthsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, timeUnitTense: Tense.Future, count: 10) },
                    new object[]{ "DateHumanize_MultipleYearsFromNow", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, timeUnitTense: Tense.Future, count: 10) },

                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Millisecond, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Second, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Minute, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Hour, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Day, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Week, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Month, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.GetResourceKey(TimeUnit.Year, Tense.Past, count: 0) },
                    new object[]{ "DateHumanize_Now", ResourceKeys.DateHumanize.Now }
                };
            }
        }

        public static IEnumerable<object[]> TimeSpanHumanizeResourceKeys
        {
            get
            {
                return new[] {
                    new object[]{ "TimeSpanHumanize_SingleMillisecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond) },
                    new object[]{ "TimeSpanHumanize_SingleSecond", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second) },
                    new object[]{ "TimeSpanHumanize_SingleMinute", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute) },
                    new object[]{ "TimeSpanHumanize_SingleHour", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour) },
                    new object[]{ "TimeSpanHumanize_SingleDay", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day) },
                    new object[]{ "TimeSpanHumanize_SingleWeek", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week) },
                    new object[]{ "TimeSpanHumanize_SingleMonth", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month) },
                    new object[]{ "TimeSpanHumanize_SingleYear", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year) },
                    new object[]{ "TimeSpanHumanize_MultipleMilliseconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleSeconds", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleMinutes", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleHours", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleDays", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleWeeks", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleMonths", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 10) },
                    new object[]{ "TimeSpanHumanize_MultipleYears", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 10) },

                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Millisecond, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Second, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Minute, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Hour, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Day, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Week, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Month, 0) },
                    new object[]{ "TimeSpanHumanize_Zero", ResourceKeys.TimeSpanHumanize.GetResourceKey(TimeUnit.Year, 0) }
                };
            }
        }
    }
}
