using Xunit;
namespace Humanizer.Tests.Extensions
{
    public class NumberToWordsTests
    {
        [Fact]
        public void ToWords()
        {
            Assert.Equal("one", 1.ToWords());
            Assert.Equal("ten", 10.ToWords());
            Assert.Equal("eleven", 11.ToWords());
            Assert.Equal("one hundred and twenty-two", 122.ToWords());
            Assert.Equal("three thousand five hundred and one", 3501.ToWords());
        }
    }
}
