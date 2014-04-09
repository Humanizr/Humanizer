using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nbNO
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests()
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
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "i morgen")]
        [InlineData(10, "10 dager fra nå")]
        [InlineData(28, "28 dager fra nå")]
        [InlineData(32, "en måned fra nå")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "ett sekund fra nå")]
        [InlineData(10, "10 sekunder fra nå")]
        [InlineData(59, "59 sekunder fra nå")]
        [InlineData(60, "ett minutt fra nå")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "10 timer siden")]
        [InlineData(-3, "3 timer siden")]
        [InlineData(-2, "2 timer siden")]
        [InlineData(-1, "en time siden")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en time fra nå")]
        [InlineData(10, "10 timer fra nå")]
        [InlineData(23, "23 timer fra nå")]
        [InlineData(24, "i morgen")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "10 minutter siden")]
        [InlineData(-3, "3 minutter siden")]
        [InlineData(-2, "2 minutter siden")]
        [InlineData(-1, "ett minutt siden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ett minutt fra nå")]
        [InlineData(10, "10 minutter fra nå")]
        [InlineData(44, "44 minutter fra nå")]
        [InlineData(45, "en time fra nå")]
        [InlineData(119, "en time fra nå")]
        [InlineData(120, "2 timer fra nå")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }
        [Theory]
        [InlineData(-10, "10 måneder siden")]
        [InlineData(-3, "3 måneder siden")]
        [InlineData(-2, "2 måneder siden")]
        [InlineData(-1, "en måned siden")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en måned fra nå")]
        [InlineData(10, "10 måneder fra nå")]
        [InlineData(11, "11 måneder fra nå")]
        [InlineData(12, "ett år fra nå")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "10 sekunder siden")]
        [InlineData(-3, "3 sekunder siden")]
        [InlineData(-2, "2 sekunder siden")]
        [InlineData(-1, "ett sekund siden")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 år siden")]
        [InlineData(-3, "3 år siden")]
        [InlineData(-2, "2 år siden")]
        [InlineData(-1, "ett år siden")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ett år fra nå")]
        [InlineData(2, "2 år fra nå")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "nå")]
        public void Now(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
