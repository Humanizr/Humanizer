using System;
using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nl
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("nl-NL") { }

        [Theory]
        [InlineData(-2, "2 dagen geleden")]
        [InlineData(-1, "gisteren")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "2 uur geleden")]
        [InlineData(-1, "één uur geleden")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "2 minuten geleden")]
        [InlineData(-1, "één minuut geleden")]
        [InlineData(60, "één uur geleden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "2 maanden geleden")]
        [InlineData(-1, "één maand geleden")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "2 seconden geleden")]
        [InlineData(-1, "één seconde geleden")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "2 jaar geleden")]
        [InlineData(-1, "één jaar geleden")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
