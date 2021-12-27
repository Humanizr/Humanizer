using Xunit;

namespace Humanizer.Tests.Localisation.koKR
{
    [UseCulture("ko-KR")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "영")]
        [InlineData(1, "일")]
        [InlineData(10, "십")]
        [InlineData(11, "십일")]
        [InlineData(122, "백이십이")]
        [InlineData(3501, "삼천오백일")]
        [InlineData(100, "백")]
        [InlineData(1000, "천")]
        [InlineData(10000, "일만")]
        [InlineData(100000, "십만")]
        [InlineData(1000000, "백만")]
        [InlineData(10000000, "천만")]
        [InlineData(100000000, "일억")]
        [InlineData(1000000000, "십억")]
        [InlineData(111, "백십일")]
        [InlineData(1111, "천백십일")]
        [InlineData(11111, "일만천백십일")]
        [InlineData(111111, "십일만천백십일")]
        [InlineData(1111111, "백십일만천백십일")]
        [InlineData(11111111, "천백십일만천백십일")]
        [InlineData(111111111, "일억천백십일만천백십일")]
        [InlineData(1111111111, "십일억천백십일만천백십일")]
        [InlineData(123, "백이십삼")]
        [InlineData(1234, "천이백삼십사")]
        [InlineData(12345, "일만이천삼백사십오")]
        [InlineData(123456, "십이만삼천사백오십육")]
        [InlineData(1234567, "백이십삼만사천오백육십칠")]
        [InlineData(12345678, "천이백삼십사만오천육백칠십팔")]
        [InlineData(123456789, "일억이천삼백사십오만육천칠백팔십구")]
        [InlineData(1234567890, "십이억삼천사백오십육만칠천팔백구십")]
        [InlineData(-123, "마이너스 백이십삼")]
        public void ToWordsInt(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1L, "일")]
        [InlineData(11L, "십일")]
        [InlineData(111L, "백십일")]
        [InlineData(1111L, "천백십일")]
        [InlineData(11111L, "일만천백십일")]
        [InlineData(111111L, "십일만천백십일")]
        [InlineData(1111111L, "백십일만천백십일")]
        [InlineData(11111111L, "천백십일만천백십일")]
        [InlineData(111111111L, "일억천백십일만천백십일")]
        [InlineData(1111111111L, "십일억천백십일만천백십일")]
        [InlineData(11111111111L, "백십일억천백십일만천백십일")]
        [InlineData(111111111111L, "천백십일억천백십일만천백십일")]
        [InlineData(1111111111111L, "일조천백십일억천백십일만천백십일")]
        [InlineData(11111111111111L, "십일조천백십일억천백십일만천백십일")]
        [InlineData(111111111111111L, "백십일조천백십일억천백십일만천백십일")]
        [InlineData(1111111111111111L, "천백십일조천백십일억천백십일만천백십일")]
        [InlineData(11111111111111111L, "일경천백십일조천백십일억천백십일만천백십일")]
        [InlineData(111111111111111111L, "십일경천백십일조천백십일억천백십일만천백십일")]
        [InlineData(1111111111111111111L, "백십일경천백십일조천백십일억천백십일만천백십일")]
        public void ToWordsLong(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "영번째")]
        [InlineData(1, "첫번째")]
        [InlineData(2, "두번째")]
        [InlineData(3, "세번째")]
        [InlineData(10, "열번째")]
        [InlineData(11, "열한번째")]
        [InlineData(100, "백번째")]
        [InlineData(112, "백십이번째")]
        [InlineData(1000000, "백만번째")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
