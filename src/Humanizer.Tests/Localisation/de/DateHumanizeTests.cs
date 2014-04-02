using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.de
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("de-DE") {}

        [Theory]
        [InlineData(-10, "vor 10 Tagen")]
        [InlineData(-3, "vor 3 Tagen")]
        [InlineData(-2, "vor 2 Tagen")]
        [InlineData(-1, "gestern")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Tagen")]
        [InlineData(1, "morgen")]
        public void InDays(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-10, "vor 10 Stunden")]
        [InlineData(-3, "vor 3 Stunden")]
        [InlineData(-2, "vor 2 Stunden")]
        [InlineData(-1, "vor einer Stunde")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Stunden")]
        [InlineData(1, "in einer Stunde")]
        public void InHours(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-10, "vor 10 Minuten")]
        [InlineData(-3, "vor 3 Minuten")]
        [InlineData(-2, "vor 2 Minuten")]
        [InlineData(-1, "vor einer Minute")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Minuten")]
        [InlineData(1, "in einer Minute")]
        public void InMinutes(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-10, "vor 10 Monaten")]
        [InlineData(-3, "vor 3 Monaten")]
        [InlineData(-2, "vor 2 Monaten")]
        [InlineData(-1, "vor einem Monat")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Monaten")]
        [InlineData(1, "in einem Monat")]
        public void InMonths(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-10, "vor 10 Sekunden")]
        [InlineData(-3, "vor 3 Sekunden")]
        [InlineData(-2, "vor 2 Sekunden")]
        [InlineData(-1, "vor einer Sekunde")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Sekunden")]
        [InlineData(1, "in einer Sekunde")]
        public void InSeconds(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-10, "vor 10 Jahren")]
        [InlineData(-3, "vor 3 Jahren")]
        [InlineData(-2, "vor 2 Jahren")]
        [InlineData(-1, "vor einem Jahr")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }

        [Theory]
        [InlineData(2, "in 2 Jahren")]
        [InlineData(1, "in einem Jahr")]
        public void InYears(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
