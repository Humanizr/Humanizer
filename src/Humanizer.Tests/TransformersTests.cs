using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class TransformersTests
    {
        [Theory]
        [InlineData("lower case statement", "Lower Case Statement")]
        [InlineData("Sentence casing", "Sentence Casing")]
        [InlineData("honors UPPER case", "Honors UPPER Case")]
        [InlineData("INvalid caSEs arE corrected", "Invalid Cases Are Corrected")]
        [InlineData("Can deal w 1 letter words as i do", "Can Deal W 1 Letter Words As I Do")]
        [InlineData("  random spaces   are HONORED    too ", "  Random Spaces   Are HONORED    Too ")]
        [InlineData("Title Case", "Title Case")]
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

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("some text", "some text")]
        [InlineData("+79091234567 (ext. 123)", "+79091234567 (ext. 123)")]
        [InlineData("++79091234567", "++79091234567")]
        public void TransformToPhoneNumberWhenItsNot(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.PhoneNumber));
        }

        [Theory]
        [InlineData("+79091234567", "+7 909 123-45-67")]
        [InlineData("89091234567", "8 909 123-45-67")]
        [InlineData("+7 909 12-34-567", "+7 909 123-45-67")]
        [InlineData("8 909 12-34-567", "8 909 123-45-67")]
        public void TransformToPhoneNumberRussianFormat(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Transform(To.PhoneNumber));
        }
    }
}