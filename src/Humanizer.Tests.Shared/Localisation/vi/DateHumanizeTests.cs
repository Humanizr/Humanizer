using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.vi
{
    [UseCulture("vi")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(1, "cách đây một giây")]
        [InlineData(10, "cách đây 10 giây")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "một giây nữa")]
        [InlineData(10, "10 giây nữa")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "cách đây một phút")]
        [InlineData(10, "cách đây 10 phút")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "một phút nữa")]
        [InlineData(10, "10 phút nữa")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "cách đây một tiếng")]
        [InlineData(10, "cách đây 10 tiếng")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "một tiếng nữa")]
        [InlineData(10, "10 tiếng nữa")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "hôm qua")]
        [InlineData(10, "cách đây 10 ngày")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ngày mai")]
        [InlineData(10, "10 ngày nữa")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "cách đây một tháng")]
        [InlineData(10, "cách đây 10 tháng")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "một tháng nữa")]
        [InlineData(10, "10 tháng nữa")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "cách đây một năm")]
        [InlineData(2, "cách đây 2 năm")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "một năm nữa")]
        [InlineData(2, "2 năm nữa")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

    }
}
