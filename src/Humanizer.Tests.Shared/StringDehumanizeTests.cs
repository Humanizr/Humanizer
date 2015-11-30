using Xunit;

namespace Humanizer.Tests
{
    public class StringDehumanizeTests
    {
        [Theory]
        [InlineData("Pascal case sentence is camelized", "PascalCaseSentenceIsCamelized")]
        [InlineData("Title Case Sentence Is Camelized", "TitleCaseSentenceIsCamelized")]
        [InlineData("Mixed case sentence Is Camelized", "MixedCaseSentenceIsCamelized")]
        [InlineData("lower case sentence is camelized", "LowerCaseSentenceIsCamelized")]
        [InlineData("AlreadyDehumanizedStringIsUntouched", "AlreadyDehumanizedStringIsUntouched")]
        [InlineData("", "")]
        [InlineData("A special character is removed?", "ASpecialCharacterIsRemoved")]
        [InlineData("A special character is removed after a space ?", "ASpecialCharacterIsRemovedAfterASpace")]
        [InlineData("Internal special characters ?)@ are removed", "InternalSpecialCharactersAreRemoved")]
        public void CanDehumanizeIntoAPascalCaseWord(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Dehumanize());
        }
    }
}
