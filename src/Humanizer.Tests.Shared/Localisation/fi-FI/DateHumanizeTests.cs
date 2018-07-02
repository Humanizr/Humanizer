using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.fiFI
{
    [UseCulture("fi-FI")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(2, "2 päivää sitten")]
        [InlineData(1, "eilen")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 tuntia sitten")]
        [InlineData(1, "tunti sitten")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 minuuttia sitten")]
        [InlineData(1, "minuutti sitten")]
        [InlineData(60, "tunti sitten")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 kuukautta sitten")]
        [InlineData(1, "kuukausi sitten")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 sekuntia sitten")]
        [InlineData(1, "sekuntti sitten")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 vuotta sitten")]
        [InlineData(1, "vuosi sitten")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
