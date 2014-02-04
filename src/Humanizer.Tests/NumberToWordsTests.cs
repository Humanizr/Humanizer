using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData("one", 1)]
        [InlineData("ten", 10)]
        [InlineData("eleven", 11)]
        [InlineData("one hundred and twenty-two", 122)]
        [InlineData("three thousand five hundred and one", 3501)]

        public void ToWords(string expected, int number)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData("one hundred", 100)]
        [InlineData("one thousand", 1000)]
        [InlineData("one hundred thousand", 100000)]
        [InlineData("one million", 1000000)]
        public void RoundNumbersHaveNoSpaceAtTheEnd(string expected, int number)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
