using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class NumberToWordsTests
    {
        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(122, "one hundred and twenty-two")]
        [InlineData(3501, "three thousand five hundred and one")]
        [InlineData(100, "one hundred")]
        [InlineData(1000, "one thousand")]
        [InlineData(100000, "one hundred thousand")]
        [InlineData(1000000, "one million")]
        [Theory]
        public void Test(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
