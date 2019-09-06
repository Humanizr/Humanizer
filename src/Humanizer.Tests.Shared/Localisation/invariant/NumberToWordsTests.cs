using Xunit;

namespace Humanizer.Tests.Localisation.invariant
{
    [UseCulture("")]
    public class NumberToWordsTests
    {

        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(20, "twenty")]
        [InlineData(122, "one hundred and twenty-two")]
        [InlineData(3501, "three thousand five hundred and one")]
        [InlineData(100, "one hundred")]
        [InlineData(1000, "one thousand")]
        [InlineData(100000, "one hundred thousand")]
        [InlineData(1000000, "one million")]
        [InlineData(10000000, "ten million")]
        [InlineData(100000000, "one hundred million")]
        [InlineData(1000000000, "one billion")]
        [InlineData(111, "one hundred and eleven")]
        [InlineData(1111, "one thousand one hundred and eleven")]
        [InlineData(111111, "one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111, "one million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111, "eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111, "one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111, "one billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(123, "one hundred and twenty-three")]
        [InlineData(1234, "one thousand two hundred and thirty-four")]
        [InlineData(12345, "twelve thousand three hundred and forty-five")]
        [InlineData(123456, "one hundred and twenty-three thousand four hundred and fifty-six")]
        [InlineData(1234567, "one million two hundred and thirty-four thousand five hundred and sixty-seven")]
        [InlineData(12345678, "twelve million three hundred and forty-five thousand six hundred and seventy-eight")]
        [InlineData(123456789, "one hundred and twenty-three million four hundred and fifty-six thousand seven hundred and eighty-nine")]
        [InlineData(1234567890, "one billion two hundred and thirty-four million five hundred and sixty-seven thousand eight hundred and ninety")]
        [Theory]
        public void ToWordsInt(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
