using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.nl
{
    [UseCulture("nl-NL")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(2, "2 dagen geleden")]
        [InlineData(1, "gisteren")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 uur geleden")]
        [InlineData(1, "1 uur geleden")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 minuten geleden")]
        [InlineData(1, "1 minuut geleden")]
        [InlineData(60, "1 uur geleden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 maanden geleden")]
        [InlineData(1, "1 maand geleden")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 seconden geleden")]
        [InlineData(1, "1 seconde geleden")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 jaar geleden")]
        [InlineData(1, "1 jaar geleden")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "over 2 dagen")]
        [InlineData(1, "morgen")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "over 2 uur")]
        [InlineData(1, "over 1 uur")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(2, "over 2 minuten")]
        [InlineData(1, "over 1 minuut")]
        [InlineData(60, "over 1 uur")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(2, "over 2 maanden")]
        [InlineData(1, "over 1 maand")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(2, "over 2 seconden")]
        [InlineData(1, "over 1 seconde")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(2, "over 2 jaar")]
        [InlineData(1, "over 1 jaar")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "nu")]
        public void RightNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
