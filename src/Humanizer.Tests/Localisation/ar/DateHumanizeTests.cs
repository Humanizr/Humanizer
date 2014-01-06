using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ar
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ar") { }

        [Theory]
        [InlineData(-2, "منذ يومين")]
        [InlineData(-1, "أمس")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ ساعتين")]
        [InlineData(-1, "منذ ساعة واحدة")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ دقيقتين")]
        [InlineData(-1, "منذ دقيقة واحدة")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ شهرين")]
        [InlineData(-1, "منذ شهر واحد")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ ثانيتين")]
        [InlineData(-1, "منذ ثانية واحدة")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-2, "منذ سنتين")]
        [InlineData(-1, "العام السابق")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }

        [Fact]
        public void NotYet()
        {
            Assert.Equal("ليس بعد", DateTime.UtcNow.AddDays(1).Humanize());
        }
    }
}
