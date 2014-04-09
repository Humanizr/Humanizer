using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.da
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("da-DK") { }

        [Theory]
        [InlineData(-10, "10 dage siden")]
        [InlineData(-3, "3 dage siden")]
        [InlineData(-2, "2 dage siden")]
        [InlineData(-1, "i går")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "i morgen")]
        [InlineData(10, "10 dage fra nu")]
        [InlineData(28, "28 dage fra nu")]
        [InlineData(32, "en måned fra nu")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "et sekund fra nu")]
        [InlineData(10, "10 sekunder fra nu")]
        [InlineData(59, "59 sekunder fra nu")]
        [InlineData(60, "et minut fra nu")]
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
        [InlineData(1, "en time fra nu")]
        [InlineData(10, "10 timer fra nu")]
        [InlineData(23, "23 timer fra nu")]
        [InlineData(24, "i morgen")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "10 minutter siden")]
        [InlineData(-3, "3 minutter siden")]
        [InlineData(-2, "2 minutter siden")]
        [InlineData(-1, "et minut siden")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "et minut fra nu")]
        [InlineData(10, "10 minutter fra nu")]
        [InlineData(44, "44 minutter fra nu")]
        [InlineData(45, "en time fra nu")]
        [InlineData(119, "en time fra nu")]
        [InlineData(120, "2 timer fra nu")]
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
        [InlineData(1, "en måned fra nu")]
        [InlineData(10, "10 måneder fra nu")]
        [InlineData(11, "11 måneder fra nu")]
        [InlineData(12, "et år fra nu")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "10 sekunder siden")]
        [InlineData(-3, "3 sekunder siden")]
        [InlineData(-2, "2 sekunder siden")]
        [InlineData(-1, "et sekund siden")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 år siden")]
        [InlineData(-3, "3 år siden")]
        [InlineData(-2, "2 år siden")]
        [InlineData(-1, "et år siden")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "et år fra nu")]
        [InlineData(2, "2 år fra nu")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "nu")]
        public void Now(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
