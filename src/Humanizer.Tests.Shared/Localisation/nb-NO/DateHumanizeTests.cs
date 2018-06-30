using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.nbNO
{
    [UseCulture("nb-NO")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(-2, "2 dager siden")]
        [InlineData(-1, "i går")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "i morgen")]
        [InlineData(10, "10 dager fra nå")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "ett sekund fra nå")]
        [InlineData(10, "10 sekunder fra nå")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 timer siden")]
        [InlineData(-1, "en time siden")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en time fra nå")]
        [InlineData(10, "10 timer fra nå")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 minutter siden")]
        [InlineData(-1, "ett minutt siden")]
        [InlineData(60, "en time siden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ett minutt fra nå")]
        [InlineData(59, "59 minutter fra nå")]
        [InlineData(60, "en time fra nå")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 måneder siden")]
        [InlineData(-1, "en måned siden")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en måned fra nå")]
        [InlineData(10, "10 måneder fra nå")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 sekunder siden")]
        [InlineData(-1, "ett sekund siden")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
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
