using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ja
{
    [UseCulture("ja")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "1 秒前")]
        [InlineData(2, "2 秒前")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1 秒後")]
        [InlineData(2, "2 秒後")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1 分前")]
        [InlineData(2, "2 分前")]
        [InlineData(60, "1 時間前")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1 分後")]
        [InlineData(2, "2 分後")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1 時間前")]
        [InlineData(2, "2 時間前")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1 時間後")]
        [InlineData(2, "2 時間後")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "昨日")]
        [InlineData(2, "2 日前")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "明日")]
        [InlineData(2, "2 日後")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "先月")]
        [InlineData(2, "2 か月前")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "来月")]
        [InlineData(2, "2 か月後")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "去年")]
        [InlineData(2, "2 年前")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "来年")]
        [InlineData(2, "2 年後")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("今", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
