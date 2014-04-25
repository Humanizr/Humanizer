using Humanizer.Localisation;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sl
{
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("sl-SI") {}

        [Theory]
        [InlineData(-10, "pred 10 leti")]
        [InlineData(-3, "pred 3 leti")]
        [InlineData(-2, "pred 2 leti")]
        [InlineData(-1, "pred enim letom")]
        public void YearsAgo(int years, string expected) {
           DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 let")]
        [InlineData(4, "čez 4 let")]
        [InlineData(3, "čez 3 let")]
        [InlineData(2, "čez 2 leti")]
        [InlineData(1, "čez eno leto")]
        public void YearsFromNow(int years, string expected) {
           DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 meseci")]
        [InlineData(-3, "pred 3 meseci")]
        [InlineData(-2, "pred 2 mesecema")]
        [InlineData(-1, "pred enim mesecem")]
        public void MonthsAgo(int months, string expected) {
           DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(3, "čez 3 mesecev")]
        [InlineData(2, "čez 2 meseca")]
        [InlineData(1, "čez en mesec")]
        public void MonthsFromNow(int months, string expected) {
           DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 dnevi")]
        [InlineData(-3, "pred 3 dnevi")]
        [InlineData(-2, "pred 2 dnevoma")]
        [InlineData(-1, "včeraj")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "čez 2 dni")]
        [InlineData(1, "jutri")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 urami")]
        [InlineData(-3, "pred 3 urami")]
        [InlineData(-2, "pred 2 urama")]
        [InlineData(-1, "pred eno uro")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 ur")]
        [InlineData(4, "čez 4 ur")]
        [InlineData(3, "čez 3 ur")]
        [InlineData(2, "čez 2 uri")]
        [InlineData(1, "čez eno uro")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 minutami")]
        [InlineData(-3, "pred 3 minutami")]
        [InlineData(-2, "pred 2 minutama")]
        [InlineData(-1, "pred eno minuto")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(5, "čez 5 minut")]
        [InlineData(4, "čez 4 minut")]
        [InlineData(3, "čez 3 minut")]
        [InlineData(2, "čez 2 minuti")]
        [InlineData(1, "čez eno minuto")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-10, "pred 10 sekundami")]
        [InlineData(-3, "pred 3 sekundami")]
        [InlineData(-2, "pred 2 sekundama")]
        [InlineData(-1, "pred eno sekundo")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(3, "čez 3 sekund")]
        [InlineData(2, "čez 2 sekundi")]
        [InlineData(1, "čez eno sekundo")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
