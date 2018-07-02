using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.el
{
    [UseCulture("el")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(-2, "πριν από 2 ημέρες")]
        [InlineData(-1, "χθες")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 ημέρες από τώρα")]
        [InlineData(1, "αύριο")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "πριν από 2 ώρες")]
        [InlineData(-1, "πριν από μία ώρα")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 ώρες από τώρα")]
        [InlineData(1, "πρίν από μία ώρα από τώρα")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "πριν από 2 λεπτά")]
        [InlineData(-1, "πριν από ένα λεπτό")]
        [InlineData(60, "πριν από μία ώρα")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 λεπτά από τώρα")]
        [InlineData(1, "πρίν από ένα λεπτό από τώρα")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "πριν από 2 μήνες")]
        [InlineData(-1, "πριν από έναν μήνα")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 μήνες από τώρα")]
        [InlineData(1, "πριν από έναν μήνα από τώρα")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "πριν από 2 δευτερόλεπτα")]
        [InlineData(-1, "πριν από ένα δευτερόλεπτο")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 δευτερόλεπτα από τώρα")]
        [InlineData(1, "πριν από ένα δευτερόλεπτο από τώρα")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "πριν από 2 χρόνια")]
        [InlineData(-1, "πριν από έναν χρόνο")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 χρόνια από τώρα")]
        [InlineData(1, "πριν από έναν χρόνο από τώρα")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
