using Xunit;
using System.Globalization;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class WordsToNumberTests_US
    {
        [Theory]
        [InlineData("zero", 0)]
        [InlineData("one", 1)]
        [InlineData("minus five", -5)]
        [InlineData("eleven", 11)]
        [InlineData("ninety five", 95)]
        [InlineData("hundred five", 105)]
        [InlineData("one hundred ninety six", 196)]
        [InlineData("minus one hundred and five", -105)]
        [InlineData("seventeenth", 17)]
        [InlineData("thirtieth", 30)]
        [InlineData("twenty-seventh", 27)]
        [InlineData("thirty-first", 31)]
        [InlineData("minus twenty-first", -21)]
        [InlineData("two thousand twenty three", 2023)]
        [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567)]
        [InlineData("one hundred and third", 103)]
        [InlineData("two hundred and first", 201)]
        [InlineData("five thousand and ninth", 5009)]
        [InlineData("17th", 17)]
        [InlineData("31st", 31)]
        [InlineData("100th", 100)]
        [InlineData("203rd", 203)]
        [InlineData("minus 21st", -21)]
        [InlineData("negative five", -5)]
        [InlineData("negative one hundred and five", -105)]
        [InlineData("negative twenty-first", -21)]
        public void ToNumber_US(string words, int expectedNumber) => Assert.Equal(expectedNumber, words.ToNumber(CultureInfo.CurrentCulture));
    }

    [UseCulture("en-GB")]
    public class WordsToNumberTests_GB
    {
        [Theory]
        [InlineData("zero", 0)]
        [InlineData("one", 1)]
        [InlineData("minus five", -5)]
        [InlineData("eleven", 11)]
        [InlineData("ninety-five", 95)]
        [InlineData("hundred and five", 105)]
        [InlineData("one hundred and ninety-six", 196)]
        [InlineData("minus one hundred and five", -105)]
        [InlineData("seventeenth", 17)]
        [InlineData("thirtieth", 30)]
        [InlineData("twenty-seventh", 27)]
        [InlineData("thirty-first", 31)]
        [InlineData("minus twenty-first", -21)]
        [InlineData("two thousand and twenty-three", 2023)]
        [InlineData("one million, two hundred and thirty-four thousand, five hundred and sixty-seven", 1234567)]
        [InlineData("one hundred and third", 103)]
        [InlineData("two hundred and first", 201)]
        [InlineData("five thousand and ninth", 5009)]
        [InlineData("17th", 17)]
        [InlineData("31st", 31)]
        [InlineData("100th", 100)]
        [InlineData("203rd", 203)]
        [InlineData("minus 21st", -21)]
        [InlineData("negative five", -5)]
        [InlineData("negative one hundred and five", -105)]
        [InlineData("negative twenty-first", -21)]
        public void ToNumber_GB(string words, int expectedNumber) => Assert.Equal(expectedNumber, words.ToNumber(CultureInfo.CurrentCulture));
    }
    public class WordsToNumberTests_NonEnglish
    {
        [Theory]
        [InlineData("es-ES", "veinte")]
        [InlineData("fr-FR", "vingt")] 
        public void ThrowsForNonEnglishWords(string cultureName, string word)
        {
            var culture = new CultureInfo(cultureName);
            var ex = Assert.Throws<NotSupportedException>(() =>
                word.ToNumber(culture));

            Assert.Contains($"'{culture.TwoLetterISOLanguageName}'", ex.Message);
        }
    }
}
