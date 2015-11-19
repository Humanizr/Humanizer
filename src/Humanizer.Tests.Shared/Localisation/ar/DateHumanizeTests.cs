using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ar
{
    [UseCulture("ar")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(-1, "أمس")]
        [InlineData(-2, "منذ يومين")]
        [InlineData(-3, "منذ 3 أيام")]
        [InlineData(-11, "منذ 11 يوم")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "في غضون يوم واحد من الآن")]
        [InlineData(2, "في غضون يومين من الآن")]
        [InlineData(10, "في غضون 10 أيام من الآن")]
        [InlineData(17, "في غضون 17 يوم من الآن")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "منذ ساعتين")]
        [InlineData(-1, "منذ ساعة واحدة")]
        [InlineData(-3, "منذ 3 ساعات")]
        [InlineData(-11, "منذ 11 ساعة")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "في غضون ساعة واحدة من الآن")]
        [InlineData(2, "في غضون ساعتين من الآن")]
        [InlineData(10, "في غضون 10 ساعات من الآن")]
        [InlineData(23, "في غضون 23 ساعة من الآن")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "منذ دقيقتين")]
        [InlineData(-1, "منذ دقيقة واحدة")]
        [InlineData(-3, "منذ 3 دقائق")]
        [InlineData(-11, "منذ 11 دقيقة")]
        [InlineData(60, "منذ ساعة واحدة")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "في غضون دقيقة واحدة من الآن")]
        [InlineData(2, "في غضون دقيقتين من الآن")]
        [InlineData(10, "في غضون 10 دقائق من الآن")]
        [InlineData(23, "في غضون 23 دقيقة من الآن")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "منذ شهرين")]
        [InlineData(-1, "منذ شهر واحد")]
        [InlineData(-3, "منذ 3 أشهر")]
        [InlineData(-11, "منذ 11 شهر")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "في غضون شهر واحد من الآن")]
        [InlineData(2, "في غضون شهرين من الآن")]
        [InlineData(10, "في غضون 10 أشهر من الآن")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "منذ ثانيتين")]
        [InlineData(-1, "منذ ثانية واحدة")]
        [InlineData(-3, "منذ 3 ثوان")]
        [InlineData(-11, "منذ 11 ثانية")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(0, "الآن")]
        [InlineData(1, "في غضون ثانية واحدة من الآن")]
        [InlineData(2, "في غضون ثانيتين من الآن")]
        [InlineData(10, "في غضون 10 ثوان من الآن")]
        [InlineData(24, "في غضون 24 ثانية من الآن")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "منذ عامين")]
        [InlineData(-1, "العام السابق")]
        [InlineData(-3, "منذ 3 أعوام")]
        [InlineData(-11, "منذ 11 عام")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "في غضون سنة واحدة من الآن")]
        [InlineData(2, "في غضون سنتين من الآن")]
        [InlineData(7, "في غضون 7 سنوات من الآن")]
        [InlineData(55, "في غضون 55 سنة من الآن")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
