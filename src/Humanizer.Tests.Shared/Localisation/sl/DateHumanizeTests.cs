using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.sl
{
    [UseCulture("sl-SI")]
    public class DateHumanizeTests
    {
        [Theory]
        [InlineData(-10, "pred 10 leti")]
        [InlineData(-5, "pred 5 leti")]
        [InlineData(-4, "pred 4 leti")]
        [InlineData(-3, "pred 3 leti")]
        [InlineData(-2, "pred 2 letoma")]
        [InlineData(-1, "pred enim letom")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 let")]
        [InlineData(4, "čez 4 leta")]
        [InlineData(3, "čez 3 leta")]
        [InlineData(2, "čez 2 leti")]
        [InlineData(1, "čez eno leto")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 meseci")]
        [InlineData(-5, "pred 5 meseci")]
        [InlineData(-4, "pred 4 meseci")]
        [InlineData(-3, "pred 3 meseci")]
        [InlineData(-2, "pred 2 mesecema")]
        [InlineData(-1, "pred enim mesecem")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 mesecev")]
        [InlineData(4, "čez 4 mesece")]
        [InlineData(3, "čez 3 mesece")]
        [InlineData(2, "čez 2 meseca")]
        [InlineData(1, "čez en mesec")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 dnevi")]
        [InlineData(-5, "pred 5 dnevi")]
        [InlineData(-4, "pred 4 dnevi")]
        [InlineData(-3, "pred 3 dnevi")]
        [InlineData(-2, "pred 2 dnevoma")]
        [InlineData(-1, "včeraj")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 dni")]
        [InlineData(4, "čez 4 dni")]
        [InlineData(3, "čez 3 dni")]
        [InlineData(2, "čez 2 dni")]
        [InlineData(1, "jutri")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 urami")]
        [InlineData(-5, "pred 5 urami")]
        [InlineData(-4, "pred 4 urami")]
        [InlineData(-3, "pred 3 urami")]
        [InlineData(-2, "pred 2 urama")]
        [InlineData(-1, "pred eno uro")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 ur")]
        [InlineData(4, "čez 4 ure")]
        [InlineData(3, "čez 3 ure")]
        [InlineData(2, "čez 2 uri")]
        [InlineData(1, "čez eno uro")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 minutami")]
        [InlineData(-5, "pred 5 minutami")]
        [InlineData(-4, "pred 4 minutami")]
        [InlineData(-3, "pred 3 minutami")]
        [InlineData(-2, "pred 2 minutama")]
        [InlineData(-1, "pred eno minuto")]
        [InlineData(60, "pred eno uro")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 minut")]
        [InlineData(4, "čez 4 minute")]
        [InlineData(3, "čez 3 minute")]
        [InlineData(2, "čez 2 minuti")]
        [InlineData(1, "čez eno minuto")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 sekundami")]
        [InlineData(-5, "pred 5 sekundami")]
        [InlineData(-4, "pred 4 sekundami")]
        [InlineData(-3, "pred 3 sekundami")]
        [InlineData(-2, "pred 2 sekundama")]
        [InlineData(-1, "pred eno sekundo")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(10, "čez 10 sekund")]
        [InlineData(5, "čez 5 sekund")]
        [InlineData(4, "čez 4 sekunde")]
        [InlineData(3, "čez 3 sekunde")]
        [InlineData(2, "čez 2 sekundi")]
        [InlineData(1, "čez eno sekundo")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
