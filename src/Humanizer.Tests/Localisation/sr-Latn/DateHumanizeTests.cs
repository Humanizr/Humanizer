using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.srLatn
{
    public class DateHumanizeDefaultStrategyTests : AmbientCulture
    {
        public DateHumanizeDefaultStrategyTests()
            : base("sr-Latn")
        {
        }

        [Theory]
        [InlineData(1, "pre sekund")]
        [InlineData(10, "pre 10 sekundi")]
        [InlineData(59, "pre 59 sekundi")]
        [InlineData(60, "pre minut")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za sekund")]
        [InlineData(10, "za 10 sekundi")]
        [InlineData(59, "za 59 sekundi")]
        [InlineData(60, "za minut")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre minut")]
        [InlineData(10, "pre 10 minuta")]
        [InlineData(44, "pre 44 minuta")]
        [InlineData(45, "pre sat vremena")]
        [InlineData(119, "pre sat vremena")]
        [InlineData(120, "pre 2 sata")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za minut")]
        [InlineData(10, "za 10 minuta")]
        [InlineData(44, "za 44 minuta")]
        [InlineData(45, "za sat vremena")]
        [InlineData(119, "za sat vremena")]
        [InlineData(120, "za 2 sata")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre sat vremena")]
        [InlineData(10, "pre 10 sati")]
        [InlineData(23, "pre 23 sata")]
        [InlineData(24, "juče")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za sat vremena")]
        [InlineData(10, "za 10 sati")]
        [InlineData(23, "za 23 sata")]
        [InlineData(24, "sutra")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "juče")]
        [InlineData(10, "pre 10 dana")]
        [InlineData(28, "pre 28 dana")]
        [InlineData(32, "pre mesec dana")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sutra")]
        [InlineData(10, "za 10 dana")]
        [InlineData(28, "za 28 dana")]
        [InlineData(32, "za mesec dana")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pre mesec dana")]
        [InlineData(10, "pre 10 meseci")]
        [InlineData(11, "pre 11 meseci")]
        [InlineData(12, "pre godinu dana")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "za mesec dana")]
        [InlineData(10, "za 10 meseci")]
        [InlineData(11, "za 11 meseci")]
        [InlineData(12, "za godinu dana")]
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