using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.it
{
    public class ItalianNumberToOrdinalWordsTests : AmbientCulture
    {
        public ItalianNumberToOrdinalWordsTests() : base("it") { }

        [Theory]
        [InlineData(0, "zeresimo")]
        [InlineData(1, "primo")]
        [InlineData(2, "secondo")]
        [InlineData(3, "terzo")]
        [InlineData(4, "quarto")]
        [InlineData(5, "quinto")]
        [InlineData(6, "sesto")]
        [InlineData(7, "settimo")]
        [InlineData(8, "ottavo")]
        [InlineData(9, "nono")]
        [InlineData(10, "decimo")]
        [InlineData(11, "undicesimo")]
        [InlineData(12, "dodicesimo")]
        [InlineData(13, "tredicesimo")]
        [InlineData(14, "quattordicesimo")]
        [InlineData(15, "quindicesimo")]
        [InlineData(16, "sedicesimo")]
        [InlineData(17, "diciassettesimo")]
        [InlineData(18, "diciottesimo")]
        [InlineData(19, "diciannovesimo")]
        [InlineData(20, "ventesimo")]
        [InlineData(21, "ventunesimo")]
        [InlineData(22, "ventiduesimo")]
        [InlineData(30, "trentesimo")]
        [InlineData(40, "quarantesimo")]
        [InlineData(50, "cinquantesimo")]
        [InlineData(60, "sessantesimo")]
        [InlineData(70, "settantesimo")]
        [InlineData(80, "ottantesimo")]
        [InlineData(90, "novantesimo")]
        [InlineData(95, "novantacinquesimo")]
        [InlineData(96, "novantaseiesimo")]
        [InlineData(100, "centesimo")]
        [InlineData(120, "centoventesimo")]
        [InlineData(121, "centoventunesimo")]
        [InlineData(1000, "millesimo")]
        [InlineData(1001, "milleunesimo")]
        [InlineData(1021, "milleventunesimo")]
        [InlineData(10000, "diecimillesimo")]
        [InlineData(10121, "diecimilacentoventunesimo")]
        [InlineData(100000, "centomillesimo")]
        [InlineData(1000000, "milionesimo")]
        [InlineData(123456789, "centoventitremilioniquattrocentocinquantaseimilasettecentoottantanovesimo")]
        public void ItalianToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
