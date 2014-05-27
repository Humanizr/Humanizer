using System;
using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nl
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("nl-NL") { }

        [Theory]
        [InlineData(-10, "10 dagen geleden")]
        [InlineData(-3, "3 dagen geleden")]
        [InlineData(-2, "2 dagen geleden")]
        [InlineData(-1, "gisteren")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 uur geleden")]
        [InlineData(-3, "3 uur geleden")]
        [InlineData(-2, "2 uur geleden")]
        [InlineData(-1, "één uur geleden")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 minuten geleden")]
        [InlineData(-3, "3 minuten geleden")]
        [InlineData(-2, "2 minuten geleden")]
        [InlineData(-1, "één minuut geleden")]
        [InlineData(60, "één uur geleden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 maanden geleden")]
        [InlineData(-3, "3 maanden geleden")]
        [InlineData(-2, "2 maanden geleden")]
        [InlineData(-1, "één maand geleden")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 seconden geleden")]
        [InlineData(-3, "3 seconden geleden")]
        [InlineData(-2, "2 seconden geleden")]
        [InlineData(-1, "één seconde geleden")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 jaar geleden")]
        [InlineData(-3, "3 jaar geleden")]
        [InlineData(-2, "2 jaar geleden")]
        [InlineData(-1, "één jaar geleden")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
