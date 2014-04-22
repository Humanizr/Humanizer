using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sr
{
    public class DateHumanizeDefaultStrategyTests : AmbientCulture
    {
        public DateHumanizeDefaultStrategyTests()
            : base("sr")
        {
        }

        [Theory]
        [InlineData(1, "пре секунд")]
        [InlineData(10, "пре 10 секунди")]
        [InlineData(59, "пре 59 секунди")]
        [InlineData(60, "пре минут")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за секунд")]
        [InlineData(10, "за 10 секунди")]
        [InlineData(59, "за 59 секунди")]
        [InlineData(60, "за минут")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре минут")]
        [InlineData(10, "пре 10 минута")]
        [InlineData(44, "пре 44 минута")]
        [InlineData(45, "пре сат времена")]
        [InlineData(119, "пре сат времена")]
        [InlineData(120, "пре 2 сата")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за минут")]
        [InlineData(10, "за 10 минута")]
        [InlineData(44, "за 44 минута")]
        [InlineData(45, "за сат времена")]
        [InlineData(119, "за сат времена")]
        [InlineData(120, "за 2 сата")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре сат времена")]
        [InlineData(10, "пре 10 сати")]
        [InlineData(23, "пре 23 сата")]
        [InlineData(24, "јуче")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за сат времена")]
        [InlineData(10, "за 10 сати")]
        [InlineData(23, "за 23 сата")]
        [InlineData(24, "сутра")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "јуче")]
        [InlineData(10, "пре 10 дана")]
        [InlineData(28, "пре 28 дана")]
        [InlineData(32, "пре месец дана")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "сутра")]
        [InlineData(10, "за 10 дана")]
        [InlineData(28, "за 28 дана")]
        [InlineData(32, "за месец дана")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре месец дана")]
        [InlineData(10, "пре 10 месеци")]
        [InlineData(11, "пре 11 месеци")]
        [InlineData(12, "пре годину дана")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за месец дана")]
        [InlineData(10, "за 10 месеци")]
        [InlineData(11, "за 11 месеци")]
        [InlineData(12, "за годину дана")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре годину дана")]
        [InlineData(2, "пре 2 године")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за годину дана")]
        [InlineData(2, "за 2 године")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("сада", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
