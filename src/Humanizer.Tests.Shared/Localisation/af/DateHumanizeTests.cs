using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.af
{
    [UseCulture("af")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(2, "2 dae gelede")]
        [InlineData(1, "gister")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 ure gelede")]
        [InlineData(1, "1 uur terug")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 minute terug")]
        [InlineData(1, "1 minuut terug")]
        [InlineData(60, "1 uur terug")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 maande gelede")]
        [InlineData(1, "1 maand gelede")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 sekondes terug")]
        [InlineData(1, "1 sekonde terug")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 jaar gelede")]
        [InlineData(1, "1 jaar gelede")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "oor 2 dae")]
        [InlineData(1, "môre")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "oor 2 ure")]
        [InlineData(1, "oor 1 uur")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(2, "oor 2 minute")]
        [InlineData(1, "oor 1 minuut")]
        [InlineData(60, "oor 1 uur")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(2, "oor 2 maande")]
        [InlineData(1, "oor 1 maand")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(2, "oor 2 sekondes")]
        [InlineData(1, "oor 1 sekonde")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(2, "oor 2 jaar")]
        [InlineData(1, "oor 1 jaar")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "nou")]
        public void RightNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
