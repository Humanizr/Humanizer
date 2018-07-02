using Xunit;

namespace Humanizer.Tests.Localisation.invariant
{
    [UseCulture("")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(122)]
        [InlineData(3501)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        [InlineData(100000000)]
        [InlineData(1000000000)]
        [InlineData(111)]
        [InlineData(1111)]
        [InlineData(111111)]
        [InlineData(1111111)]
        [InlineData(11111111)]
        [InlineData(111111111)]
        [InlineData(1111111111)]
        [InlineData(123)]
        [InlineData(1234)]
        [InlineData(12345)]
        [InlineData(123456)]
        [InlineData(1234567)]
        [InlineData(12345678)]
        [InlineData(123456789)]
        [InlineData(1234567890)]
        public void ToWords(int number)
        {
            Assert.Equal(number.ToString(), number.ToWords());
        }
    }
}
