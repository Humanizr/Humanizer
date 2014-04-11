using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.hu
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests()
            : base("hu-HU")
        {
        }

        [Theory]
        [InlineData(1, "egy másodperce")]
        [InlineData(10, "10 másodperce")]
        [InlineData(59, "59 másodperce")]
        [InlineData(60, "egy perce")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "egy másodperc múlva")]
        [InlineData(10, "10 másodperc múlva")]
        [InlineData(59, "59 másodperc múlva")]
        [InlineData(60, "egy perc múlva")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "egy perce")]
        [InlineData(10, "10 perce")]
        [InlineData(44, "44 perce")]
        [InlineData(45, "egy órája")]
        [InlineData(120, "2 órája")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "egy perc múlva")]
        [InlineData(10, "10 perc múlva")]
        [InlineData(44, "44 perc múlva")]
        [InlineData(45, "egy óra múlva")]
        [InlineData(119, "egy óra múlva")]
        [InlineData(120, "2 óra múlva")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "egy órája")]
        [InlineData(10, "10 órája")]
        [InlineData(23, "23 órája")]
        [InlineData(24, "tegnap")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "egy óra múlva")]
        [InlineData(10, "10 óra múlva")]
        [InlineData(23, "23 óra múlva")]
        [InlineData(24, "holnap")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "tegnap")]
        [InlineData(10, "10 napja")]
        [InlineData(28, "28 napja")]
        [InlineData(32, "egy hónapja")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "holnap")]
        [InlineData(10, "10 nap múlva")]
        [InlineData(28, "28 nap múlva")]
        [InlineData(32, "egy hónap múlva")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "egy hónapja")]
        [InlineData(10, "10 hónapja")]
        [InlineData(11, "11 hónapja")]
        [InlineData(12, "egy éve")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "one month from now")]
        [InlineData(10, "10 months from now")]
        [InlineData(11, "11 months from now")]
        [InlineData(12, "one year from now")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "one year ago")]
        [InlineData(2, "2 years ago")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "one year from now")]
        [InlineData(2, "2 years from now")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

    }
}
