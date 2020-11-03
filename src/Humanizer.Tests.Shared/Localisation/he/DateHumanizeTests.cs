using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.he
{
    [UseCulture("he")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(1, "אתמול")]
        [InlineData(2, "לפני יומיים")]
        [InlineData(3, "לפני 3 ימים")]
        [InlineData(11, "לפני 11 יום")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "לפני שעתיים")]
        [InlineData(1, "לפני שעה")]
        [InlineData(3, "לפני 3 שעות")]
        [InlineData(11, "לפני 11 שעות")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "לפני 2 דקות")]
        [InlineData(1, "לפני דקה")]
        [InlineData(3, "לפני 3 דקות")]
        [InlineData(11, "לפני 11 דקות")]
        [InlineData(60, "לפני שעה")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "לפני חודשיים")]
        [InlineData(1, "לפני חודש")]
        [InlineData(3, "לפני 3 חודשים")]
        [InlineData(11, "לפני 11 חודשים")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "לפני 2 שניות")]
        [InlineData(1, "לפני שנייה")]
        [InlineData(3, "לפני 3 שניות")]
        [InlineData(11, "לפני 11 שניות")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "לפני שנתיים")]
        [InlineData(1, "לפני שנה")]
        [InlineData(3, "לפני 3 שנים")]
        [InlineData(11, "לפני 11 שנה")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "בעוד יומיים")]
        [InlineData(1, "מחר")]
        [InlineData(3, "בעוד 3 ימים")]
        [InlineData(11, "בעוד 11 יום")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "בעוד חודשיים")]
        [InlineData(1, "בעוד חודש")]
        [InlineData(10, "בעוד 10 חודשים")]
        [InlineData(11, "בעוד 11 חודשים")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(2, "בעוד שנתיים")]
        [InlineData(1, "בעוד שנה")]
        [InlineData(3, "בעוד 3 שנים")]
        [InlineData(11, "בעוד 11 שנה")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(2, "בעוד שעתיים")]
        [InlineData(1, "בעוד שעה")]
        [InlineData(3, "בעוד 3 שעות")]
        [InlineData(11, "בעוד 11 שעות")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(2, "בעוד 2 דקות")]
        [InlineData(1, "בעוד דקה")]
        [InlineData(3, "בעוד 3 דקות")]
        [InlineData(11, "בעוד 11 דקות")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(2, "בעוד 2 שניות")]
        [InlineData(1, "בעוד שנייה")]
        [InlineData(3, "בעוד 3 שניות")]
        [InlineData(11, "בעוד 11 שניות")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
