using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.tr
{
    [UseCulture("tr")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "bir saniye önce")]
        [InlineData(10, "10 saniye önce")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir saniye sonra")]
        [InlineData(10, "10 saniye sonra")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir dakika önce")]
        [InlineData(10, "10 dakika önce")]
        [InlineData(60, "bir saat önce")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir dakika sonra")]
        [InlineData(10, "10 dakika sonra")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir saat önce")]
        [InlineData(10, "10 saat önce")]
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
        [InlineData(1, "dün")]
        [InlineData(10, "10 gün önce")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "yarın")]
        [InlineData(10, "10 gün sonra")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir ay önce")]
        [InlineData(10, "10 ay önce")]
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
        [InlineData(1, "bir yıl önce")]
        [InlineData(2, "2 yıl önce")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir yıl sonra")]
        [InlineData(2, "2 yıl sonra")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("şimdi", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
