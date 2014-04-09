using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sv
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests()
            : base("sv-SE")
        {
        }

        [Theory]
        [InlineData(1, "om en sekund")]
        [InlineData(2, "om 2 sekunder")]
        [InlineData(3, "om 3 sekunder")]
        [InlineData(4, "om 4 sekunder")]
        [InlineData(5, "om 5 sekunder")]
        [InlineData(6, "om 6 sekunder")]
        [InlineData(10, "om 10 sekunder")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en minut")]
		[InlineData(2, "om 2 minuter")]
		[InlineData(3, "om 3 minuter")]
		[InlineData(4, "om 4 minuter")]
		[InlineData(5, "om 5 minuter")]
		[InlineData(6, "om 6 minuter")]
		[InlineData(10, "om 10 minuter")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en timme")]
        [InlineData(2, "om 2 timmar")]
		[InlineData(3, "om 3 timmar")]
		[InlineData(4, "om 4 timmar")]
		[InlineData(5, "om 5 timmar")]
		[InlineData(6, "om 6 timmar")]
		[InlineData(10, "om 10 timmar")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "i morgon")]
        [InlineData(2, "om 2 dagar")]
		[InlineData(3, "om 3 dagar")]
		[InlineData(4, "om 4 dagar")]
		[InlineData(9, "om 9 dagar")]
		[InlineData(10, "om 10 dagar")]
        public void DayFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om en månad")]
        [InlineData(2, "om 2 månader")]
		[InlineData(3, "om 3 månader")]
		[InlineData(4, "om 4 månader")]
		[InlineData(5, "om 5 månader")]
		[InlineData(6, "om 6 månader")]
		[InlineData(10, "om 10 månader")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "om ett år")]
        [InlineData(2, "om 2 år")]
		[InlineData(3, "om 3 år")]
		[InlineData(4, "om 4 år")]
		[InlineData(5, "om 5 år")]
		[InlineData(6, "om 6 år")]
		[InlineData(10, "om 10 år")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(1, "en sekund sedan")]
        [InlineData(2, "för 2 sekunder sedan")]
		[InlineData(3, "för 3 sekunder sedan")]
		[InlineData(4, "för 4 sekunder sedan")]
		[InlineData(5, "för 5 sekunder sedan")]
		[InlineData(6, "för 6 sekunder sedan")]
		[InlineData(10, "för 10 sekunder sedan")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en minut sedan")]
		[InlineData(2, "för 2 minuter sedan")]
		[InlineData(3, "för 3 minuter sedan")]
		[InlineData(4, "för 4 minuter sedan")]
		[InlineData(5, "för 5 minuter sedan")]
		[InlineData(6, "för 6 minuter sedan")]
		[InlineData(10, "för 10 minuter sedan")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en timme sedan")]
		[InlineData(2, "för 2 timmar sedan")]
		[InlineData(3, "för 3 timmar sedan")]
		[InlineData(4, "för 4 timmar sedan")]
		[InlineData(5, "för 5 timmar sedan")]
		[InlineData(6, "för 6 timmar sedan")]
		[InlineData(10, "för 10 timmar sedan")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "igår")]
		[InlineData(2, "för 2 dagar sedan")]
		[InlineData(3, "för 3 dagar sedan")]
		[InlineData(4, "för 4 dagar sedan")]
		[InlineData(9, "för 9 dagar sedan")]
		[InlineData(10, "för 10 dagar sedan")]
        public void DayAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "en månad sedan")]
		[InlineData(2, "för 2 månader sedan")]
		[InlineData(3, "för 3 månader sedan")]
		[InlineData(4, "för 4 månader sedan")]
		[InlineData(5, "för 5 månader sedan")]
		[InlineData(6, "för 6 månader sedan")]
		[InlineData(10, "för 10 månader sedan")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ett år sedan")]
		[InlineData(2, "för 2 år sedan")]
		[InlineData(3, "för 3 år sedan")]
		[InlineData(4, "för 4 år sedan")]
		[InlineData(5, "för 5 år sedan")]
		[InlineData(6, "för 6 år sedan")]
		[InlineData(10, "för 10 år sedan")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
