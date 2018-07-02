using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.cs
{
    [UseCulture("cs-CZ")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(1, "za sekundu")]
        [InlineData(2, "za 2 sekundy")]
        [InlineData(3, "za 3 sekundy")]
        [InlineData(4, "za 4 sekundy")]
        [InlineData(5, "za 5 sekund")]
        [InlineData(6, "za 6 sekund")]
        [InlineData(10, "za 10 sekund")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za minutu")]
        [InlineData(2, "za 2 minuty")]
        [InlineData(3, "za 3 minuty")]
        [InlineData(4, "za 4 minuty")]
        [InlineData(5, "za 5 minut")]
        [InlineData(6, "za 6 minut")]
        [InlineData(10, "za 10 minut")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za hodinu")]
        [InlineData(2, "za 2 hodiny")]
        [InlineData(3, "za 3 hodiny")]
        [InlineData(4, "za 4 hodiny")]
        [InlineData(5, "za 5 hodin")]
        [InlineData(6, "za 6 hodin")]
        [InlineData(10, "za 10 hodin")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "zítra")]
        [InlineData(2, "za 2 dny")]
        [InlineData(3, "za 3 dny")]
        [InlineData(4, "za 4 dny")]
        [InlineData(9, "za 9 dnů")]
        [InlineData(10, "za 10 dnů")]
        public void DayFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za měsíc")]
        [InlineData(2, "za 2 měsíce")]
        [InlineData(3, "za 3 měsíce")]
        [InlineData(4, "za 4 měsíce")]
        [InlineData(5, "za 5 měsíců")]
        [InlineData(6, "za 6 měsíců")]
        [InlineData(10, "za 10 měsíců")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za rok")]
        [InlineData(2, "za 2 roky")]
        [InlineData(3, "za 3 roky")]
        [InlineData(4, "za 4 roky")]
        [InlineData(5, "za 5 let")]
        [InlineData(6, "za 6 let")]
        [InlineData(10, "za 10 let")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(1, "před sekundou")]
        [InlineData(2, "před 2 sekundami")]
        [InlineData(3, "před 3 sekundami")]
        [InlineData(4, "před 4 sekundami")]
        [InlineData(5, "před 5 sekundami")]
        [InlineData(6, "před 6 sekundami")]
        [InlineData(10, "před 10 sekundami")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "před minutou")]
        [InlineData(2, "před 2 minutami")]
        [InlineData(3, "před 3 minutami")]
        [InlineData(4, "před 4 minutami")]
        [InlineData(5, "před 5 minutami")]
        [InlineData(6, "před 6 minutami")]
        [InlineData(10, "před 10 minutami")]
        [InlineData(60, "před hodinou")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "před hodinou")]
        [InlineData(2, "před 2 hodinami")]
        [InlineData(3, "před 3 hodinami")]
        [InlineData(4, "před 4 hodinami")]
        [InlineData(5, "před 5 hodinami")]
        [InlineData(6, "před 6 hodinami")]
        [InlineData(10, "před 10 hodinami")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "včera")]
        [InlineData(2, "před 2 dny")]
        [InlineData(3, "před 3 dny")]
        [InlineData(4, "před 4 dny")]
        [InlineData(9, "před 9 dny")]
        [InlineData(10, "před 10 dny")]
        public void DayAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "před měsícem")]
        [InlineData(2, "před 2 měsíci")]
        [InlineData(3, "před 3 měsíci")]
        [InlineData(4, "před 4 měsíci")]
        [InlineData(5, "před 5 měsíci")]
        [InlineData(6, "před 6 měsíci")]
        [InlineData(10, "před 10 měsíci")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "před rokem")]
        [InlineData(2, "před 2 lety")]
        [InlineData(3, "před 3 lety")]
        [InlineData(4, "před 4 lety")]
        [InlineData(5, "před 5 lety")]
        [InlineData(6, "před 6 lety")]
        [InlineData(10, "před 10 lety")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }
    }
}
