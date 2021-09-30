using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    [UseCulture("is")]
    public class DateHumanizeTests
    {

        [Theory]
        [InlineData(2, "fyrir 2 dögum")]
        [InlineData(1, "í gær")]
        public void DaysAgo(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 daga")]
        [InlineData(1, "á morgun")]
        public void DaysFromNow(int days, string expected)
        {
            DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);
        }

        [Theory]
        [InlineData(2, "fyrir 2 klukkustundum")]
        [InlineData(1, "fyrir einni klukkustund")]
        public void HoursAgo(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 klukkustundir")]
        [InlineData(1, "eftir eina klukkustund")]
        public void HoursFromNow(int hours, string expected)
        {
            DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);
        }

        [Theory]
        [InlineData(-2, "fyrir 2 mínútum")]
        [InlineData(-1, "fyrir einni mínútu")]
        [InlineData(60, "fyrir einni klukkustund")]
        public void MinutesAgo(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 mínútur")]
        [InlineData(1, "eftir eina mínútu")]
        [InlineData(10, "eftir 10 mínútur")]
        [InlineData(59, "eftir 59 mínútur")]
        [InlineData(60, "eftir eina klukkustund")]
        public void MinutesFromNow(int minutes, string expected)
        {
            DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);
        }

        [Theory]
        [InlineData(2, "fyrir 2 mánuðum")]
        [InlineData(1, "fyrir einum mánuði")]
        public void MonthsAgo(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 mánuði")]
        [InlineData(1, "eftir einn mánuð")]
        public void MonthsFromNow(int months, string expected)
        {
            DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);
        }

        [Theory]
        [InlineData(2, "fyrir 2 sekúndum")]
        [InlineData(1, "fyrir einni sekúndu")]
        public void SecondsAgo(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 sekúndur")]
        [InlineData(1, "eftir eina sekúndu")]
        public void SecondsFromNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }

        [Theory]
        [InlineData(2, "fyrir 2 árum")]
        [InlineData(1, "fyrir einu ári")]
        [InlineData(20, "fyrir 20 árum")]
        [InlineData(30, "fyrir 30 árum")]
        public void YearsAgo(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);
        }

        [Theory]
        [InlineData(2, "eftir 2 ár")]
        [InlineData(1, "eftir eitt ár")]
        public void YearsFromNow(int years, string expected)
        {
            DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
        }


        [Theory]
        [InlineData(0, "núna")]
        public void RightNow(int seconds, string expected)
        {
            DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);
        }
    }
}
