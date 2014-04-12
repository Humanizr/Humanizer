using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.id
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("id-ID") { }

        [Theory]
        [InlineData(-1, "sedetik yang lalu")]
        [InlineData(120, "2 menit yang lalu")]
        [InlineData(90, "semenit yang lalu")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sedetik dari sekarang")]
        [InlineData(60, "semenit dari sekarang")]
        [InlineData(120, "2 menit dari sekarang")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "semenit yang lalu")]
        [InlineData(15, "15 menit yang lalu")]
        [InlineData(45, "sejam yang lalu")]
        [InlineData(150, "2 jam yang lalu")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "semenit dari sekarang")]
        [InlineData(15, "15 menit dari sekarang")]
        [InlineData(45, "sejam dari sekarang")]
        [InlineData(150, "2 jam dari sekarang")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "sejam yang lalu")]
        [InlineData(10, "10 jam yang lalu")]
        [InlineData(24, "kemarin")]
        [InlineData(48, "2 hari yang lalu")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sejam dari sekarang")]
        [InlineData(10, "10 jam dari sekarang")]
        [InlineData(24, "besok")]
        [InlineData(48, "2 hari dari sekarang")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "kemarin")]
        [InlineData(15, "15 hari yang lalu")]
        [InlineData(38, "sebulan yang lalu")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "besok")]
        [InlineData(10, "10 hari dari sekarang")]
        [InlineData(32, "sebulan dari sekarang")]
        [InlineData(80, "2 bulan dari sekarang")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "sebulan yang lalu")]
        [InlineData(10, "10 bulan yang lalu")]
        [InlineData(12, "setahun yang lalu")]
        [InlineData(32, "2 tahun yang lalu")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "sebulan dari sekarang")]
        [InlineData(8, "8 bulan dari sekarang")]
        [InlineData(12, "setahun dari sekarang")]
        [InlineData(26, "2 tahun dari sekarang")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "setahun yang lalu")]
        [InlineData(-2, "2 tahun yang lalu")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(-1, "setahun dari sekarang")]
        [InlineData(-5, "5 tahun dari sekarang")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
