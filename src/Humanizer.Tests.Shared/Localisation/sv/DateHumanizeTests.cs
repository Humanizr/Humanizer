using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.sv
{
    [UseCulture("sv-SE")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "om en sekund")]
        [InlineData(2, "om 2 sekunder")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en minut")]
        [InlineData(2, "om 2 minuter")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en timme")]
        [InlineData(2, "om 2 timmar")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "i morgon")]
        [InlineData(2, "om 2 dagar")]
        public void DayFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en månad")]
        [InlineData(2, "om 2 månader")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om ett år")]
        [InlineData(2, "om 2 år")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(1, "en sekund sedan")]
        [InlineData(2, "för 2 sekunder sedan")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en minut sedan")]
        [InlineData(2, "för 2 minuter sedan")]
        [InlineData(60, "en timme sedan")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en timme sedan")]
        [InlineData(2, "för 2 timmar sedan")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "igår")]
        [InlineData(2, "för 2 dagar sedan")]
        public void DayAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en månad sedan")]
        [InlineData(2, "för 2 månader sedan")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ett år sedan")]
        [InlineData(2, "för 2 år sedan")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
