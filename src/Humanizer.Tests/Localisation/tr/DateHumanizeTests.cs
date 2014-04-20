using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.tr
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests()
            : base("tr")
        {
        }

        [Theory]
        [InlineData(1, "bir saniye önce")]
        [InlineData(10, "10 saniye önce")]
        [InlineData(59, "59 saniye önce")]
        [InlineData(60, "bir dakika önce")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir saniye sonra")]
        [InlineData(10, "10 saniye sonra")]
        [InlineData(59, "59 saniye sonra")]
        [InlineData(60, "bir dakika sonra")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir dakika önce")]
        [InlineData(10, "10 dakika önce")]
        [InlineData(44, "44 dakika önce")]
        [InlineData(45, "bir saat önce")]
        [InlineData(119, "bir saat önce")]
        [InlineData(120, "2 saat önce")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir dakika sonra")]
        [InlineData(10, "10 dakika sonra")]
        [InlineData(44, "44 dakika sonra")]
        [InlineData(45, "bir saat sonra")]
        [InlineData(119, "bir saat sonra")]
        [InlineData(120, "2 saat sonra")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir saat önce")]
        [InlineData(10, "10 saat önce")]
        [InlineData(23, "23 saat önce")]
        [InlineData(24, "dün")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir saat sonra")]
        [InlineData(10, "10 saat sonra")]
        [InlineData(23, "23 saat sonra")]
        [InlineData(24, "yarın")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "dün")]
        [InlineData(10, "10 gün önce")]
        [InlineData(28, "28 gün önce")]
        [InlineData(32, "bir ay önce")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "yarın")]
        [InlineData(10, "10 gün sonra")]
        [InlineData(28, "28 gün sonra")]
        [InlineData(32, "bir ay sonra")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir ay önce")]
        [InlineData(10, "10 ay önce")]
        [InlineData(11, "11 ay önce")]
        [InlineData(12, "bir yıl önce")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir ay sonra")]
        [InlineData(10, "10 ay sonra")]
        [InlineData(11, "11 ay sonra")]
        [InlineData(12, "bir yıl sonra")]
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
