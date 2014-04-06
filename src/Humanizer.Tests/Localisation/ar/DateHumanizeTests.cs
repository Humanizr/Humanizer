using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ar
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ar") { }


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
        [InlineData(-2, "منذ ساعتين")]
        [InlineData(-1, "منذ ساعة واحدة")]
        [InlineData(-3, "منذ 3 ساعات")]
        [InlineData(-11, "منذ 11 ساعة")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(-2, "منذ دقيقتين")]
        [InlineData(-1, "منذ دقيقة واحدة")]
        [InlineData(-3, "منذ 3 دقائق")]
        [InlineData(-11, "منذ 11 دقيقة")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
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
        [InlineData(-2, "منذ ثانيتين")]
        [InlineData(-1, "منذ ثانية واحدة")]
        [InlineData(-3, "منذ 3 ثوان")]
        [InlineData(-11, "منذ 11 ثانية")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
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
    }
}
