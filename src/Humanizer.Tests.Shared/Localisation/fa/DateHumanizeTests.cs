using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.fa
{
    [UseCulture("fa")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(1, "فردا")]
        [InlineData(13, "13 روز بعد")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "دیروز")]
        [InlineData(-11, "11 روز پیش")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "یک ساعت بعد")]
        [InlineData(11, "11 ساعت بعد")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "یک ساعت پیش")]
        [InlineData(-11, "11 ساعت پیش")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "یک دقیقه بعد")]
        [InlineData(13, "13 دقیقه بعد")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "یک دقیقه پیش")]
        [InlineData(-13, "13 دقیقه پیش")]
        [InlineData(60, "یک ساعت پیش")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "یک ماه بعد")]
        [InlineData(10, "10 ماه بعد")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "یک ماه پیش")]
        [InlineData(-10, "10 ماه پیش")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "یک ثانیه بعد")]
        [InlineData(11, "11 ثانیه بعد")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "یک ثانیه پیش")]
        [InlineData(-11, "11 ثانیه پیش")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "یک سال بعد")]
        [InlineData(21, "21 سال بعد")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(-1, "یک سال پیش")]
        [InlineData(-21, "21 سال پیش")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
