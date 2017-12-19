﻿using System.Globalization;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
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

        [InlineData(1L, "one")]
        [InlineData(11L, "eleven")]
        [InlineData(111L, "one hundred and eleven")]
        [InlineData(1111L, "one thousand one hundred and eleven")]
        [InlineData(11111L, "eleven thousand one hundred and eleven")]
        [InlineData(111111L, "one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111L, "one million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111L, "eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111L, "one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111L, "one billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111111L, "eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111111L, "one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111111L, "one trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111111111L, "eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111111111L, "one hundred and eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111111111L, "one quadrillion one hundred and eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111111111111L, "eleven quadrillion one hundred and eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111111111111L, "one hundred and eleven quadrillion one hundred and eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111111111111L, "one quintillion one hundred and eleven quadrillion one hundred and eleven trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [Theory]
        public void ToWordsLong(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

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
        [InlineData(21, "twenty-first")]
        [InlineData(22, "twenty-second")]
        [InlineData(30, "thirtieth")]
        [InlineData(40, "fortieth")]
        [InlineData(50, "fiftieth")]
        [InlineData(60, "sixtieth")]
        [InlineData(70, "seventieth")]
        [InlineData(80, "eightieth")]
        [InlineData(90, "ninetieth")]
        [InlineData(95, "ninety-fifth")]
        [InlineData(96, "ninety-sixth")]
        [InlineData(100, "hundredth")]
        [InlineData(112, "hundred and twelfth")]
        [InlineData(120, "hundred and twentieth")]
        [InlineData(121, "hundred and twenty-first")]
        [InlineData(1000, "thousandth")]
        [InlineData(1001, "thousand and first")]
        [InlineData(1021, "thousand and twenty-first")]
        [InlineData(10000, "ten thousandth")]
        [InlineData(10121, "ten thousand one hundred and twenty-first")]
        [InlineData(100000, "hundred thousandth")]
        [InlineData(1000000, "millionth")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }

        [Theory]
        [InlineData(11, "en-US", "eleven")]
        [InlineData(22, "ar", "اثنان و عشرون")]
        [InlineData(40, "ru", "сорок")]
        public void ToWords_CanSpecifyCultureExplicitly(int number, string culture, string expected)
        {
            Assert.Equal(expected, number.ToWords(new CultureInfo(culture)));
        }

        [Theory]
        [InlineData(1021, "en-US", "thousand and twenty-first")]
        [InlineData(21, "ar", "الحادي و العشرون")]
        [InlineData(1112, "ru", "одна тысяча сто двенадцатый")]
        public void ToOrdinalWords_CanSpecifyCultureExplicitly(int number, string culture, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords(new CultureInfo(culture)));
        }
    }
}
