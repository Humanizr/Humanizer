using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation
{
    public class DateHumanizeTests_nbNO : AmbientCulture
    {
        public DateHumanizeTests_nbNO()
            : base("nb-NO")
        {
        }

        [Theory]
        [InlineData(-10, "10 dager siden")]
        [InlineData(-3, "3 dager siden")]
        [InlineData(-2, "2 dager siden")]
		[InlineData(-1, "i går")]
        public void DaysAgo(int days, string expected)
        {
            var date = DateTime.UtcNow.AddDays(days);
			Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 timer siden")]
        [InlineData(-3, "3 timer siden")]
        [InlineData(-2, "2 timer siden")]
        [InlineData(-1, "en time siden")]
        public void HoursAgo(int hours, string expected)
        {
            var date = DateTime.UtcNow.AddHours(hours);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 minutter siden")]
        [InlineData(-3, "3 minutter siden")]
        [InlineData(-2, "2 minutter siden")]
        [InlineData(-1, "et minutt siden")]
        public void MinutesAgo(int minutes, string expected)
        {
            var date = DateTime.UtcNow.AddMinutes(minutes);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 måneder siden")]
        [InlineData(-3, "3 måneder siden")]
        [InlineData(-2, "2 måneder siden")]
        [InlineData(-1, "en måned siden")]
        public void MonthsAgo(int months, string expected)
        {
            var date = DateTime.UtcNow.AddMonths(months);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 sekunder siden")]
        [InlineData(-3, "3 sekunder siden")]
        [InlineData(-2, "2 sekunder siden")]
        [InlineData(-1, "et sekund siden")]
        public void SecondsAgo(int seconds, string expected)
        {
            var date = DateTime.UtcNow.AddSeconds(seconds);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 år siden")]
        [InlineData(-3, "3 år siden")]
        [InlineData(-2, "2 år siden")]
        [InlineData(-1, "et år siden")]
        public void YearsAgo(int years, string expected)
        {
            var date = DateTime.UtcNow.AddYears(years);
            Assert.Equal(expected, date.Humanize());
        }
    }
}
