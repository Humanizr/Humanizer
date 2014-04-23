using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.de
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("de-DE") { }

        [Theory]
        [InlineData(0, "null")]
        [InlineData(1, "eins")]
        [InlineData(2, "zwei")]
        [InlineData(3, "drei")]
        [InlineData(4, "vier")]
        [InlineData(5, "fünf")]
        [InlineData(6, "sechs")]
        [InlineData(7, "sieben")]
        [InlineData(8, "acht")]
        [InlineData(9, "neun")]
        [InlineData(10, "zehn")]
        [InlineData(20, "zwanzig")]
        [InlineData(30, "dreißig")]
        [InlineData(40, "vierzig")]
        [InlineData(50, "fünfzig")]
        [InlineData(60, "sechzig")]
        [InlineData(70, "siebzig")]
        [InlineData(80, "achtzig")]
        [InlineData(90, "neunzig")]
        [InlineData(100, "einhundert")]
        [InlineData(200, "zweihundert")]
        [InlineData(1000, "eintausend")]
        [InlineData(10000, "zehntausend")]
        [InlineData(100000, "einhunderttausend")]
        [InlineData(1000000, "eine Million")]
        [InlineData(10000000, "zehn Millionen")]
        [InlineData(100000000, "einhundert Millionen")]
        [InlineData(1000000000, "eine Milliarde")]
        [InlineData(2000000000, "zwei Milliarden")]
        [InlineData(122, "einhundertzweiundzwanzig")]
        [InlineData(3501, "dreitausendfünfhunderteins")]
        [InlineData(111, "einhundertelf")]
        [InlineData(1112, "eintausendeinhundertzwölf")]
        [InlineData(11213, "elftausendzweihundertdreizehn")]
        [InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehn")]
        [InlineData(2132415, "zwei Millionen einhundertzweiunddreißigtausendvierhundertfünfzehn")]
        [InlineData(12345516, "zwölf Millionen dreihundertfünfundvierzigtausendfünfhundertsechzehn")]
        [InlineData(751633617, "siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertsiebzehn")]
        [InlineData(1111111118, "eine Milliarde einhundertelf Millionen einhundertelftausendeinhundertachtzehn")]
        [InlineData(-751633619, "minus siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertneunzehn")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
