using System.Globalization;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.it
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("it")
        {
        }

        [InlineData(1, "uno")]
        [InlineData(10, "dieci")]
        [InlineData(11, "undici")]
        [InlineData(122, "centoventidue")]
        [InlineData(3501, "tremilacinquecentouno")]
        [InlineData(100, "cento")]
        [InlineData(1000, "mille")]
        [InlineData(100000, "centomila")]
        [InlineData(1000000, "unmilione")]
        [InlineData(10000000, "diecimilioni")]
        [InlineData(100000000, "centomilioni")]
        [InlineData(1000000000, "unmiliardo")]
        [InlineData(111, "centoundici")]
        [InlineData(1111, "millecentoundici")]
        [InlineData(111111, "centoundicimilacentoundici")]
        [InlineData(1111111, "unmilionecentoundicimilacentoundici")]
        [InlineData(11111111, "undicimilionicentoundicimilacentoundici")]
        [InlineData(111111111, "centoundicimilionicentoundicimilacentoundici")]
        [InlineData(1111111111, "unmiliardocentoundicimilionicentoundicimilacentoundici")]
        [InlineData(123, "centoventitre")]
        [InlineData(1234, "milleduecentotrentaquattro")]
        [InlineData(12345, "dodicimilatrecentoquarantacinque")]
        [InlineData(123456, "centoventitremilaquattrocentocinquantasei")]
        [InlineData(1234567, "unmilioneduecentotrentaquattromilacinquecentosessantasette")]
        [InlineData(12345678, "dodicimilionitrecentoquarantacinquemilaseicentosettantotto")]
        [InlineData(123456789, "centoventitremilioniquattrocentocinquantaseimilasettecentoottantanove")]
        [InlineData(1234567890, "unmiliardoduecentotrentaquattromilionicinquecentosessantasettemilaottocentonovanta")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
