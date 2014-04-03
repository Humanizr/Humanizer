using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fa
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("fa") { }

        [Theory]
        [InlineData(1, "فردا")]
        [InlineData(13, "13 روز بعد")]
        public void DaysFromNow(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-1, "دیروز")]
        [InlineData(-11, "11 روز پیش")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "یک ساعت بعد")]
        [InlineData(11, "11 ساعت بعد")]
        public void HoursFromNow(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-1, "یک ساعت پیش")]
        [InlineData(-11, "11 ساعت پیش")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "یک دقیقه بعد")]
        [InlineData(13, "13 دقیقه بعد")]
        public void MinutesFromNow(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-1, "یک دقیقه پیش")]
        [InlineData(-13, "13 دقیقه پیش")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "یک ماه بعد")]
        [InlineData(10, "10 ماه بعد")]
        public void MonthsFromNow(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-1, "یک ماه پیش")]
        [InlineData(-10, "10 ماه پیش")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(1, "یک ثانیه بعد")]
        [InlineData(11, "11 ثانیه بعد")]
        public void SecondsFromNow(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-1, "یک ثانیه پیش")]
        [InlineData(-11, "11 ثانیه پیش")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "یک سال بعد")]
        [InlineData(21, "21 سال بعد")]
        public void YearsFromNow(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }

        [Theory]
        [InlineData(-1, "یک سال پیش")]
        [InlineData(-21, "21 سال پیش")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
