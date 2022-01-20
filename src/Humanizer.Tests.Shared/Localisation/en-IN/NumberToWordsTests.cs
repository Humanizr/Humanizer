using Xunit;

namespace Humanizer.Tests.Localisation.enIN
{
    [UseCulture("en-IN")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(20, "twenty")]
        [InlineData(122, "one hundred and twenty two")]
        [InlineData(3501, "three thousand five hundred and one")]
        [InlineData(100, "one hundred")]
        [InlineData(1000, "one thousand")]
        [InlineData(1001, "one thousand one")]
        [InlineData(100000, "one lakh")]
        [InlineData(1000000, "ten lakh")]
        [InlineData(10000000, "one crore")]
        [InlineData(100000000, "ten crore")]
        [InlineData(1000000000, "one hundred crore")]
        [InlineData(111, "one hundred and eleven")]
        [InlineData(1111, "one thousand one hundred and eleven")]
        [InlineData(111111, "one lakh eleven thousand one hundred and eleven")]
        [InlineData(1111111, "eleven lakh eleven thousand one hundred and eleven")]
        [InlineData(11111111, "one crore eleven lakh eleven thousand one hundred and eleven")]
        [InlineData(111111111, "eleven crore eleven lakh eleven thousand one hundred and eleven")]
        [InlineData(1111111111, "one hundred and eleven crore eleven lakh eleven thousand one hundred and eleven")]
        [InlineData(101, "one hundred and one")]
        [InlineData(1011, "one thousand eleven")]
        [InlineData(100011, "one lakh eleven")]
        [InlineData(1100001, "eleven lakh one")]
        [InlineData(11000011, "one crore ten lakh eleven")]
        [InlineData(110000011, "eleven crore eleven")]
        [InlineData(1100000111, "one hundred and ten crore one hundred and eleven")]
        [InlineData(123, "one hundred and twenty three")]
        [InlineData(1234, "one thousand two hundred and thirty four")]
        [InlineData(12345, "twelve thousand three hundred and forty five")]
        [InlineData(123456, "one lakh twenty three thousand four hundred and fifty six")]
        [InlineData(1234567, "twelve lakh thirty four thousand five hundred and sixty seven")]
        [InlineData(12345678, "one crore twenty three lakh forty five thousand six hundred and seventy eight")]
        [InlineData(123456789, "twelve crore thirty four lakh fifty six thousand seven hundred and eighty nine")]
        [InlineData(1234567890, "one hundred and twenty three crore forty five lakh sixty seven thousand eight hundred and ninety")]
        [InlineData(1000000000000, "one lakh crore")]
        [InlineData(45678912345678, "forty five lakh sixty seven thousand eight hundred and ninety one crore twenty three lakh forty five thousand six hundred and seventy eight")]
        [InlineData(-7516, "(Negative) seven thousand five hundred and sixteen")]

        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1, "one")]
        [InlineData(3501, "three thousand five hundred and one")]
        public void ToWordsFeminine(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }
        
    }
}
