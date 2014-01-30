using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.es
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("es-ES") { }

        [Theory]
        [InlineData(-10, "hace 10 días")]
        [InlineData(-3, "hace 3 días")]
        [InlineData(-2, "hace 2 días")]
		[InlineData(-1, "ayer")]
        public void DaysAgo(int days, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddDays(days).Humanize());
        }

        [Theory]
        [InlineData(-10, "hace 10 horas")]
        [InlineData(-3, "hace 3 horas")]
        [InlineData(-2, "hace 2 horas")]
        [InlineData(-1, "hace una hora")]
        public void HoursAgo(int hours, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddHours(hours).Humanize());
        }

        [Theory]
        [InlineData(-10, "hace 10 minutos")]
        [InlineData(-3, "hace 3 minutos")]
        [InlineData(-2, "hace 2 minutos")]
        [InlineData(-1, "hace un minuto")]
        public void MinutesAgo(int minutes, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(-10, "hace 10 meses")]
        [InlineData(-3, "hace 3 meses")]
        [InlineData(-2, "hace 2 meses")]
        [InlineData(-1, "hace un mes")]
        public void MonthsAgo(int months, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddMonths(months).Humanize());
        }

        [Theory]
        [InlineData(-10, "hace 10 segundos")]
        [InlineData(-3, "hace 3 segundos")]
        [InlineData(-2, "hace 2 segundos")]
        [InlineData(-1, "hace un segundo")]
        public void SecondsAgo(int seconds, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(-10, "hace 10 años")]
        [InlineData(-3, "hace 3 años")]
        [InlineData(-2, "hace 2 años")]
        [InlineData(-1, "hace un año")]
        public void YearsAgo(int years, string expected)
        {
            Assert.Equal(expected, DateTime.UtcNow.AddYears(years).Humanize());
        }
    }
}
