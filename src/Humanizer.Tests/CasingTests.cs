using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class CasingTests
    {
        [Theory]
        [InlineData("lower case statement", "Lower Case Statement")]
        [InlineData("Sentence casing", "Sentence Casing")]
        [InlineData("honors UPPER case", "Honors UPPER Case")]
        [InlineData("Title Case", "Title Case")]
        public void ApplyCaseTitle(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.ApplyCase(LetterCasing.Title));
        }
    }
}