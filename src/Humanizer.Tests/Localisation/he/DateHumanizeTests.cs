using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.he
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("he") { }

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
    }
}
