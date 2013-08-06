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
                "Pascal case sentence is camelized".Duhumanize());
        }

        [Fact]
        public void TitleCaseSentence()
        {
            Assert.Equal(
                "TitleCaseSentenceIsCamelized",
                "Title Case Sentence Is Camelized".Duhumanize());
        }

        [Fact]
        public void MixedCaseSentence()
        {
            Assert.Equal(
                "MixedCaseSentenceIsCamelized",
                "Mixed case sentence Is Camelized".Duhumanize());
        }

        [Fact]
        public void LowerCaseSentence()
        {
            Assert.Equal(
                "LowerCaseSentenceIsCamelized",
                "lower case sentence is camelized".Duhumanize());
        }

        [Fact]
        public void EmptySentence()
        {
            Assert.Equal(
                "",
                "".Duhumanize());
        }
    }
}
