using System.Globalization;
using Xunit;

namespace Humanizer.Tests.Localisation.af
{
    [UseCulture("af")]
    public class AfrikaansNumberToWordsTests
    {

        [InlineData(1, "een")]
        [InlineData(10, "tien")]
        [InlineData(11, "elf")]
        [InlineData(20, "twintig")]
        [InlineData(122, "een honderd twee en twintig")]
        [InlineData(3501, "drie duisend vyf honderd en een")]
        [InlineData(100, "een honderd")]
        [InlineData(1000, "een duisend")]
        [InlineData(100000, "een honderd duisend")]
        [InlineData(1000000, "een miljoen")]
        [InlineData(10000000, "tien miljoen")]
        [InlineData(100000000, "een honderd miljoen")]
        [InlineData(1000000000, "een miljard")]
        [InlineData(111, "een honderd en elf")]
        [InlineData(1111, "een duisend een honderd en elf")]
        [InlineData(111111, "een honderd en elf duisend een honderd en elf")]
        [InlineData(1111111, "een miljoen een honderd en elf duisend een honderd en elf")]
        [InlineData(11111111, "elf miljoen een honderd en elf duisend een honderd en elf")]
        [InlineData(111111111, "een honderd en elf miljoen een honderd en elf duisend een honderd en elf")]
        [InlineData(1111111111, "een miljard een honderd en elf miljoen een honderd en elf duisend een honderd en elf")]
        [InlineData(123, "een honderd drie en twintig")]
        [InlineData(1234, "een duisend twee honderd vier en dertig")]
        [InlineData(12345, "twaalf duisend drie honderd vyf en veertig")]
        [InlineData(123456, "een honderd drie en twintig duisend vier honderd ses en vyftig")]
        [InlineData(1234567, "een miljoen twee honderd vier en dertig duisend vyf honderd sewe en sestig")]
        [InlineData(12345678, "twaalf miljoen drie honderd vyf en veertig duisend ses honderd agt en sewentig")]
        [InlineData(123456789, "een honderd drie en twintig miljoen vier honderd ses en vyftig duisend sewe honderd nege en tagtig")]
        [InlineData(1234567890, "een miljard twee honderd vier en dertig miljoen vyf honderd sewe en sestig duisend agt honderd en negentig")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "nulste")]
        [InlineData(1, "eerste")]
        [InlineData(2, "tweede")]
        [InlineData(3, "derde")]
        [InlineData(4, "vierde")]
        [InlineData(5, "vyfde")]
        [InlineData(6, "sesde")]
        [InlineData(7, "sewende")]
        [InlineData(8, "agste")]
        [InlineData(9, "negende")]
        [InlineData(10, "tiende")]
        [InlineData(11, "elfde")]
        [InlineData(12, "twaalfde")]
        [InlineData(13, "dertiende")]
        [InlineData(14, "veertiende")]
        [InlineData(15, "vyftiende")]
        [InlineData(16, "sestiende")]
        [InlineData(17, "sewentiende")]
        [InlineData(18, "agtiende")]
        [InlineData(19, "negentiende")]
        [InlineData(20, "twintigste")]
        [InlineData(21, "een en twintigste")]
        [InlineData(22, "twee en twintigste")]
        [InlineData(30, "dertigste")]
        [InlineData(40, "veertigste")]
        [InlineData(50, "vyftigste")]
        [InlineData(60, "sestigste")]
        [InlineData(70, "sewentigste")]
        [InlineData(80, "tagtigste")]
        [InlineData(90, "negentigste")]
        [InlineData(95, "vyf en negentigste")]
        [InlineData(96, "ses en negentigste")]
        [InlineData(100, "honderdste")]
        [InlineData(112, "honderd en twaalfde")]
        [InlineData(120, "honderd en twintigste")]
        [InlineData(121, "honderd een en twintigste")]
        [InlineData(1000, "duisendste")]
        [InlineData(1001, "duisend en eerste")]
        [InlineData(1021, "duisend een en twintigste")]
        [InlineData(10000, "tien duisendste")]
        [InlineData(10121, "tien duisend een honderd een en twintigste")]
        [InlineData(100000, "honderd duisendste")]
        [InlineData(1000000, "miljoenste")]
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
