using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.it
{
    public class ItalianDateHumanizeTests : AmbientCulture
    {
        public ItalianDateHumanizeTests() : base("it") { }

        [Theory]
        [InlineData(-10, "10 giorni fa")]
        [InlineData(-3, "3 giorni fa")]
        [InlineData(-2, "2 giorni fa")]
		[InlineData(-1, "ieri")]
        public void ItalianDaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 ore fa")]
        [InlineData(-3, "3 ore fa")]
        [InlineData(-2, "2 ore fa")]
        [InlineData(-1, "un'ora fa")]
        public void ItalianHoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 minuti fa")]
        [InlineData(-3, "3 minuti fa")]
        [InlineData(-2, "2 minuti fa")]
        [InlineData(-1, "un minuto fa")]
        public void ItalianMinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 mesi fa")]
        [InlineData(-3, "3 mesi fa")]
        [InlineData(-2, "2 mesi fa")]
        [InlineData(-1, "un mese fa")]
        public void ItalianMonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 secondi fa")]
        [InlineData(-3, "3 secondi fa")]
        [InlineData(-2, "2 secondi fa")]
        [InlineData(-1, "un secondo fa")]
        public void ItalianSecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-10, "10 anni fa")]
        [InlineData(-3, "3 anni fa")]
        [InlineData(-2, "2 anni fa")]
        [InlineData(-1, "un anno fa")]
        public void ItalianYearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
