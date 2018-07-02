using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.uzCyrl
{
    [UseCulture("uz-Cyrl-UZ")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "бир сония аввал")]
        [InlineData(10, "10 секунд аввал")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "бир сониядан сўнг")]
        [InlineData(10, "10 секунддан сўнг")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "бир дақиқа аввал")]
        [InlineData(10, "10 минут аввал")]
        [InlineData(60, "бир соат аввал")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "бир дақиқадан сўнг")]
        [InlineData(10, "10 минутдан сўнг")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "бир соат аввал")]
        [InlineData(10, "10 соат аввал")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "бир соатдан сўнг")]
        [InlineData(10, "10 соатдан сўнг")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "кеча")]
        [InlineData(10, "10 кун аввал")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "эртага")]
        [InlineData(10, "10 кундан сўнг")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "бир ой аввал")]
        [InlineData(10, "10 ой аввал")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "бир ойдан сўнг")]
        [InlineData(10, "10 ойдан сўнг")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "бир йил аввал")]
        [InlineData(2, "2 йил аввал")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "бир йилдан сўнг")]
        [InlineData(2, "2 йилдан сўнг")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("ҳозир", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
