using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.sk
{
    [UseCulture("sk-SK")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "o sekundu")]
        [InlineData(2, "o 2 sekundy")]
        [InlineData(3, "o 3 sekundy")]
        [InlineData(4, "o 4 sekundy")]
        [InlineData(5, "o 5 sekúnd")]
        [InlineData(6, "o 6 sekúnd")]
        [InlineData(10, "o 10 sekúnd")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "o minútu")]
        [InlineData(2, "o 2 minúty")]
        [InlineData(3, "o 3 minúty")]
        [InlineData(4, "o 4 minúty")]
        [InlineData(5, "o 5 minút")]
        [InlineData(6, "o 6 minút")]
        [InlineData(10, "o 10 minút")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "o hodinu")]
        [InlineData(2, "o 2 hodiny")]
        [InlineData(3, "o 3 hodiny")]
        [InlineData(4, "o 4 hodiny")]
        [InlineData(5, "o 5 hodín")]
        [InlineData(6, "o 6 hodín")]
        [InlineData(10, "o 10 hodín")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "zajtra")]
        [InlineData(2, "o 2 dni")]
        [InlineData(3, "o 3 dni")]
        [InlineData(4, "o 4 dni")]
        [InlineData(9, "o 9 dní")]
        [InlineData(10, "o 10 dní")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "o mesiac")]
        [InlineData(2, "o 2 mesiace")]
        [InlineData(3, "o 3 mesiace")]
        [InlineData(4, "o 4 mesiace")]
        [InlineData(5, "o 5 mesiacov")]
        [InlineData(6, "o 6 mesiacov")]
        [InlineData(10, "o 10 mesiacov")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "o rok")]
        [InlineData(2, "o 2 roky")]
        [InlineData(3, "o 3 roky")]
        [InlineData(4, "o 4 roky")]
        [InlineData(5, "o 5 rokov")]
        [InlineData(6, "o 6 rokov")]
        [InlineData(10, "o 10 rokov")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(1, "pred sekundou")]
        [InlineData(2, "pred 2 sekundami")]
        [InlineData(3, "pred 3 sekundami")]
        [InlineData(4, "pred 4 sekundami")]
        [InlineData(5, "pred 5 sekundami")]
        [InlineData(6, "pred 6 sekundami")]
        [InlineData(10, "pred 10 sekundami")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "pred minútou")]
        [InlineData(2, "pred 2 minútami")]
        [InlineData(3, "pred 3 minútami")]
        [InlineData(4, "pred 4 minútami")]
        [InlineData(5, "pred 5 minútami")]
        [InlineData(6, "pred 6 minútami")]
        [InlineData(10, "pred 10 minútami")]
        [InlineData(60, "pred hodinou")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "pred hodinou")]
        [InlineData(2, "pred 2 hodinami")]
        [InlineData(3, "pred 3 hodinami")]
        [InlineData(4, "pred 4 hodinami")]
        [InlineData(5, "pred 5 hodinami")]
        [InlineData(6, "pred 6 hodinami")]
        [InlineData(10, "pred 10 hodinami")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "včera")]
        [InlineData(2, "pred 2 dňami")]
        [InlineData(3, "pred 3 dňami")]
        [InlineData(4, "pred 4 dňami")]
        [InlineData(9, "pred 9 dňami")]
        [InlineData(10, "pred 10 dňami")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "pred mesiacom")]
        [InlineData(2, "pred 2 mesiacmi")]
        [InlineData(3, "pred 3 mesiacmi")]
        [InlineData(4, "pred 4 mesiacmi")]
        [InlineData(5, "pred 5 mesiacmi")]
        [InlineData(6, "pred 6 mesiacmi")]
        [InlineData(10, "pred 10 mesiacmi")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "pred rokom")]
        [InlineData(2, "pred 2 rokmi")]
        [InlineData(3, "pred 3 rokmi")]
        [InlineData(4, "pred 4 rokmi")]
        [InlineData(5, "pred 5 rokmi")]
        [InlineData(6, "pred 6 rokmi")]
        [InlineData(10, "pred 10 rokmi")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
