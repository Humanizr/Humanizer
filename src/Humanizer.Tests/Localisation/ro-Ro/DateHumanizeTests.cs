using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation.roRo
{
    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    public class DateHumanizeTests : AmbientCulture
    {
        public DateHumanizeTests() : base("ro-RO")
        {
        }

        [Fact]
        public void RomanianTranslationIsCorrectForThreeHoursAgo()
        {
            DateHumanize.Verify("acum 3 ore", 3, TimeUnit.Hour, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor20HoursAgo()
        {
            DateHumanize.Verify("acum 20 de ore", 20, TimeUnit.Hour, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor19MinutesAgo()
        {
            DateHumanize.Verify("acum 19 minute", 19, TimeUnit.Minute, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor60MinutesAgo()
        {
            DateHumanize.Verify("acum o oră", 60, TimeUnit.Minute, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor44MinutesAgo()
        {
            DateHumanize.Verify("acum 44 de minute", 44, TimeUnit.Minute, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor2SecondsAgo()
        {
            DateHumanize.Verify("acum 2 secunde", 2, TimeUnit.Second, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor59SecondsAgo()
        {
            DateHumanize.Verify("acum 59 de secunde", 59, TimeUnit.Second, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor10DaysAgo()
        {
            DateHumanize.Verify("acum 10 zile", 10, TimeUnit.Day, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor23DaysAgo()
        {
            DateHumanize.Verify("acum 23 de zile", 23, TimeUnit.Day, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor119YearsAgo()
        {
            DateHumanize.Verify("acum 119 ani", 119, TimeUnit.Year, Tense.Past);
        }

        [Fact]
        public void RomanianTranslationIsCorrectFor100YearsAgo()
        {
            DateHumanize.Verify("acum 100 de ani", 100, TimeUnit.Year, Tense.Past);
        }
    }
}