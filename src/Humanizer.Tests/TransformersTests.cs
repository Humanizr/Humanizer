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
        [InlineData("Title Case", "Title Case")]
        public void TransformToTitleCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.TransformWith(Transformers.ToTitleCase));
        }

        [Theory]
        [InlineData("lower case statement", "lower case statement")]
        [InlineData("Sentence casing", "sentence casing")]
        [InlineData("No honor for UPPER case", "no honor for upper case")]
        [InlineData("Title Case", "title case")]
        public void TransformToLowerCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.TransformWith(Transformers.ToLowerCase));
        }

        [Theory]
        [InlineData("lower case statement", "Lower case statement")]
        [InlineData("Sentence casing", "Sentence casing")]
        [InlineData("honors UPPER case", "Honors UPPER case")]
        public void TransformToSentenceCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.TransformWith(Transformers.ToSentenceCase));
        }

        [Theory]
        [InlineData("lower case statement", "LOWER CASE STATEMENT")]
        [InlineData("Sentence casing", "SENTENCE CASING")]
        [InlineData("Title Case", "TITLE CASE")]
        public void TransformToUpperCase(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.TransformWith(Transformers.ToUpperCase));
        }
    }
}