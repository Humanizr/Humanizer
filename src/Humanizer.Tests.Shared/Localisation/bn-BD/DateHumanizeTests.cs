using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.bnBD
{
    [UseCulture("bn-BD")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(1, "আগামিকাল")]
        [InlineData(13, "13 দিন পর")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "গতকাল")]
        [InlineData(-11, "11 দিন আগে")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "এক ঘণ্টা পর")]
        [InlineData(11, "11 ঘণ্টা পর")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "এক ঘণ্টা আগে")]
        [InlineData(-11, "11 ঘণ্টা আগে")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "এক মিনিট পর")]
        [InlineData(13, "13 মিনিট পর")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "এক মিনিট আগে")]
        [InlineData(-13, "13 মিনিট আগে")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "এক মাস পর")]
        [InlineData(10, "10 মাস পর")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "এক মাস আগে")]
        [InlineData(-10, "10 মাস আগে")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "এক সেকেন্ড পর")]
        [InlineData(11, "11 সেকেন্ড পর")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "এক সেকেন্ড আগে")]
        [InlineData(-11, "11 সেকেন্ড আগে")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "এক বছর পর")]
        [InlineData(21, "21 বছর পর")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "এক বছর আগে")]
        [InlineData(-21, "21 বছর আগে")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
