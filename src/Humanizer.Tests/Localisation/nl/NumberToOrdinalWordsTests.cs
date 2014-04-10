using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nl
{
    public class NumberToOrdinalWordsTests : AmbientCulture
    {
		public NumberToOrdinalWordsTests() : base("nl-NL") { }

		[Theory]
        [InlineData(0, "nulde")]
        [InlineData(1, "eerste")]
        [InlineData(2, "tweede")]
        [InlineData(3, "derde")]
        [InlineData(4, "vierde")]
        [InlineData(5, "vijfde")]
        [InlineData(6, "zesde")]
        [InlineData(7, "zevende")]
        [InlineData(8, "achtste")]
        [InlineData(9, "negende")]
        [InlineData(10, "tiende")]
        [InlineData(11, "elfde")]
        [InlineData(12, "twaalfde")]
        [InlineData(13, "dertiende")]
        [InlineData(14, "veertiende")]
        [InlineData(15, "vijftiende")]
        [InlineData(16, "zestiende")]
        [InlineData(17, "zeventiende")]
		[InlineData(18, "achttiende")]
        [InlineData(19, "negentiende")]
        [InlineData(20, "twintigste")]
        [InlineData(21, "eenentwintigste")]
		[InlineData(22, "tweeëntwintigste")]
        [InlineData(30, "dertigste")]
        [InlineData(40, "veertigste")]
        [InlineData(50, "vijftigste")]
        [InlineData(60, "zestigste")]
        [InlineData(70, "zeventigste")]
        [InlineData(80, "tachtigste")]
        [InlineData(90, "negentigste")]
        [InlineData(95, "vijfennegentigste")]
        [InlineData(96, "zesennegentigste")]
        [InlineData(100, "honderdste")]
		[InlineData(101, "honderdeerste")]
		[InlineData(106, "honderdzesde")]
		[InlineData(108, "honderdachtste")]
		[InlineData(112, "honderdtwaalfde")]
        [InlineData(120, "honderdtwintigste")]
        [InlineData(121, "honderdeenentwintigste")]
        [InlineData(1000, "duizendste")]
        [InlineData(1001, "duizend eerste")]
		[InlineData(1005, "duizend vijfde")]
		[InlineData(1008, "duizend achtste")]
		[InlineData(1012, "duizend twaalfde")]
        [InlineData(1021, "duizend eenentwintigste")]
        [InlineData(10000, "tienduizendste")]
        [InlineData(10121, "tienduizend honderdeenentwintigste")]
        [InlineData(100000, "honderdduizendste")]
		[InlineData(100001, "honderdduizend eerste")]
        [InlineData(1000000, "een miljoenste")]
		[InlineData(1000001, "een miljoen eerste")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
