using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.pl
{
    [UseCulture("pl")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "za sekundę")]
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
        [InlineData(1, "za minutę")]
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
        [InlineData(1, "za godzinę")]
        [InlineData(2, "za 2 godziny")]
        [InlineData(3, "za 3 godziny")]
        [InlineData(4, "za 4 godziny")]
        [InlineData(5, "za 5 godzin")]
        [InlineData(6, "za 6 godzin")]
        [InlineData(10, "za 10 godzin")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "jutro")]
        [InlineData(2, "za 2 dni")]
        [InlineData(3, "za 3 dni")]
        [InlineData(4, "za 4 dni")]
        [InlineData(5, "za 5 dni")]
        [InlineData(6, "za 6 dni")]
        [InlineData(10, "za 10 dni")]
        public void DayFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za miesiąc")]
        [InlineData(2, "za 2 miesiące")]
        [InlineData(3, "za 3 miesiące")]
        [InlineData(4, "za 4 miesiące")]
        [InlineData(5, "za 5 miesięcy")]
        [InlineData(6, "za 6 miesięcy")]
        [InlineData(10, "za 10 miesięcy")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "za rok")]
        [InlineData(2, "za 2 lata")]
        [InlineData(3, "za 3 lata")]
        [InlineData(4, "za 4 lata")]
        [InlineData(5, "za 5 lat")]
        [InlineData(6, "za 6 lat")]
        [InlineData(10, "za 10 lat")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(1, "przed sekundą")]
        [InlineData(2, "przed 2 sekundami")]
        [InlineData(3, "przed 3 sekundami")]
        [InlineData(4, "przed 4 sekundami")]
        [InlineData(5, "przed 5 sekundami")]
        [InlineData(6, "przed 6 sekundami")]
        [InlineData(10, "przed 10 sekundami")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "przed minutą")]
        [InlineData(2, "przed 2 minutami")]
        [InlineData(3, "przed 3 minutami")]
        [InlineData(4, "przed 4 minutami")]
        [InlineData(5, "przed 5 minutami")]
        [InlineData(6, "przed 6 minutami")]
        [InlineData(10, "przed 10 minutami")]
        [InlineData(60, "przed godziną")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "przed godziną")]
        [InlineData(2, "przed 2 godzinami")]
        [InlineData(3, "przed 3 godzinami")]
        [InlineData(4, "przed 4 godzinami")]
        [InlineData(5, "przed 5 godzinami")]
        [InlineData(6, "przed 6 godzinami")]
        [InlineData(10, "przed 10 godzinami")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "wczoraj")]
        [InlineData(2, "przed 2 dniami")]
        [InlineData(3, "przed 3 dniami")]
        [InlineData(4, "przed 4 dniami")]
        [InlineData(9, "przed 9 dniami")]
        [InlineData(10, "przed 10 dniami")]
        public void DayAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "przed miesiącem")]
        [InlineData(2, "przed 2 miesiącami")]
        [InlineData(3, "przed 3 miesiącami")]
        [InlineData(4, "przed 4 miesiącami")]
        [InlineData(5, "przed 5 miesiącami")]
        [InlineData(6, "przed 6 miesiącami")]
        [InlineData(10, "przed 10 miesiącami")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "przed rokiem")]
        [InlineData(2, "przed 2 laty")]
        [InlineData(3, "przed 3 laty")]
        [InlineData(4, "przed 4 laty")]
        [InlineData(5, "przed 5 laty")]
        [InlineData(6, "przed 6 laty")]
        [InlineData(10, "przed 10 laty")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("teraz", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
