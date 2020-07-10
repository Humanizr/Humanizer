using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.hy
{
    [UseCulture("hy")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "մեկ վայրկյան առաջ")]
        [InlineData(2, "2 վայրկյան առաջ")]
        [InlineData(3, "3 վայրկյան առաջ")]
        [InlineData(4, "4 վայրկյան առաջ")]
        [InlineData(11, "11 վայրկյան առաջ")]
        [InlineData(21, "21 վայրկյան առաջ")]
        [InlineData(24, "24 վայրկյան առաջ")]
        [InlineData(40, "40 վայրկյան առաջ")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "մեկ վայրկյանից")]
        [InlineData(2, "2 վայրկյանից")]
        [InlineData(11, "11 վայրկյանից")]
        [InlineData(20, "20 վայրկյանից")]
        [InlineData(40, "40 վայրկյանից")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "մեկ րոպե առաջ")]
        [InlineData(2, "2 րոպե առաջ")]
        [InlineData(10, "10 րոպե առաջ")]
        [InlineData(25, "25 րոպե առաջ")]
        [InlineData(40, "40 րոպե առաջ")]
        [InlineData(60, "մեկ ժամ առաջ")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "մեկ րոպեից")]
        [InlineData(2, "2 րոպեից")]
        [InlineData(19, "19 րոպեից")]
        [InlineData(25, "25 րոպեից")]
        [InlineData(40, "40 րոպեից")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "մեկ ժամ առաջ")]
        [InlineData(2, "2 ժամ առաջ")]
        [InlineData(19, "19 ժամ առաջ")]
        [InlineData(20, "20 ժամ առաջ")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "մեկ ժամից")]
        [InlineData(5, "5 ժամից")]
        [InlineData(23, "23 ժամից")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "երեկ")]
        [InlineData(2, "2 օր առաջ")]
        [InlineData(25, "25 օր առաջ")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "վաղը")]
        [InlineData(2, "2 օրից")]
        [InlineData(25, "25 օրից")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "մեկ ամիս առաջ")]
        [InlineData(11, "11 ամիս առաջ")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "մեկ ամսից")]
        [InlineData(11, "11 ամսից")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "մեկ տարի առաջ")]
        [InlineData(2, "2 տարի առաջ")]
        [InlineData(21, "21 տարի առաջ")]
        [InlineData(111, "111 տարի առաջ")]
        [InlineData(121, "121 տարի առաջ")]
        [InlineData(222, "222 տարի առաջ")]
        [InlineData(325, "325 տարի առաջ")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "մեկ տարուց")]
        [InlineData(2, "2 տարուց")]
        [InlineData(21, "21 տարուց")]
        [InlineData(111, "111 տարուց")]
        [InlineData(121, "121 տարուց")]
        [InlineData(222, "222 տարուց")]
        [InlineData(325, "325 տարուց")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("հիմա", 0, TimeUnit.Day, Tense.Past);
        }
    }
}
