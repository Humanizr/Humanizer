using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.zhHK
{
    [UseCulture("zh-HK")]
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
        [InlineData(2, "2 天後")]
        [InlineData(1, "明天")]
        [InlineData(0, "今天")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "2 小時前")]
        [InlineData(1, "1 小時前")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 小時後")]
        [InlineData(1, "1 小時後")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 分鐘前")]
        [InlineData(-1, "1 分鐘前")]
        [InlineData(60, "1 小時前")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 分鐘後")]
        [InlineData(1, "1 分鐘後")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 個月前")]
        [InlineData(-1, "1 個月前")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 個月後")]
        [InlineData(1, "1 個月後")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 秒鐘前")]
        [InlineData(-1, "1 秒鐘前")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 秒鐘後")]
        [InlineData(1, "1 秒鐘後")]
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
        [InlineData(2, "2 年後")]
        [InlineData(1, "明年")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
