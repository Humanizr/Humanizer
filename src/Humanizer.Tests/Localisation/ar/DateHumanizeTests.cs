using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ar
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ar") { }


        [Theory]
        [InlineData(-1, "أمس")]
        [InlineData(-2, "منذ يومين")]
        [InlineData(-3, "منذ 3 أيام")]
        [InlineData(-11, "منذ 11 يوم")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ ساعتين")]
        [InlineData(-1, "منذ ساعة واحدة")]
        [InlineData(-3, "منذ 3 ساعات")]
        [InlineData(-11, "منذ 11 ساعة")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ دقيقتين")]
        [InlineData(-1, "منذ دقيقة واحدة")]
        [InlineData(-3, "منذ 3 دقائق")]
        [InlineData(-11, "منذ 11 دقيقة")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ شهرين")]
        [InlineData(-1, "منذ شهر واحد")]
        [InlineData(-3, "منذ 3 أشهر")]
        [InlineData(-11, "منذ 11 شهر")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ ثانيتين")]
        [InlineData(-1, "منذ ثانية واحدة")]
        [InlineData(-3, "منذ 3 ثوان")]
        [InlineData(-11, "منذ 11 ثانية")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ عامين")]
        [InlineData(-1, "العام السابق")]
        [InlineData(-3, "منذ 3 أعوام")]
        [InlineData(-11, "منذ 11 عام")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
