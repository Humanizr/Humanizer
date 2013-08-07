using Xunit;

namespace Humanizer.Tests.Extensions
{
    class StringDehumanizeTests
    {
        [Fact]
        public void PascalCaseSentence()
        {
            Assert.Equal(
                "PascalCaseSentenceIsCamelized",
                "Pascal case sentence is camelized".Dehumanize());
        }

        [Fact]
        public void TitleCaseSentence()
        {
            Assert.Equal(
                "TitleCaseSentenceIsCamelized",
                "Title Case Sentence Is Camelized".Dehumanize());
        }

        [Fact]
        public void MixedCaseSentence()
        {
            Assert.Equal(
                "MixedCaseSentenceIsCamelized",
                "Mixed case sentence Is Camelized".Dehumanize());
        }

        [Fact]
        public void LowerCaseSentence()
        {
            Assert.Equal(
                "LowerCaseSentenceIsCamelized",
                "lower case sentence is camelized".Dehumanize());
        }

        [Fact]
        public void EmptySentence()
        {
            Assert.Equal(
                "",
                "".Dehumanize());
        }
    }
}
