namespace Humanizer.Tests
{
    using Xunit;
    using Xunit.Extensions;

    public class TruncateTests
    {
        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text longer than truncate length", 10, "Text long…")]
        [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
        public void TruncateFixedLength(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, TruncateMode.FixedLength));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text longer than truncate length", 10, "...", "Text lo...")]
        [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Null truncatation string truncates to truncate length without truncation string", 4, null, "Null")]
        public void TruncateFixedLengthWithTruncationString(string input, int length, string truncatationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncatationString, TruncateMode.FixedLength));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more characters than truncate length", 10, "Text with m…")]
        [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
        public void TruncateFixedNumberOfCharacters(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, TruncateMode.FixedNumberOfCharacters));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more characters than truncate length", 10, "...", "Text wit...")]
        [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Null truncatation string truncates to truncate length without truncation string", 4, null, "Null")]
        public void TruncateFixedNumberOfCharactersWithTruncationString(string input, int length, string truncatationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncatationString, TruncateMode.FixedNumberOfCharacters));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more words than truncate length", 4, "Text with more words…")]
        [InlineData("Text with number of words equal to truncate length", 9, "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "Words are\nsplit\rby…")]
        public void TruncateFixedNumberOfWords(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, TruncateMode.FixedNumberOfWords));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more words than truncate length", 4, "...", "Text with more words...")]
        [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "Words are\nsplit\rby...")]
        [InlineData("Null truncatation string truncates to truncate length without truncation string", 4, null, "Null truncatation string truncates")]
        public void TruncateFixedNumberOfWordsWithTruncationString(string input, int length, string truncatationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncatationString, TruncateMode.FixedNumberOfWords));
        }
    }
}