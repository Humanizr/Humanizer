using Xunit;

namespace Humanizer.Tests
{
    public class TruncatorTests
    {
        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text longer than truncate length", 10, "Text long…")]
        [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
        public void Truncate(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text longer than truncate length", 10, "Text long…")]
        [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
        public void TruncateWithFixedLengthTruncator(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedLength));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more characters than truncate length", 10, "Text with m…")]
        [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
        public void TruncateWithFixedNumberOfCharactersTruncator(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfCharacters));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more words than truncate length", 4, "Text with more words…")]
        [InlineData("Text with number of words equal to truncate length", 9, "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "Words are\nsplit\rby…")]
        public void TruncateWithFixedNumberOfWordsTruncator(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfWords));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text longer than truncate length", 10, "...", "Text lo...")]
        [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
        public void TruncateWithTruncationString(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text longer than truncate length", 10, "...", "Text lo...")]
        [InlineData("Text with different truncation string", 10, "---", "Text wi---")]
        [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null")]
        public void TruncateWithTruncationStringAndFixedLengthTruncator(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more characters than truncate length", 10, "...", "Text wit...")]
        [InlineData("Text with different truncation string", 10, "---", "Text wit---")]
        [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "Te")]
        [InlineData("Text     with additional spaces and null truncate string", 10, null, "Text     with ad")]
        [InlineData("Text     with additional spaces and empty string as truncate string", 10, "", "Text     with ad")]
        public void TruncateWithTruncationStringAndFixedNumberOfCharactersTruncator(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfCharacters));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more words than truncate length", 4, "...", "Text with more words...")]
        [InlineData("Text with different truncation string", 4, "---", "Text with different truncation---")]
        [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "Words are\nsplit\rby...")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "Null truncation string truncates")]
        public void TruncateWithTruncationStringAndFixedNumberOfWordsTruncator(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text longer than truncate length", 10, "…te length")]
        [InlineData("Text with length equal to truncate length", 41, "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "Text smaller than truncate length")]
        public void TruncateWithFixedLengthTruncatorTruncateFromLeft(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedLength, TruncateFrom.Left));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more characters than truncate length", 10, "…ate length")]
        [InlineData("Text with number of characters equal to truncate length", 47, "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "Text with less characters than truncate length")]
        [InlineData("Text with strange characters ^$(*^ and more ^$**)%  ", 10, "…rs ^$(*^ and more ^$**)%  ")]
        public void TruncateWithFixedNumberOfCharactersTruncatorTruncateFromLeft(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfCharacters, TruncateFrom.Left));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("", 10, "")]
        [InlineData("a", 1, "a")]
        [InlineData("Text with more words than truncate length", 4, "…words than truncate length")]
        [InlineData("Text with number of words equal to truncate length", 9, "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "…are\nsplit\rby\twhitespace")]
        [InlineData("Text with whitespace at the end  ", 4, "…whitespace at the end")]
        public void TruncateWithFixedNumberOfWordsTruncatorTruncateFromLeft(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfWords, TruncateFrom.Left));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text longer than truncate length", 10, "...", "... length")]
        [InlineData("Text with different truncation string", 10, "---", "--- string")]
        [InlineData("Text with length equal to truncate length", 41, "...", "Text with length equal to truncate length")]
        [InlineData("Text smaller than truncate length", 34, "...", "Text smaller than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed length without truncation string", 2, "...", "ng")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "ring")]
        public void TruncateWithTruncationStringAndFixedLengthTruncatorTruncateFromLeft(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedLength, TruncateFrom.Left));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more characters than truncate length", 10, "...", "...e length")]
        [InlineData("Text with different truncation string", 10, "---", "---n string")]
        [InlineData("Text with number of characters equal to truncate length", 47, "...", "Text with number of characters equal to truncate length")]
        [InlineData("Text with less characters than truncate length", 41, "...", "Text with less characters than truncate length")]
        [InlineData("Text with delimiter length greater than truncate length truncates to fixed number of characters without truncation string", 2, "...", "ng")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "ring")]
        public void TruncateWithTruncationStringAndFixedNumberOfCharactersTruncatorTruncateFromLeft(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfCharacters, TruncateFrom.Left));
        }

        [Theory]
        [InlineData(null, 10, "...", null)]
        [InlineData("", 10, "...", "")]
        [InlineData("a", 1, "...", "a")]
        [InlineData("Text with more words than truncate length", 4, "...", "...words than truncate length")]
        [InlineData("Text with different truncation string", 4, "---", "---with different truncation string")]
        [InlineData("Text with number of words equal to truncate length", 9, "...", "Text with number of words equal to truncate length")]
        [InlineData("Text with less words than truncate length", 8, "...", "Text with less words than truncate length")]
        [InlineData("Words are\nsplit\rby\twhitespace", 4, "...", "...are\nsplit\rby\twhitespace")]
        [InlineData("Null truncation string truncates to truncate length without truncation string", 4, null, "length without truncation string")]
        [InlineData("Text with whitespace at the end  ", 4, "...", "...whitespace at the end")]
        public void TruncateWithTruncationStringAndFixedNumberOfWordsTruncatorTruncateFromLeft(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords, TruncateFrom.Left));
        }
    }
}