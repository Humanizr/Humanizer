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

        [Theory]
        [InlineData("صفر", 0)]
        [InlineData("واحد", 1)]
        [InlineData("اثنان", 2)]
        [InlineData("اثنان و عشرون", 22)]
        [InlineData("أحد عشر", 11)]
        [InlineData("ثلاثة آلاف و خمسمائة و واحد", 3501)]
        [InlineData("مليون و واحد", 1000001)]
        public void ToWordsArabic(string expected, int number)
        {
            Assert.Equal(expected, number.ToWords(NumeralSystem.Arabic));
        }
    }
}
