using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class NumberToOrdinalWordsTests : AmbientCulture
    {
        public NumberToOrdinalWordsTests() : base("en") { }

        [Theory]
        [InlineData(0, "zeroth")]
        [InlineData(1, "first")]
        [InlineData(2, "second")]
        [InlineData(3, "third")]
        [InlineData(4, "fourth")]
        [InlineData(5, "fifth")]
        [InlineData(6, "sixth")]
        [InlineData(7, "seventh")]
        [InlineData(8, "eighth")]
        [InlineData(9, "ninth")]
        [InlineData(10, "tenth")]
        [InlineData(11, "eleventh")]
        [InlineData(12, "twelfth")]
        [InlineData(13, "thirteenth")]
        [InlineData(14, "fourteenth")]
        [InlineData(15, "fifteenth")]
        [InlineData(16, "sixteenth")]
        [InlineData(17, "seventeenth")]
        [InlineData(18, "eighteenth")]
        [InlineData(19, "nineteenth")]
        [InlineData(20, "twentieth")]
        [InlineData(21, "twenty first")]
        [InlineData(22, "twenty second")]
        [InlineData(30, "thirtieth")]
        [InlineData(40, "fortieth")]
        [InlineData(50, "fiftieth")]
        [InlineData(60, "sixtieth")]
        [InlineData(70, "seventieth")]
        [InlineData(80, "eightieth")]
        [InlineData(90, "ninetieth")]
        [InlineData(95, "ninety fifth")]
        [InlineData(96, "ninety sixth")]
        [InlineData(100, "hundredth")]
        [InlineData(120, "hundred and twentieth")]
        [InlineData(121, "hundred and twenty first")]
        [InlineData(1000, "thousandth")]
        [InlineData(1001, "thousand first")]
        [InlineData(1021, "thousand and twenty first")]
        [InlineData(10000, "ten thousandth")]
        [InlineData(10121, "ten thousand one hundred and twenty first")]
        [InlineData(100000, "hundred thousandth")]
        [InlineData(1000000, "millionth")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
