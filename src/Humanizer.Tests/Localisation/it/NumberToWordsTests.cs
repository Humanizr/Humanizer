﻿using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.it
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests()
            : base("it")
        {
        }

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
        /*
        [InlineData(1, "primo")]
        [InlineData(2, "segundo")]
        [InlineData(3, "terceiro")]
        [InlineData(4, "quarto")]
        [InlineData(5, "quinto")]
        [InlineData(6, "sexto")]
        [InlineData(7, "sétimo")]
        [InlineData(8, "oitavo")]
        [InlineData(9, "nono")]
        [InlineData(10, "décimo")]
        [InlineData(11, "décimo primeiro")]
        [InlineData(12, "décimo segundo")]
        [InlineData(13, "décimo terceiro")]
        [InlineData(14, "décimo quarto")]
        [InlineData(15, "décimo quinto")]
        [InlineData(16, "décimo sexto")]
        [InlineData(17, "décimo sétimo")]
        [InlineData(18, "décimo oitavo")]
        [InlineData(19, "décimo nono")]
        [InlineData(20, "vigésimo")]
        [InlineData(21, "vigésimo primeiro")]
        [InlineData(22, "vigésimo segundo")]
        [InlineData(30, "trigésimo")]
        [InlineData(40, "quadragésimo")]
        [InlineData(50, "quinquagésimo")]
        [InlineData(60, "sexagésimo")]
        [InlineData(70, "septuagésimo")]
        [InlineData(80, "octogésimo")]
        [InlineData(90, "nonagésimo")]
        [InlineData(95, "nonagésimo quinto")]
        [InlineData(96, "nonagésimo sexto")]
        [InlineData(100, "centésimo")]
        [InlineData(120, "centésimo vigésimo")]
        [InlineData(121, "centésimo vigésimo primeiro")]
        [InlineData(200, "ducentésimo")]
        [InlineData(300, "trecentésimo")]
        [InlineData(400, "quadringentésimo")]
        [InlineData(500, "quingentésimo")]
        [InlineData(600, "sexcentésimo")]
        [InlineData(700, "septingentésimo")]
        [InlineData(800, "octingentésimo")]
        [InlineData(900, "noningentésimo")]
        [InlineData(1000, "milésimo")]
        [InlineData(1001, "milésimo primeiro")]
        [InlineData(1021, "milésimo vigésimo primeiro")]
        [InlineData(2021, "segundo milésimo vigésimo primeiro")]
        [InlineData(10000, "décimo milésimo")]
        [InlineData(10121, "décimo milésimo centésimo vigésimo primeiro")]
        [InlineData(100000, "centésimo milésimo")]
        [InlineData(1000000, "milionésimo")]
        [InlineData(1000000000, "bilionésimo")]
        */
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }
        
        /*
        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "primeira")]
        [InlineData(2, "segunda")]
        [InlineData(3, "terceira")]
        [InlineData(4, "quarta")]
        [InlineData(5, "quinta")]
        [InlineData(6, "sexta")]
        [InlineData(7, "sétima")]
        [InlineData(8, "oitava")]
        [InlineData(9, "nona")]
        [InlineData(10, "décima")]
        [InlineData(11, "décima primeira")]
        [InlineData(12, "décima segunda")]
        [InlineData(13, "décima terceira")]
        [InlineData(14, "décima quarta")]
        [InlineData(15, "décima quinta")]
        [InlineData(16, "décima sexta")]
        [InlineData(17, "décima sétima")]
        [InlineData(18, "décima oitava")]
        [InlineData(19, "décima nona")]
        [InlineData(20, "vigésima")]
        [InlineData(21, "vigésima primeira")]
        [InlineData(22, "vigésima segunda")]
        [InlineData(30, "trigésima")]
        [InlineData(40, "quadragésima")]
        [InlineData(50, "quinquagésima")]
        [InlineData(60, "sexagésima")]
        [InlineData(70, "septuagésima")]
        [InlineData(80, "octogésima")]
        [InlineData(90, "nonagésima")]
        [InlineData(95, "nonagésima quinta")]
        [InlineData(96, "nonagésima sexta")]
        [InlineData(100, "centésima")]
        [InlineData(120, "centésima vigésima")]
        [InlineData(121, "centésima vigésima primeira")]
        [InlineData(200, "ducentésima")]
        [InlineData(300, "trecentésima")]
        [InlineData(400, "quadringentésima")]
        [InlineData(500, "quingentésima")]
        [InlineData(600, "sexcentésima")]
        [InlineData(700, "septingentésima")]
        [InlineData(800, "octingentésima")]
        [InlineData(900, "noningentésima")]
        [InlineData(1000, "milésima")]
        [InlineData(1001, "milésima primeira")]
        [InlineData(1021, "milésima vigésima primeira")]
        [InlineData(2021, "segunda milésima vigésima primeira")]
        [InlineData(10000, "décima milésima")]
        [InlineData(10121, "décima milésima centésima vigésima primeira")]
        [InlineData(100000, "centésima milésima")]
        [InlineData(1000000, "milionésima")]
        [InlineData(1000000000, "bilionésima")]
        public void ToFeminineOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords(GrammaticalGender.Feminine));
        }
        */
    }
}