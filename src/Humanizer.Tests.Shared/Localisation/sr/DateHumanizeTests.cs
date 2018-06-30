using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.sr
{
    [UseCulture("sr")]
    public class DateHumanizeDefaultStrategyTests
    {

        [Theory]
        [InlineData(1, "пре секунд")]
        [InlineData(10, "пре 10 секунди")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за секунд")]
        [InlineData(10, "за 10 секунди")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре минут")]
        [InlineData(10, "пре 10 минута")]
        [InlineData(60, "пре сат времена")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за минут")]
        [InlineData(10, "за 10 минута")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре сат времена")]
        [InlineData(10, "пре 10 сати")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за сат времена")]
        [InlineData(10, "за 10 сати")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "јуче")]
        [InlineData(10, "пре 10 дана")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "сутра")]
        [InlineData(10, "за 10 дана")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "пре месец дана")]
        [InlineData(10, "пре 10 месеци")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "за месец дана")]
        [InlineData(10, "за 10 месеци")]
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
