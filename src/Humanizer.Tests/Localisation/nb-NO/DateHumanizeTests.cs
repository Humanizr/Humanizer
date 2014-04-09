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
        [InlineData(-10, "10 timer siden")]
        [InlineData(-3, "3 timer siden")]
        [InlineData(-2, "2 timer siden")]
        [InlineData(-1, "en time siden")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
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
        [InlineData(-10, "10 måneder siden")]
        [InlineData(-3, "3 måneder siden")]
        [InlineData(-2, "2 måneder siden")]
        [InlineData(-1, "en måned siden")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
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
    }
}
