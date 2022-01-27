using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.hr
{
    [UseCulture("hr-HR")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(-22, "prije 22 godine")]
        [InlineData(-5, "prije 5 godina")]
        [InlineData(-4, "prije 4 godine")]
        [InlineData(-2, "prije 2 godine")]
        [InlineData(-1, "prije godinu dana")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(5, "za 5 godina")]
        [InlineData(4, "za 4 godine")]
        [InlineData(3, "za 3 godine")]
        [InlineData(2, "za 2 godine")]
        [InlineData(1, "za godinu dana")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(-8, "prije 8 mjeseci")]
        [InlineData(-5, "prije 5 mjeseci")]
        [InlineData(-4, "prije 4 mjeseca")]
        [InlineData(-3, "prije 3 mjeseca")]
        [InlineData(-2, "prije 2 mjeseca")]
        [InlineData(-1, "prije mjesec dana")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(5, "za 5 mjeseci")]
        [InlineData(4, "za 4 mjeseca")]
        [InlineData(2, "za 2 mjeseca")]
        [InlineData(1, "za mjesec dana")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-24, "prije 24 dana")]
        [InlineData(-22, "prije 22 dana")]
        [InlineData(-10, "prije 10 dana")]
        [InlineData(-5, "prije 5 dana")]
        [InlineData(-4, "prije 4 dana")]
        [InlineData(-3, "prije 3 dana")]
        [InlineData(-2, "prije 2 dana")]
        [InlineData(-1, "jučer")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(10, "za 10 dana")]
        [InlineData(5, "za 5 dana")]
        [InlineData(1, "sutra")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "prije 10 sati")]
        [InlineData(-5, "prije 5 sati")]
        [InlineData(-4, "prije 4 sata")]
        [InlineData(-3, "prije 3 sata")]
        [InlineData(-2, "prije 2 sata")]
        [InlineData(-1, "prije sat vremena")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(5, "za 5 sati")]
        [InlineData(4, "za 4 sata")]
        [InlineData(3, "za 3 sata")]
        [InlineData(2, "za 2 sata")]
        [InlineData(1, "za sat vremena")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "prije 10 minuta")]
        [InlineData(-5, "prije 5 minuta")]
        [InlineData(-4, "prije 4 minute")]
        [InlineData(-3, "prije 3 minute")]
        [InlineData(-2, "prije 2 minute")]
        [InlineData(-1, "prije jedne minute")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(5, "za 5 minuta")]
        [InlineData(4, "za 4 minute")]
        [InlineData(3, "za 3 minute")]
        [InlineData(2, "za 2 minute")]
        [InlineData(1, "za jednu minutu")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "prije 10 sekundi")]
        [InlineData(-5, "prije 5 sekundi")]
        [InlineData(-4, "prije 4 sekunde")]
        [InlineData(-3, "prije 3 sekunde")]
        [InlineData(-2, "prije 2 sekunde")]
        [InlineData(-1, "prije jedne sekunde")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(10, "za 10 sekundi")]
        [InlineData(5, "za 5 sekundi")]
        [InlineData(4, "za 4 sekunde")]
        [InlineData(3, "za 3 sekunde")]
        [InlineData(2, "za 2 sekunde")]
        [InlineData(1, "za jednu sekundu")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
