using Xunit;

namespace Humanizer.Tests.Localisation.sv
{
    [UseCulture("sv-SE")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "noll")]
        [InlineData(1, "ett")]
        [InlineData(2, "två")]
        [InlineData(3, "tre")]
        [InlineData(4, "fyra")]
        [InlineData(5, "fem")]
        [InlineData(6, "sex")]
        [InlineData(7, "sju")]
        [InlineData(8, "åtta")]
        [InlineData(9, "nio")]
        [InlineData(10, "tio")]
        [InlineData(20, "tjugo")]
        [InlineData(30, "trettio")]
        [InlineData(40, "fyrtio")]
        [InlineData(50, "femtio")]
        [InlineData(60, "sextio")]
        [InlineData(70, "sjuttio")]
        [InlineData(80, "åttio")]
        [InlineData(90, "nittio")]
        [InlineData(100, "hundra")]
        [InlineData(200, "tvåhundra")]
        [InlineData(201, "tvåhundraett")]
        [InlineData(211, "tvåhundraelva")]
        [InlineData(221, "tvåhundratjugoett")]
        [InlineData(1000, "ett tusen")]
        [InlineData(10000, "tio tusen")]
        [InlineData(100000, "hundra tusen")]
        [InlineData(1000000, "en miljon")]
        [InlineData(10000000, "tio miljoner")]
        [InlineData(100000000, "hundra miljoner")]
        [InlineData(1000000000, "en miljard")]
        [InlineData(2000000000, "två miljarder")]
        [InlineData(122, "hundratjugotvå")]
        [InlineData(3501, "tre tusen femhundraett")]
        [InlineData(111, "hundraelva")]
        [InlineData(1112, "ett tusen hundratolv")]
        [InlineData(11213, "elva tusen tvåhundratretton")]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "nollte")]
        [InlineData(1, "första")]
        [InlineData(2, "andra")]
        [InlineData(3, "tredje")]
        [InlineData(4, "fjärde")]
        [InlineData(5, "femte")]
        [InlineData(6, "sjätte")]
        [InlineData(7, "sjunde")]
        [InlineData(8, "åttonde")]
        [InlineData(9, "nionde")]
        [InlineData(10, "tionde")]
        [InlineData(20, "tjugonde")]
        [InlineData(30, "trettionde")]
        [InlineData(40, "fyrtionde")]
        [InlineData(50, "femtionde")]
        [InlineData(60, "sextionde")]
        [InlineData(70, "sjuttionde")]
        [InlineData(80, "åttionde")]
        [InlineData(90, "nittionde")]
        [InlineData(100, "hundrade")]
        [InlineData(200, "tvåhundrade")]
        [InlineData(201, "tvåhundraförsta")]
        [InlineData(211, "tvåhundraelfte")]
        [InlineData(221, "tvåhundratjugoförsta")]
        [InlineData(1000, "ett tusende")]
        [InlineData(10000, "tio tusende")]
        [InlineData(100000, "hundra tusende")]
        [InlineData(1000000, "en miljonte")]
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }
    }
}
