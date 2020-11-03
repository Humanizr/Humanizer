using Xunit;

namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de-DE")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "null")]
        [InlineData(1, "ein")]
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
        [InlineData(3501, "dreitausendfünfhundertein")]
        [InlineData(111, "einhundertelf")]
        [InlineData(1112, "eintausendeinhundertzwölf")]
        [InlineData(11213, "elftausendzweihundertdreizehn")]
        [InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehn")]
        [InlineData(2132415, "zwei Millionen einhundertzweiunddreißigtausendvierhundertfünfzehn")]
        [InlineData(12345516, "zwölf Millionen dreihundertfünfundvierzigtausendfünfhundertsechzehn")]
        [InlineData(751633617, "siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertsiebzehn")]
        [InlineData(1111111118, "eine Milliarde einhundertelf Millionen einhundertelftausendeinhundertachtzehn")]
        [InlineData(35484694489515, "fünfunddreißig Billionen vierhundertvierundachtzig Milliarden sechshundertvierundneunzig Millionen vierhundertneunundachtzigtausendfünfhundertfünfzehn")]
        [InlineData(8183162164626926, "acht Billiarden einhundertdreiundachtzig Billionen einhundertzweiundsechzig Milliarden einhundertvierundsechzig Millionen sechshundertsechsundzwanzigtausendneunhundertsechsundzwanzig")]
        [InlineData(4564121926659524672, "vier Trillionen fünfhundertvierundsechzig Billiarden einhunderteinundzwanzig Billionen neunhundertsechsundzwanzig Milliarden sechshundertneunundfünfzig Millionen fünfhundertvierundzwanzigtausendsechshundertzweiundsiebzig")]
        [InlineData(-751633619, "minus siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertneunzehn")]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1, "eine")]
        [InlineData(3501, "dreitausendfünfhunderteine")]
        public void ToWordsFeminine(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(0, "nullter")]
        [InlineData(1, "erster")]
        [InlineData(2, "zweiter")]
        [InlineData(3, "dritter")]
        [InlineData(4, "vierter")]
        [InlineData(5, "fünfter")]
        [InlineData(6, "sechster")]
        [InlineData(7, "siebter")]
        [InlineData(8, "achter")]
        [InlineData(9, "neunter")]
        [InlineData(10, "zehnter")]
        [InlineData(20, "zwanzigster")]
        [InlineData(30, "dreißigster")]
        [InlineData(40, "vierzigster")]
        [InlineData(50, "fünfzigster")]
        [InlineData(60, "sechzigster")]
        [InlineData(70, "siebzigster")]
        [InlineData(80, "achtzigster")]
        [InlineData(90, "neunzigster")]
        [InlineData(100, "einhundertster")]
        [InlineData(200, "zweihundertster")]
        [InlineData(1000, "eintausendster")]
        [InlineData(10000, "zehntausendster")]
        [InlineData(100000, "einhunderttausendster")]
        [InlineData(1000000, "einmillionster")]
        [InlineData(10000000, "zehnmillionster")]
        [InlineData(100000000, "einhundertmillionster")]
        [InlineData(1000000000, "einmilliardster")]
        [InlineData(2000000000, "zweimilliardster")]
        [InlineData(122, "einhundertzweiundzwanzigster")]
        [InlineData(3501, "dreitausendfünfhunderterster")]
        [InlineData(111, "einhundertelfter")]
        [InlineData(1112, "eintausendeinhundertzwölfter")]
        [InlineData(11213, "elftausendzweihundertdreizehnter")]
        [InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehnter")]
        [InlineData(2132415, "zweimillioneneinhundertzweiunddreißigtausendvierhundertfünfzehnter")]
        [InlineData(12345516, "zwölfmillionendreihundertfünfundvierzigtausendfünfhundertsechzehnter")]
        [InlineData(751633617, "siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertsiebzehnter")]
        [InlineData(1111111118, "einemilliardeeinhundertelfmillioneneinhundertelftausendeinhundertachtzehnter")]
        [InlineData(-751633619, "minus siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertneunzehnter")]
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }

        [Theory]
        [InlineData(0, "nullte")]
        [InlineData(1, "erste")]
        [InlineData(2, "zweite")]
        [InlineData(3, "dritte")]
        [InlineData(4, "vierte")]
        [InlineData(5, "fünfte")]
        [InlineData(6, "sechste")]
        [InlineData(7, "siebte")]
        [InlineData(8, "achte")]
        [InlineData(9, "neunte")]
        [InlineData(10, "zehnte")]
        [InlineData(111, "einhundertelfte")]
        [InlineData(1112, "eintausendeinhundertzwölfte")]
        [InlineData(11213, "elftausendzweihundertdreizehnte")]
        [InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehnte")]
        [InlineData(2132415, "zweimillioneneinhundertzweiunddreißigtausendvierhundertfünfzehnte")]
        [InlineData(12345516, "zwölfmillionendreihundertfünfundvierzigtausendfünfhundertsechzehnte")]
        [InlineData(751633617, "siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertsiebzehnte")]
        [InlineData(1111111118, "einemilliardeeinhundertelfmillioneneinhundertelftausendeinhundertachtzehnte")]
        [InlineData(-751633619, "minus siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertneunzehnte")]
        public void ToOrdinalWordsFeminine(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(0, "nulltes")]
        [InlineData(1, "erstes")]
        [InlineData(2, "zweites")]
        [InlineData(3, "drittes")]
        [InlineData(4, "viertes")]
        [InlineData(5, "fünftes")]
        [InlineData(6, "sechstes")]
        [InlineData(7, "siebtes")]
        [InlineData(8, "achtes")]
        [InlineData(9, "neuntes")]
        [InlineData(10, "zehntes")]
        [InlineData(111, "einhundertelftes")]
        [InlineData(1112, "eintausendeinhundertzwölftes")]
        [InlineData(11213, "elftausendzweihundertdreizehntes")]
        [InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehntes")]
        [InlineData(2132415, "zweimillioneneinhundertzweiunddreißigtausendvierhundertfünfzehntes")]
        [InlineData(12345516, "zwölfmillionendreihundertfünfundvierzigtausendfünfhundertsechzehntes")]
        [InlineData(751633617, "siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertsiebzehntes")]
        [InlineData(1111111118, "einemilliardeeinhundertelfmillioneneinhundertelftausendeinhundertachtzehntes")]
        [InlineData(-751633619, "minus siebenhunderteinundfünfzigmillionensechshundertdreiunddreißigtausendsechshundertneunzehntes")]
        public void ToOrdinalWordsNeuter(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Neuter));
        }
    }
}
