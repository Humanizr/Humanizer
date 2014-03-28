using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Truncation
{
    public class FixedNumberOfCharactersTruncatorTests
    {
        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more characters than truncate length", 10, "...", "Text wit...")]
        [InlineData("Text with different truncation string", 10, "---", "Text wit---")]
        [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Null truncatation string truncates to truncate length without truncation string", 4, null, "Null")]
        public void Truncate(string input, int length, string truncatationString, string expectedOutput)
        {
            var truncatedString = new FixedNumberOfCharactersTruncator().Truncate(input, length, truncatationString);

            Assert.Equal(expectedOutput, truncatedString);
        }
    }
}