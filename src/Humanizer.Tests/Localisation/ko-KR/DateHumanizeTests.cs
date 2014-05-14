using Humanizer.Localisation;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ko
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ko-KR") { }

        [Theory]
        [InlineData(1, "1초 전")]
        [InlineData(2, "2초 전")]
        public void SecondsAgo(int seconds, string expected) 
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1초 후")]
        [InlineData(2, "2초 후")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1분 전")]
        [InlineData(2, "2분 전")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1분 후")]
        [InlineData(2, "2분 후")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1시간 전")]
        [InlineData(2, "2시간 전")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1시간 후")]
        [InlineData(2, "2시간 후")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "어제")]
        [InlineData(2, "2일 전")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "내일")]
        [InlineData(2, "2일 후")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1개월 전")]
        [InlineData(2, "2개월 전")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1개월 후")]
        [InlineData(2, "2개월 후")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "1년 전")]
        [InlineData(2, "2년 전")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "1년 후")]
        [InlineData(2, "2년 후")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("지금", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
