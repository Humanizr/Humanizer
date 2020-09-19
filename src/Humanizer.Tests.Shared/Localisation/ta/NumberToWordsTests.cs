using Xunit;

namespace Humanizer.Tests.Localisation.ta
{
    [UseCulture("ta")]
    public class NumberToWordsTests
    {
        //http://tnschools.gov.in/media/textbooks/3rd_Maths_Science_and_Social_science_TM_Combine_pd_Full_Book.pdf page 12
        [Theory]
        //[InlineData(0, "சுழியம்")]
        //[InlineData(1, "ஒன்று")]
        //[InlineData(2, "இரண்டு")]
        //[InlineData(3, "மூன்று")]
        //[InlineData(4, "நான்கு")]
        //[InlineData(5, "ஐந்து")]
        //[InlineData(6, "ஆறு")]
        //[InlineData(7, "ஏழு")]
        //[InlineData(8, "எட்டு")]
        //[InlineData(9, "ஒன்பது")]
        //[InlineData(10, "பத்து")]

        //[InlineData(11, "பதினொன்று")]
        //[InlineData(12, "பனிரெண்டு")]
        //[InlineData(13, "பதிமூன்று")]
        //[InlineData(14, "பதினான்கு")]
        //[InlineData(15, "பதினைந்து")]
        //[InlineData(16, "பதினாறு")]
        //[InlineData(17, "பதினேழு")]
        //[InlineData(18, "பதினெட்டு")]
        //[InlineData(19, "பத்தொன்பது")]
        //[InlineData(20, "இருபது")]
        //[InlineData(21, "இருபத்து ஒன்று")]

        //[InlineData(30, "முப்பது")]
        //[InlineData(31, "முப்பத்து ஒன்று")]

        //[InlineData(40, "நாற்பது")]

        //[InlineData(41, "நாற்பத்தி ஒன்று")]


        //[InlineData(50, "ஐம்பது")]
        //[InlineData(60, "அறுபது")]
        //[InlineData(64, "அறுபத்து நான்கு")]

        //[InlineData(70, "எழுபது")]
        //[InlineData(80, "எண்பது")]
        //[InlineData(89, "எண்பத்தி ஒன்பது")]
        //[InlineData(90, "தொண்ணூறு")]
        //[InlineData(95, "தொண்ணூற்றி ஐந்து")]

        //[InlineData(100, "நூறு")]
        //[InlineData(101, "நூற்று ஒன்று")]
        //[InlineData(121, "நூற்று இருபத்து ஒன்று")]
        //[InlineData(191, "நூற்று தொண்ணூற்றி ஒன்று")]

        //[InlineData(200, "இருநூறு")]
        //[InlineData(201, "இருநூற்று ஒன்று")]
        //[InlineData(411, "நானூற்று பதினொன்று")]

        //[InlineData(535, "ஐந்நூற்று முப்பத்து ஐந்து")]
        [InlineData(985, "தொள்ளாயிரத்து எண்பத்தி ஐந்து")]

        //[InlineData(1000, "ஆயிரம்")]
        //[InlineData(1535, "ஆயிரத்து ஐந்நூற்று முப்பத்து ஐந்து")]

        //[InlineData(2000, "இரண்டாயிரம்")]
        //[InlineData(3000, "மூன்றாயிரம்")]
        //[InlineData(4000, "நான்காயிரம்")]
        //[InlineData(5000, "ஐந்தாயிரம்")]
        //[InlineData(6000, "ஆறாயிரம்")]
        //[InlineData(7000, "ஏழாயிரம்")]
        //[InlineData(8000, "எட்டாயிரம்")]
        //[InlineData(8888, "எட்டாயிரத்து எண்ணூற்று எண்பத்தி எட்டு")]  
        //[InlineData(9000, "ஒன்பதாயிரம்")]
        [InlineData(9999, "ஒன்பதாயிரத்து தொள்ளாயிரத்து தொண்ணூற்றி ஒன்பது")]

        //[InlineData(10000, "பத்தாயிரம்")]
        //[InlineData(20000, "இருபதாயிரம்")]
        //[InlineData(20005, "இருபதாயிரத்து ஐந்து")]
        //[InlineData(20205, "இருபதாயிரத்து இருநூற்று ஐந்து")]
        //[InlineData(25435, "இருபத்து ஐந்தாயிரத்து நானூற்று முப்பத்து ஐந்து")]
        //[InlineData(90995, "தொண்ணூறாயிரத்து தொள்ளாயிரத்து தொண்ணூற்றி ஐந்து")]

        //[InlineData(100000, "ஒரு இலட்சம்")]
        //[InlineData(1000000, "பத்து இலட்சம்")]

        //[InlineData(10000000, "ஒரு கோடி")]
        //[InlineData(100000000, "பத்து கோடி")]
        //[InlineData(1000000000, "நூறு கோடி")]
        //[InlineData(2000000000, "இருநூறு கோடி")]
        //[InlineData(122, "நூற்று இருபத்து இரண்டு")]
        //[InlineData(3501, "மூன்றாயிரத்து ஐநூற்று ஒன்று")]
        //[InlineData(111, "நூற்று பதினொன்று")]
        //[InlineData(1112, "ஆயிரத்து நூற்று பனிரெண்டு")]
        //[InlineData(11213, "பதினொன்றாயிரத்து இருநூற்று பதிமூன்று")]
        //[InlineData(121314, "einhunderteinundzwanzigtausenddreihundertvierzehn")]
        //[InlineData(2132415, "zwei Millionen einhundertzweiunddreißigtausendvierhundertfünfzehn")]
        //[InlineData(12345516, "zwölf Millionen dreihundertfünfundvierzigtausendfünfhundertsechzehn")]
        //[InlineData(751633617, "siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertsiebzehn")]
        //[InlineData(1111111118, "eine Milliarde einhundertelf Millionen einhundertelftausendeinhundertachtzehn")]
        //[InlineData(35484694489515, "fünfunddreißig Billionen vierhundertvierundachtzig Milliarden sechshundertvierundneunzig Millionen vierhundertneunundachtzigtausendfünfhundertfünfzehn")]
        //[InlineData(8183162164626926, "acht Billiarden einhundertdreiundachtzig Billionen einhundertzweiundsechzig Milliarden einhundertvierundsechzig Millionen sechshundertsechsundzwanzigtausendneunhundertsechsundzwanzig")]
        //[InlineData(4564121926659524672, "vier Trillionen fünfhundertvierundsechzig Billiarden einhunderteinundzwanzig Billionen neunhundertsechsundzwanzig Milliarden sechshundertneunundfünfzig Millionen fünfhundertvierundzwanzigtausendsechshundertzweiundsiebzig")]
        //[InlineData(-751633619, "minus siebenhunderteinundfünfzig Millionen sechshundertdreiunddreißigtausendsechshundertneunzehn")]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1, "ஒன்று")]
        [InlineData(3501, "மூன்றாயிரத்து ஐநூற்று ஒன்று")]
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
