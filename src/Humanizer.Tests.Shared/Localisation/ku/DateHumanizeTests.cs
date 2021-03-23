using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.ku
{
    [UseCulture("ku")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(-1, "دوێنێ")]
        [InlineData(-2, "2 ڕۆژ لەمەوبەر")]
        [InlineData(-3, "3 ڕۆژ لەمەوبەر")]
        [InlineData(-11, "11 ڕۆژ لەمەوبەر")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "بەیانی")]
        [InlineData(2, "2 ڕۆژی دیکە")]
        [InlineData(10, "10 ڕۆژی دیکە")]
        [InlineData(17, "17 ڕۆژی دیکە")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 کاتژمێر لەمەوبەر")]
        [InlineData(-1, "کاتژمێرێک لەمەوبەر")]
        [InlineData(-3, "3 کاتژمێر لەمەوبەر")]
        [InlineData(-11, "11 کاتژمێر لەمەوبەر")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "کاتژمێرێکی دیکە")]
        [InlineData(2, "2 کاتژمێری دیکە")]
        [InlineData(10, "10 کاتژمێری دیکە")]
        [InlineData(23, "23 کاتژمێری دیکە")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 خولەک لەمەوبەر")]
        [InlineData(-1, "خولەکێک لەمەوبەر")]
        [InlineData(-3, "3 خولەک لەمەوبەر")]
        [InlineData(-11, "11 خولەک لەمەوبەر")]
        [InlineData(60, "کاتژمێرێک لەمەوبەر")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "خولەکێکی دیکە")]
        [InlineData(2, "2 خولەکی دیکە")]
        [InlineData(10, "10 خولەکی دیکە")]
        [InlineData(23, "23 خولەکی دیکە")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 مانگ لەمەوبەر")]
        [InlineData(-1, "مانگێک لەمەوبەر")]
        [InlineData(-3, "3 مانگ لەمەوبەر")]
        [InlineData(-11, "11 مانگ لەمەوبەر")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "مانگێکی دیکە")]
        [InlineData(2, "2 مانگی دیکە")]
        [InlineData(10, "10 مانگی دیکە")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 چرکە لەمەوبەر")]
        [InlineData(-1, "چرکەیەک لەمەوبەر")]
        [InlineData(-3, "3 چرکە لەمەوبەر")]
        [InlineData(-11, "11 چرکە لەمەوبەر")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(0, "ئێستا")]
        [InlineData(1, "چرکەیەکی دیکە")]
        [InlineData(2, "2 چرکەی دیکە")]
        [InlineData(10, "10 چرکەی دیکە")]
        [InlineData(24, "24 چرکەی دیکە")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 ساڵ لەمەوبەر")]
        [InlineData(-1, "ساڵێک لەمەوبەر")]
        [InlineData(-3, "3 ساڵ لەمەوبەر")]
        [InlineData(-11, "11 ساڵ لەمەوبەر")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ساڵێکی دیکە")]
        [InlineData(2, "2 ساڵی دیکە")]
        [InlineData(7, "7 ساڵی دیکە")]
        [InlineData(55, "55 ساڵی دیکە")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
