using Xunit;

namespace Humanizer.Tests.Localisation.it
{
    [UseCulture("it")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "uno")]
        [InlineData(-1, "meno uno")]
        [InlineData(3, "tre")]
        [InlineData(10, "dieci")]
        [InlineData(11, "undici")]
        [InlineData(21, "ventuno")]
        [InlineData(38, "trentotto")]
        [InlineData(122, "centoventidue")]
        [InlineData(3501, "tremilacinquecentouno")]
        [InlineData(-3501, "meno tremilacinquecentouno")]
        [InlineData(100, "cento")]
        [InlineData(1000, "mille")]
        [InlineData(2000, "duemila")]
        [InlineData(10000, "diecimila")]
        [InlineData(100000, "centomila")]
        [InlineData(1000000, "un milione")]
        [InlineData(5000000, "cinque milioni")]
        [InlineData(10000000, "dieci milioni")]
        [InlineData(100000000, "cento milioni")]
        [InlineData(1000000000, "un miliardo")]
        [InlineData(2000000000, "due miliardi")]
        [InlineData(2147483647, "due miliardi centoquarantasette milioni quattrocentoottantatremilaseicentoquarantasette")]  // int.MaxValue
        //[InlineData(9000000000, "nove miliardi")]  // int = System.Int32, fixed in API, is not big enough
        //[InlineData(10000000000, "dieci miliardi")]  // int = System.Int32, fixed in API, is not big enough
        //[InlineData(100000000000, "cento miliardi")]  // int = System.Int32, fixed in API, is not big enough
        [InlineData(101, "centouno")]
        [InlineData(1001, "milleuno")]
        [InlineData(10001, "diecimilauno")]
        [InlineData(100001, "centomilauno")]
        [InlineData(1000001, "un milione uno")]
        [InlineData(10000001, "dieci milioni uno")]
        [InlineData(100000001, "cento milioni uno")]
        [InlineData(1000000001, "un miliardo uno")]
        [InlineData(111, "centoundici")]
        [InlineData(1111, "millecentoundici")]
        [InlineData(111111, "centoundicimilacentoundici")]
        [InlineData(1111101, "un milione centoundicimilacentouno")]
        [InlineData(1111111, "un milione centoundicimilacentoundici")]
        [InlineData(11111111, "undici milioni centoundicimilacentoundici")]
        [InlineData(111111111, "centoundici milioni centoundicimilacentoundici")]
        [InlineData(1101111101, "un miliardo centouno milioni centoundicimilacentouno")]
        [InlineData(1111111111, "un miliardo centoundici milioni centoundicimilacentoundici")]
        [InlineData(8100, "ottomilacento")]
        [InlineData(43, "quarantatré")]  // Ref. http://dizionari.corriere.it/dizionario-si-dice/V/ventitre.shtml
        [InlineData(123, "centoventitré")]
        [InlineData(1234, "milleduecentotrentaquattro")]
        [InlineData(12345, "dodicimilatrecentoquarantacinque")]
        [InlineData(123456, "centoventitremilaquattrocentocinquantasei")]
        [InlineData(1234567, "un milione duecentotrentaquattromilacinquecentosessantasette")]
        [InlineData(12345678, "dodici milioni trecentoquarantacinquemilaseicentosettantotto")]
        [InlineData(123456789, "centoventitré milioni quattrocentocinquantaseimilasettecentoottantanove")]
        [InlineData(1234567890, "un miliardo duecentotrentaquattro milioni cinquecentosessantasettemilaottocentonovanta")]
        [InlineData(1999, "millenovecentonovantanove")]
        [InlineData(2014, "duemilaquattordici")]
        [InlineData(2048, "duemilaquarantotto")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "una")]
        [InlineData(-1, "meno una")]
        [InlineData(3, "tre")]
        [InlineData(21, "ventuno")]
        [InlineData(101, "centouno")]
        [InlineData(1001, "milleuno")]
        [InlineData(10001, "diecimilauno")]
        [InlineData(100001, "centomilauno")]
        public void ToFeminineWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "uno")]
        [InlineData(-1, "meno uno")]
        [InlineData(3, "tre")]
        [InlineData(21, "ventuno")]
        [InlineData(101, "centouno")]
        [InlineData(1001, "milleuno")]
        [InlineData(10001, "diecimilauno")]
        [InlineData(100001, "centomilauno")]
        public void ToMasculineWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "primo")]
        [InlineData(2, "secondo")]
        [InlineData(9, "nono")]
        [InlineData(10, "decimo")]
        [InlineData(11, "undicesimo")]
        [InlineData(15, "quindicesimo")]
        [InlineData(18, "diciottesimo")]
        [InlineData(20, "ventesimo")]
        [InlineData(21, "ventunesimo")]
        [InlineData(22, "ventiduesimo")]
        [InlineData(28, "ventottesimo")]
        [InlineData(30, "trentesimo")]
        [InlineData(44, "quarantaquattresimo")]
        [InlineData(55, "cinquantacinquesimo")]
        [InlineData(60, "sessantesimo")]
        [InlineData(63, "sessantatreesimo")]
        [InlineData(66, "sessantaseiesimo")]
        [InlineData(77, "settantasettesimo")]
        [InlineData(88, "ottantottesimo")]
        [InlineData(99, "novantanovesimo")]
        [InlineData(100, "centesimo")]
        [InlineData(101, "centounesimo")]
        [InlineData(102, "centoduesimo")]
        [InlineData(105, "centocinquesimo")]
        [InlineData(109, "centonovesimo")]
        [InlineData(110, "centodecimo")]
        [InlineData(119, "centodiciannovesimo")]
        [InlineData(120, "centoventesimo")]
        [InlineData(121, "centoventunesimo")]
        [InlineData(200, "duecentesimo")]
        [InlineData(201, "duecentounesimo")]
        [InlineData(240, "duecentoquarantesimo")]
        [InlineData(300, "trecentesimo")]
        [InlineData(900, "novecentesimo")]
        [InlineData(1000, "millesimo")]
        [InlineData(1001, "milleunesimo")]
        [InlineData(1002, "milleduesimo")]
        [InlineData(1003, "milletreesimo")]
        [InlineData(1009, "millenovesimo")]
        [InlineData(1010, "milledecimo")]
        [InlineData(1021, "milleventunesimo")]
        [InlineData(2000, "duemillesimo")]
        [InlineData(2001, "duemilaunesimo")]
        [InlineData(3000, "tremillesimo")]
        [InlineData(10000, "diecimillesimo")]
        [InlineData(10001, "diecimilaunesimo")]
        [InlineData(10121, "diecimilacentoventunesimo")]
        [InlineData(100000, "centomillesimo")]
        [InlineData(100001, "centomilaunesimo")]
        [InlineData(1000000, "milionesimo")]
        [InlineData(1000001, "un milione unesimo")]
        [InlineData(1000002, "un milione duesimo")]
        [InlineData(2000000, "duemilionesimo")]
        [InlineData(10000000, "diecimilionesimo")]
        [InlineData(100000000, "centomilionesimo")]
        [InlineData(1000000000, "miliardesimo")]
        [InlineData(2000000000, "duemiliardesimo")]
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "prima")]
        [InlineData(2, "seconda")]
        [InlineData(5, "quinta")]
        [InlineData(9, "nona")]
        [InlineData(10, "decima")]
        [InlineData(11, "undicesima")]
        [InlineData(18, "diciottesima")]
        [InlineData(20, "ventesima")]
        [InlineData(21, "ventunesima")]
        [InlineData(100, "centesima")]
        [InlineData(101, "centounesima")]
        [InlineData(200, "duecentesima")]
        [InlineData(1000, "millesima")]
        [InlineData(1001, "milleunesima")]
        [InlineData(10000, "diecimillesima")]
        public void ToFeminineOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "primo")]
        [InlineData(2, "secondo")]
        [InlineData(5, "quinto")]
        [InlineData(9, "nono")]
        [InlineData(10, "decimo")]
        [InlineData(11, "undicesimo")]
        [InlineData(18, "diciottesimo")]
        [InlineData(20, "ventesimo")]
        [InlineData(21, "ventunesimo")]
        [InlineData(100, "centesimo")]
        [InlineData(101, "centounesimo")]
        [InlineData(200, "duecentesimo")]
        [InlineData(1000, "millesimo")]
        [InlineData(1001, "milleunesimo")]
        [InlineData(10000, "diecimillesimo")]
        public void ToMasculineOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Masculine));
        }
    }
}
