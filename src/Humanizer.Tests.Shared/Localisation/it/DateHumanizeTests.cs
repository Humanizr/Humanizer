using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.it
{
    [UseCulture("it")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(-2, "2 giorni fa")]
        [InlineData(-1, "ieri")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 giorni")]
        [InlineData(1, "domani")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 ore fa")]
        [InlineData(-1, "un'ora fa")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 ore")]
        [InlineData(1, "tra un'ora")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 minuti fa")]
        [InlineData(-1, "un minuto fa")]
        [InlineData(60, "un'ora fa")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 minuti")]
        [InlineData(1, "tra un minuto")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 mesi fa")]
        [InlineData(-1, "un mese fa")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 mesi")]
        [InlineData(1, "tra un mese")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 secondi fa")]
        [InlineData(-1, "un secondo fa")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 secondi")]
        [InlineData(1, "tra un secondo")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 anni fa")]
        [InlineData(-1, "un anno fa")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "tra 2 anni")]
        [InlineData(1, "tra un anno")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "adesso")]
        public void Now(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
