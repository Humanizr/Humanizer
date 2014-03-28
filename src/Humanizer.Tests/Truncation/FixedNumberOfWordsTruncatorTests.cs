using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Truncation
{
    public class FixedNumberOfWordsTruncatorTests
    {
        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more words than truncate length", 4, "...", "Text with more words...")]
        [InlineData("Text with different truncation string", 4, "---", "Text with different truncation---")]
        [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "Words are\nsplit\rby...")]
        [InlineData("Null truncatation string truncates to truncate length without truncation string", 4, null, "Null truncatation string truncates")]
        public void Truncate(string input, int length, string truncatationString, string expectedOutput)
        {
            var truncatedString = new FixedNumberOfWordsTruncator().Truncate(input, length, truncatationString);

            Assert.Equal(expectedOutput, truncatedString);
        }
    }
}