using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.mt
{

    [UseCulture("mt")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(-3, "3 jiem ilu")]
        [InlineData(-2, "jumejn ilu")]
        [InlineData(-1, "il-bieraħ")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(3, "3 jiem oħra")]
        [InlineData(2, "pitgħada")]
        [InlineData(1, "għada")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(-3, "3 siegħat ilu")]
        [InlineData(-2, "sagħtejn ilu")]
        [InlineData(-1, "siegħa ilu")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(3, "3 siegħat oħra")]
        [InlineData(2, "sagħtejn oħra")]
        [InlineData(1, "siegħa oħra")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-3, "3 minuti ilu")]
        [InlineData(-2, "2 minuti ilu")]
        [InlineData(-1, "minuta ilu")]
        [InlineData(60, "siegħa ilu")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 minuti oħra")]
        [InlineData(1, "minuta oħra")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(-3, "3 xhur ilu")]
        [InlineData(-2, "xahrejn ilu")]
        [InlineData(-1, "xahar ilu")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(3, "3 xhur oħra")]
        [InlineData(2, "xahrejn oħra")]
        [InlineData(1, "xahar ieħor")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "2 sekondi ilu")]
        [InlineData(-1, "sekonda ilu")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "2 sekondi oħra")]
        [InlineData(1, "sekonda oħra")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(-3, "3 snin ilu")]
        [InlineData(-2, "sentejn ilu")]
        [InlineData(-1, "sena ilu")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(3, "3 snin oħra")]
        [InlineData(2, "sentejn oħra")]
        [InlineData(1, "sena oħra")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Theory]
        [InlineData(0, "issa")]
        public void Now(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
