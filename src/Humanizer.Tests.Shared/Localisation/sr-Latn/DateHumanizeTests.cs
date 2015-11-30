using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.srLatn
{
    [UseCulture("sr-Latn")]
    public class DateHumanizeDefaultStrategyTests
    {

        [Theory]
        [InlineData(1, "pre sekund")]
        [InlineData(10, "pre 10 sekundi")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za sekund")]
        [InlineData(10, "za 10 sekundi")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre minut")]
        [InlineData(10, "pre 10 minuta")]
        [InlineData(60, "pre sat vremena")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za minut")]
        [InlineData(10, "za 10 minuta")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre sat vremena")]
        [InlineData(10, "pre 10 sati")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za sat vremena")]
        [InlineData(10, "za 10 sati")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "juče")]
        [InlineData(10, "pre 10 dana")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sutra")]
        [InlineData(10, "za 10 dana")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre mesec dana")]
        [InlineData(10, "pre 10 meseci")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za mesec dana")]
        [InlineData(10, "za 10 meseci")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre godinu dana")]
        [InlineData(2, "pre 2 godine")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za godinu dana")]
        [InlineData(2, "za 2 godine")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("sada", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
