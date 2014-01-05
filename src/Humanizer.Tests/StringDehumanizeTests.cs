using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class StringDehumanizeTests
    {
        [Theory]
        [InlineData("Pascal case sentence is camelized", "PascalCaseSentenceIsCamelized")]
        [InlineData("Title Case Sentence Is Camelized", "TitleCaseSentenceIsCamelized")]
        [InlineData("Mixed case sentence Is Camelized", "MixedCaseSentenceIsCamelized")]
        [InlineData("lower case sentence is camelized", "LowerCaseSentenceIsCamelized")]
        [InlineData("", "")]
        public void CanDehumanizeIntoAPascalCaseWord(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Dehumanize());
        }
    }
}
