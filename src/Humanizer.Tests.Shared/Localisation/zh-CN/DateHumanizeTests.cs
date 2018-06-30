using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.zhCN
{
    [UseCulture("zh-CN")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(2, "2 天前")]
        [InlineData(1, "昨天")]
        [InlineData(0, "今天")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 天后")]
        [InlineData(1, "明天")]
        [InlineData(0, "今天")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "2 小时前")]
        [InlineData(1, "1 小时前")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 小时后")]
        [InlineData(1, "1 小时后")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 分钟前")]
        [InlineData(-1, "1 分钟前")]
        [InlineData(60, "1 小时前")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 分钟后")]
        [InlineData(1, "1 分钟后")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 个月前")]
        [InlineData(-1, "1 个月前")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 个月后")]
        [InlineData(1, "1 个月后")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 秒钟前")]
        [InlineData(-1, "1 秒钟前")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 秒钟后")]
        [InlineData(1, "1 秒钟后")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 年前")]
        [InlineData(-1, "去年")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 年后")]
        [InlineData(1, "明年")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
