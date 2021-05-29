using Xunit;

namespace Humanizer.Tests
{
    public class TransformersTests
    {
        [Theory]
        [InlineData("lower case statement", "Lower Case Statement")]
        [InlineData("Sentence casing", "Sentence Casing")]
        [InlineData("honors UPPER case", "Honors UPPER Case")]
        [InlineData("INvalid caSEs arE corrected", "Invalid Cases Are Corrected")]
        [InlineData("Can deal w 1 letter words as i do", "Can Deal W 1 Letter Words as I Do")]
        [InlineData("  random spaces   are HONORED    too ", "  Random Spaces   Are HONORED    Too ")]
        [InlineData("Title Case", "Title Case")]
        [InlineData("apostrophe's aren't capitalized", "Apostrophe's Aren't Capitalized")]
        [InlineData("titles with, commas work too", "Titles With, Commas Work Too")]
        public void TransformToTitleCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.TitleCase));
        }

        [Theory]
        [InlineData("lower case statement", "lower case statement")]
        [InlineData("Sentence casing", "sentence casing")]
        [InlineData("No honor for UPPER case", "no honor for upper case")]
        [InlineData("Title Case", "title case")]
        public void TransformToLowerCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.LowerCase));
        }

        [Theory]
        [InlineData("lower case statement", "Lower case statement")]
        [InlineData("Sentence casing", "Sentence casing")]
        [InlineData("honors UPPER case", "Honors UPPER case")]
        public void TransformToSentenceCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.SentenceCase));
        }

        [Theory]
        [InlineData("lower case statement", "LOWER CASE STATEMENT")]
        [InlineData("Sentence casing", "SENTENCE CASING")]
        [InlineData("Title Case", "TITLE CASE")]
        public void TransformToUpperCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.UpperCase));
        }
    }
}
