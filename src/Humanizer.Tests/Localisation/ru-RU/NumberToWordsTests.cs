using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ruRU
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("ru-RU") { }

        [InlineData(0, "ноль")]
        [InlineData(1, "один")]
        [InlineData(10, "десять")]
        [InlineData(11, "одиннадцать")]
        [InlineData(12, "двенадцать")]
        [InlineData(13, "тринадцать")]
        [InlineData(14, "четырнадцать")]
        [InlineData(15, "пятнадцать")]
        [InlineData(16, "шестнадцать")]
        [InlineData(17, "семнадцать")]
        [InlineData(18, "восемнадцать")]
        [InlineData(19, "девятнадцать")]
        [InlineData(20, "двадцать")]
        [InlineData(30, "тридцать")]
        [InlineData(40, "сорок")]
        [InlineData(50, "пятьдесят")]
        [InlineData(60, "шестьдесят")]
        [InlineData(70, "семьдесят")]
        [InlineData(80, "восемьдесят")]
        [InlineData(90, "девяносто")]
        [InlineData(100, "сто")]
        [InlineData(200, "двести")]
        [InlineData(300, "триста")]
        [InlineData(400, "четыреста")]
        [InlineData(500, "пятьсот")]
        [InlineData(600, "шестьсот")]
        [InlineData(700, "семьсот")]
        [InlineData(800, "восемьсот")]
        [InlineData(900, "девятьсот")]
        [InlineData(1000, "одна тысяча")]
        [InlineData(2000, "две тысячи")]
        [InlineData(3000, "три тысячи")]
        [InlineData(4000, "четыре тысячи")]
        [InlineData(5000, "пять тысячь")]
        [InlineData(10000, "десять тысячь")]
        [InlineData(100000, "сто тысячь")]
        [InlineData(1000000, "один миллион")]
        [InlineData(2000000, "два миллиона")]
        [InlineData(10000000, "десять миллионов")]
        [InlineData(100000000, "сто миллионов")]
        [InlineData(1000000000, "один миллиард")]
        [InlineData(2000000000, "два миллиарда")]
        //[InlineData(5000000000, "пять миллиардов")]

        [InlineData(122, "сто двадцать два")]
        [InlineData(3501, "три тысячи пятьсот один")]
        [InlineData(111, "сто одиннадцать")]
        [InlineData(1112, "одна тысяча сто двенадцать")]
        [InlineData(11213, "одиннадцать тысячь двести тринадцать")]
        [InlineData(121314, "сто двадцать одна тысяча триста четырнадцать")]
        [InlineData(2132415, "два миллиона сто тридцать две тысячи четыреста пятнадцать")]
        [InlineData(12345516, "двенадцать миллионов триста сорок пять тысячь пятьсот шестнадцать")]
        [InlineData(751633617, "семьсот пятьдесят один миллион шестьсот тридцать три тысячи шестьсот семнадцать")]
        [InlineData(1111111118, "один миллиард сто одиннадцать миллионов сто одиннадцать тысячь сто восемнадцать")]
        [InlineData(-751633617, "минус семьсот пятьдесят один миллион шестьсот тридцать три тысячи шестьсот семнадцать")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
