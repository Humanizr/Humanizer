using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.roRO
{


    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    [UseCulture("ro-RO")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(3, "acum 3 ore")]
        [InlineData(20, "acum 20 de ore")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(19, "acum 19 minute")]
        [InlineData(60, "acum o oră")]
        [InlineData(44, "acum 44 de minute")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "acum 2 secunde")]
        [InlineData(59, "acum 59 de secunde")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(10, "acum 10 zile")]
        [InlineData(23, "acum 23 de zile")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(119, "acum 119 ani")]
        [InlineData(100, "acum 100 de ani")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "acum")]
        [InlineData(22, "acum")]
        public void MillisecondsAgo(int milliseconds, string expected)
        {
            DateHumanize.Verify(expected, milliseconds, TimeUnit.Millisecond, Tense.Past);
        }

        [Theory]
        [InlineData(19, "peste 19 secunde")]
        [InlineData(21, "peste 21 de secunde")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(19, "peste 19 minute")]
        [InlineData(22, "peste 22 de minute")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(3, "peste 3 ore")]
        [InlineData(23, "peste 23 de ore")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(5, "peste 5 zile")]
        [InlineData(23, "peste 23 de zile")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(5, "peste 5 ani")]
        [InlineData(21, "peste 21 de ani")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }
    }
}
