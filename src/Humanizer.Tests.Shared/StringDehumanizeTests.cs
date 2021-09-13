using Xunit;

namespace Humanizer.Tests
{
    public class StringDehumanizeTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("Pascal case sentence is pascalized", "PascalCaseSentenceIsPascalized")]
        [InlineData("Title Case Sentence Is Pascalized", "TitleCaseSentenceIsPascalized")]
        [InlineData("Mixed case sentence Is Pascalized", "MixedCaseSentenceIsPascalized")]
        [InlineData("lower case sentence is pascalized", "LowerCaseSentenceIsPascalized")]
        [InlineData("A special character is removed?", "ASpecialCharacterIsRemoved")]
        [InlineData("A special character is removed after a space ?", "ASpecialCharacterIsRemovedAfterASpace")]
        [InlineData("Internal special characters ?)@ are removed", "InternalSpecialCharactersAreRemoved")]
        [InlineData("AlreadyDehumanizedStringIsUntouched", "AlreadyDehumanizedStringIsUntouched")]
        [InlineData("CanDehumanizeIntoAPascalCaseWord", "CanDehumanizeIntoAPascalCaseWord")]
        [InlineData("CanDehumanizeIntoAPascalCaseWord AndAnother", "CanDehumanizeIntoAPascalCaseWordAndAnother")]
        [InlineData("OneAndTwo", "OneAndTwo")]
        [InlineData("OneOrTwo", "OneOrTwo")]
        [InlineData("OneOfTwo", "OneOfTwo")]
        [InlineData("OneButTwo", "OneButTwo")]
        [InlineData("OneATwo", "OneATwo")]
        [InlineData("OneAsTwo", "OneAsTwo")]
        [InlineData("OneYetTwo", "OneYetTwo")]
        [InlineData("OneNorTwo", "OneNorTwo")]
        [InlineData("WordSoTwo", "WordSoTwo")]
        public void CanDehumanizeIntoAPascalCaseWord(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Dehumanize());
        }
    }
}
