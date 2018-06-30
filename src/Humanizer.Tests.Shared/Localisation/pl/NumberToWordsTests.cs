using Xunit;

namespace Humanizer.Tests.Localisation.pl
{
    [UseCulture("pl")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "jeden")]
        [InlineData(2, "dwa")]
        [InlineData(3, "trzy")]
        [InlineData(4, "cztery")]
        [InlineData(5, "pięć")]
        [InlineData(6, "sześć")]
        [InlineData(7, "siedem")]
        [InlineData(8, "osiem")]
        [InlineData(9, "dziewięć")]
        [InlineData(10, "dziesięć")]
        [InlineData(11, "jedenaście")]
        [InlineData(12, "dwanaście")]
        [InlineData(13, "trzynaście")]
        [InlineData(14, "czternaście")]
        [InlineData(15, "piętnaście")]
        [InlineData(16, "szesnaście")]
        [InlineData(17, "siedemnaście")]
        [InlineData(18, "osiemnaście")]
        [InlineData(19, "dziewiętnaście")]
        [InlineData(20, "dwadzieścia")]
        [InlineData(30, "trzydzieści")]
        [InlineData(40, "czterdzieści")]
        [InlineData(50, "pięćdziesiąt")]
        [InlineData(60, "sześćdziesiąt")]
        [InlineData(70, "siedemdziesiąt")]
        [InlineData(80, "osiemdziesiąt")]
        [InlineData(90, "dziewięćdziesiąt")]
        [InlineData(100, "sto")]
        [InlineData(112, "sto dwanaście")]
        [InlineData(128, "sto dwadzieścia osiem")]
        [InlineData(1000, "tysiąc")]
        [InlineData(2000, "dwa tysiące")]
        [InlineData(5000, "pięć tysięcy")]
        [InlineData(10000, "dziesięć tysięcy")]
        [InlineData(20000, "dwadzieścia tysięcy")]
        [InlineData(22000, "dwadzieścia dwa tysiące")]
        [InlineData(25000, "dwadzieścia pięć tysięcy")]
        [InlineData(100000, "sto tysięcy")]
        [InlineData(500000, "pięćset tysięcy")]
        [InlineData(1000000, "milion")]
        [InlineData(2000000, "dwa miliony")]
        [InlineData(5000000, "pięć milionów")]
        [InlineData(1000000000, "miliard")]
        [InlineData(2000000000, "dwa miliardy")]
        [InlineData(1501001892, "miliard pięćset jeden milionów tysiąc osiemset dziewięćdziesiąt dwa")]
        [InlineData(2147483647, "dwa miliardy sto czterdzieści siedem milionów czterysta osiemdziesiąt trzy tysiące sześćset czterdzieści siedem")]
        [InlineData(-1501001892, "minus miliard pięćset jeden milionów tysiąc osiemset dziewięćdziesiąt dwa")]
        public void ToWordsPolish(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
