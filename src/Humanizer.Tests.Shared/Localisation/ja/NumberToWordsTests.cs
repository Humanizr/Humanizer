using Xunit;

namespace Humanizer.Tests.Localisation.ja
{
    [UseCulture("ja")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "〇")]
        [InlineData(1, "一")]
        [InlineData(10, "十")]
        [InlineData(11, "十一")]
        [InlineData(122, "百二十二")]
        [InlineData(3501, "三千五百一")]
        [InlineData(100, "百")]
        [InlineData(1000, "千")]
        [InlineData(10000, "一万")]
        [InlineData(100000, "十万")]
        [InlineData(1000000, "百万")]
        [InlineData(10000000, "千万")]
        [InlineData(100000000, "一億")]
        [InlineData(1000000000, "十億")]
        [InlineData(111, "百十一")]
        [InlineData(1111, "千百十一")]
        [InlineData(11111, "一万千百十一")]
        [InlineData(111111, "十一万千百十一")]
        [InlineData(1111111, "百十一万千百十一")]
        [InlineData(11111111, "千百十一万千百十一")]
        [InlineData(111111111, "一億千百十一万千百十一")]
        [InlineData(1111111111, "十一億千百十一万千百十一")]
        [InlineData(123, "百二十三")]
        [InlineData(1234, "千二百三十四")]
        [InlineData(12345, "一万二千三百四十五")]
        [InlineData(123456, "十二万三千四百五十六")]
        [InlineData(1234567, "百二十三万四千五百六十七")]
        [InlineData(12345678, "千二百三十四万五千六百七十八")]
        [InlineData(123456789, "一億二千三百四十五万六千七百八十九")]
        [InlineData(1234567890, "十二億三千四百五十六万七千八百九十")]
        [InlineData(-123, "マイナス 百二十三")]
        public void ToWordsInt(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1L, "一")]
        [InlineData(11L, "十一")]
        [InlineData(111L, "百十一")]
        [InlineData(1111L, "千百十一")]
        [InlineData(11111L, "一万千百十一")]
        [InlineData(111111L, "十一万千百十一")]
        [InlineData(1111111L, "百十一万千百十一")]
        [InlineData(11111111L, "千百十一万千百十一")]
        [InlineData(111111111L, "一億千百十一万千百十一")]
        [InlineData(1111111111L, "十一億千百十一万千百十一")]
        [InlineData(11111111111L, "百十一億千百十一万千百十一")]
        [InlineData(111111111111L, "千百十一億千百十一万千百十一")]
        [InlineData(1111111111111L, "一兆千百十一億千百十一万千百十一")]
        [InlineData(11111111111111L, "十一兆千百十一億千百十一万千百十一")]
        [InlineData(111111111111111L, "百十一兆千百十一億千百十一万千百十一")]
        [InlineData(1111111111111111L, "千百十一兆千百十一億千百十一万千百十一")]
        [InlineData(11111111111111111L, "一京千百十一兆千百十一億千百十一万千百十一")]
        [InlineData(111111111111111111L, "十一京千百十一兆千百十一億千百十一万千百十一")]
        [InlineData(1111111111111111111L, "百十一京千百十一兆千百十一億千百十一万千百十一")]
        public void ToWordsLong(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "〇番目")]
        [InlineData(1, "一番目")]
        [InlineData(2, "二番目")]
        [InlineData(3, "三番目")]
        [InlineData(10, "十番目")]
        [InlineData(11, "十一番目")]
        [InlineData(100, "百番目")]
        [InlineData(112, "百十二番目")]
        [InlineData(1000000, "百万番目")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
