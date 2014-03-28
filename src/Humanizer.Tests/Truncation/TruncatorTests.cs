using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Truncation
{
    public class TruncatorTests
    {
        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("Uses fixed length truncator", 10, "Uses fixe…")]
        [InlineData("Uses default truncation string", 10, "Uses defa…")]
        public void TruncateWithoutTruncationStringAndTruncator(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length));
        }

        [Theory]
        [InlineData(null, 10, null)]
        [InlineData("Uses specified truncator", 2, "Uses specified…")]
        [InlineData("Uses default truncation string", 2, "Uses default…")]
        public void TruncateWithoutTruncationString(string input, int length, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, Truncator.FixedNumberOfWords));
        }

        [Theory]
        [InlineData(null, 10, "---", null)]
        [InlineData("Uses fixed length truncator", 10, "---", "Uses fi---")]
        [InlineData("Uses default truncation string", 10, "---", "Uses de---")]
        public void TruncateWithoutTruncator(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString));
        }

        [Theory]
        [InlineData(null, 10, "---", null)]
        [InlineData("Uses specified truncator", 2, "---", "Uses specified---")]
        [InlineData("Uses default truncation string", 2, "---", "Uses default---")]
        public void Truncate(string input, int length, string truncationString, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Truncate(length, truncationString, Truncator.FixedNumberOfWords));
        }

        [Fact]
        public void FixedLengthReturnsFixedLengthTruncator()
        {
            Assert.IsType<FixedLengthTruncator>(Truncator.FixedLength);
        }

        [Fact]
        public void FixedNumberOfCharactersReturnsFixedNumberOfCharactersTruncator()
        {
            Assert.IsType<FixedNumberOfCharactersTruncator>(Truncator.FixedNumberOfCharacters);
        }

        [Fact]
        public void FixedNumberOfWordsReturnsFixedNumberOfWordsTruncator()
        {
            Assert.IsType<FixedNumberOfWordsTruncator>(Truncator.FixedNumberOfWords);
        }
    }
}