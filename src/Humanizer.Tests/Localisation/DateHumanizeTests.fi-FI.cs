using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation
{
    public class DateHumanizeTests_fiFI : AmbientCulture
    {
        public DateHumanizeTests_fiFI()
            : base("fi-Fi")
        {
        }

        [Theory]
        [InlineData(-10, "10 päivää sitten")]
        [InlineData(-3, "3 päivää sitten")]
        [InlineData(-2, "2 päivää sitten")]
		[InlineData(-1, "eilen")]
        public void DaysAgo(int days, string expected)
        {
            var date = DateTime.UtcNow.AddDays(days);
			Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 tuntia sitten")]
        [InlineData(-3, "3 tuntia sitten")]
        [InlineData(-2, "2 tuntia sitten")]
        [InlineData(-1, "tunti sitten")]
        public void HoursAgo(int hours, string expected)
        {
            var date = DateTime.UtcNow.AddHours(hours);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 minuuttia sitten")]
        [InlineData(-3, "3 minuuttia sitten")]
        [InlineData(-2, "2 minuuttia sitten")]
        [InlineData(-1, "minuutti sitten")]
        public void MinutesAgo(int minutes, string expected)
        {
            var date = DateTime.UtcNow.AddMinutes(minutes);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 kuukautta sitten")]
        [InlineData(-3, "3 kuukautta sitten")]
        [InlineData(-2, "2 kuukautta sitten")]
        [InlineData(-1, "kuukausi sitten")]
        public void MonthsAgo(int months, string expected)
        {
            var date = DateTime.UtcNow.AddMonths(months);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 sekunttia sitten")]
        [InlineData(-3, "3 sekunttia sitten")]
        [InlineData(-2, "2 sekunttia sitten")]
        [InlineData(-1, "sekuntti sitten")]
        public void SecondsAgo(int seconds, string expected)
        {
            var date = DateTime.UtcNow.AddSeconds(seconds);
            Assert.Equal(expected, date.Humanize());
        }

        [Theory]
        [InlineData(-10, "10 vuotta sitten")]
        [InlineData(-3, "3 vuotta sitten")]
        [InlineData(-2, "2 vuotta sitten")]
        [InlineData(-1, "vuosi sitten")]
        public void YearsAgo(int years, string expected)
        {
            var date = DateTime.UtcNow.AddYears(years);
            Assert.Equal(expected, date.Humanize());
        }
    }
}
