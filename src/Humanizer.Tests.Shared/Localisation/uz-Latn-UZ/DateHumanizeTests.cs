using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.uzLatn
{
    [UseCulture("uz-Latn-UZ")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(1, "bir soniya avval")]
        [InlineData(10, "10 sekund avval")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir soniyadan so`ng")]
        [InlineData(10, "10 sekunddan so`ng")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir daqiqa avval")]
        [InlineData(10, "10 minut avval")]
        [InlineData(60, "bir soat avval")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir daqiqadan so`ng")]
        [InlineData(10, "10 minutdan so`ng")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir soat avval")]
        [InlineData(10, "10 soat avval")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir soatdan so`ng")]
        [InlineData(10, "10 soatdan so`ng")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(1, "kecha")]
        [InlineData(10, "10 kun avval")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(1, "ertaga")]
        [InlineData(10, "10 kundan so`ng")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir oy avval")]
        [InlineData(10, "10 oy avval")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir oydan so`ng")]
        [InlineData(10, "10 oydan so`ng")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(1, "bir yil avval")]
        [InlineData(2, "2 yil avval")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(1, "bir yildan so`ng")]
        [InlineData(2, "2 yildan so`ng")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }

        [Fact]
        public void Now()
        {
            DateHumanize.Verify("hozir", 0, TimeUnit.Year, Tense.Future);
        }
    }
}
