using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.az
{
    [UseCulture("az")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "bir saniyə əvvəl")]
        [InlineData(10, "10 saniyə əvvəl")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir saniyə sonra")]
        [InlineData(10, "10 saniyə sonra")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir dəqiqə əvvəl")]
        [InlineData(10, "10 dəqiqə əvvəl")]
        [InlineData(60, "bir saat əvvəl")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir dəqiqə sonra")]
        [InlineData(10, "10 dəqiqə sonra")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir saat əvvəl")]
        [InlineData(10, "10 saat əvvəl")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir saat sonra")]
        [InlineData(10, "10 saat sonra")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "dünən")]
        [InlineData(10, "10 gün əvvəl")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sabah")]
        [InlineData(10, "10 gün sonra")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir ay əvvəl")]
        [InlineData(10, "10 ay əvvəl")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir ay sonra")]
        [InlineData(10, "10 ay sonra")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir il əvvəl")]
        [InlineData(2, "2 il əvvəl")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir il sonra")]
        [InlineData(2, "2 il sonra")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("indi", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
