using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nl
{
	public class NumberToWordsTests : AmbientCulture
    {
		public NumberToWordsTests() : base("nl-NL") { }

		[InlineData(0, "nul")]
        [InlineData(1, "een")]
		[InlineData(-10, "min tien")]
        [InlineData(10, "tien")]
        [InlineData(11, "elf")]
		[InlineData(122, "honderdtweeëntwintig")]
        [InlineData(3501, "drieduizend vijfhonderdeen")]
        [InlineData(100, "honderd")]
        [InlineData(1000, "duizend")]
        [InlineData(100000, "honderdduizend")]
        [InlineData(1000000, "een miljoen")]
        [InlineData(10000000, "tien miljoen")]
        [InlineData(100000000, "honderd miljoen")]
        [InlineData(1000000000, "een miljard")]
        [InlineData(111, "honderdelf")]
        [InlineData(1111, "duizend honderdelf")]
        [InlineData(111111, "honderdelfduizend honderdelf")]
		[InlineData(1111111, "een miljoen honderdelfduizend honderdelf")]
		[InlineData(11111111, "elf miljoen honderdelfduizend honderdelf")]
		[InlineData(111111111, "honderdelf miljoen honderdelfduizend honderdelf")]
		[InlineData(1111111111, "een miljard honderdelf miljoen honderdelfduizend honderdelf")]
		[InlineData(123, "honderddrieëntwintig")]
		[InlineData(124, "honderdvierentwintig")]
        [InlineData(1234, "duizend tweehonderdvierendertig")]
		[InlineData(12345, "twaalfduizend driehonderdvijfenveertig")]
		[InlineData(123456, "honderddrieëntwintigduizend vierhonderdzesenvijftig")]
        [InlineData(1234567, "een miljoen tweehonderdvierendertigduizend vijfhonderdzevenenzestig")]
		[InlineData(12345678, "twaalf miljoen driehonderdvijfenveertigduizend zeshonderdachtenzeventig")]
		[InlineData(123456789, "honderddrieëntwintig miljoen vierhonderdzesenvijftigduizend zevenhonderdnegenentachtig")]
		[InlineData(1234567890, "een miljard tweehonderdvierendertig miljoen vijfhonderdzevenenzestigduizend achthonderdnegentig")]
		[InlineData(1234567899, "een miljard tweehonderdvierendertig miljoen vijfhonderdzevenenzestigduizend achthonderdnegenennegentig")]

		[InlineData(108, "honderdacht")]
		[InlineData(678, "zeshonderdachtenzeventig")]
		[InlineData(2013, "tweeduizend dertien")]
		[InlineData(2577, "tweeduizend vijfhonderdzevenenzeventig")]
		[InlineData(17053980, "zeventien miljoen drieënvijftigduizend negenhonderdtachtig")]

		[InlineData(415618, "vierhonderdvijftienduizend zeshonderdachttien")]
		[InlineData(16415618, "zestien miljoen vierhonderdvijftienduizend zeshonderdachttien")]
		[InlineData(322, "driehonderdtweeëntwintig")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
