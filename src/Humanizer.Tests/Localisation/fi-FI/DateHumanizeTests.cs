using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fiFI
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests()
            : base("fi-Fi")
        {
        }

        [Theory]
        [InlineData(-10, "10 päivää sitten")]
        [InlineData(-3, "3 päivää sitten")]
        [InlineData(-2, "2 päivää sitten")]
		[InlineData(-1, "eilen")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 tuntia sitten")]
        [InlineData(-3, "3 tuntia sitten")]
        [InlineData(-2, "2 tuntia sitten")]
        [InlineData(-1, "tunti sitten")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 minuuttia sitten")]
        [InlineData(-3, "3 minuuttia sitten")]
        [InlineData(-2, "2 minuuttia sitten")]
        [InlineData(-1, "minuutti sitten")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 kuukautta sitten")]
        [InlineData(-3, "3 kuukautta sitten")]
        [InlineData(-2, "2 kuukautta sitten")]
        [InlineData(-1, "kuukausi sitten")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 sekunttia sitten")]
        [InlineData(-3, "3 sekunttia sitten")]
        [InlineData(-2, "2 sekunttia sitten")]
        [InlineData(-1, "sekuntti sitten")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(-10, "10 vuotta sitten")]
        [InlineData(-3, "3 vuotta sitten")]
        [InlineData(-2, "2 vuotta sitten")]
        [InlineData(-1, "vuosi sitten")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
