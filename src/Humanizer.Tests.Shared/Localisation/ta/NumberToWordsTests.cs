using Xunit;

namespace Humanizer.Tests.Localisation.ta
{
    [UseCulture("ta")]
    public class NumberToWordsTests
    {
        //http://tnschools.gov.in/media/textbooks/3rd_Maths_Science_and_Social_science_TM_Combine_pd_Full_Book.pdf page 12
        [Theory]
        [InlineData(0, "சுழியம்")]
        [InlineData(1, "ஒன்று")]
        [InlineData(2, "இரண்டு")]
        [InlineData(3, "மூன்று")]
        [InlineData(4, "நான்கு")]
        [InlineData(5, "ஐந்து")]
        [InlineData(6, "ஆறு")]
        [InlineData(7, "ஏழு")]
        [InlineData(8, "எட்டு")]
        [InlineData(9, "ஒன்பது")]
        [InlineData(10, "பத்து")]

        [InlineData(11, "பதினொன்று")]
        [InlineData(12, "பனிரெண்டு")]
        [InlineData(13, "பதிமூன்று")]
        [InlineData(14, "பதினான்கு")]
        [InlineData(15, "பதினைந்து")]
        [InlineData(16, "பதினாறு")]
        [InlineData(17, "பதினேழு")]
        [InlineData(18, "பதினெட்டு")]
        [InlineData(19, "பத்தொன்பது")]
        [InlineData(20, "இருபது")]
        [InlineData(21, "இருபத்து ஒன்று")]

        [InlineData(30, "முப்பது")]
        [InlineData(31, "முப்பத்து ஒன்று")]

        [InlineData(40, "நாற்பது")]

        [InlineData(41, "நாற்பத்தி ஒன்று")]


        [InlineData(50, "ஐம்பது")]
        [InlineData(60, "அறுபது")]
        [InlineData(64, "அறுபத்து நான்கு")]

        [InlineData(70, "எழுபது")]
        [InlineData(80, "எண்பது")]
        [InlineData(89, "எண்பத்தி ஒன்பது")]
        [InlineData(90, "தொண்ணூறு")]
        [InlineData(95, "தொண்ணூற்றி ஐந்து")]

        [InlineData(100, "நூறு")]
        [InlineData(101, "நூற்று ஒன்று")]
        [InlineData(121, "நூற்று இருபத்து ஒன்று")]
        [InlineData(191, "நூற்று தொண்ணூற்றி ஒன்று")]

        [InlineData(200, "இருநூறு")]
        [InlineData(201, "இருநூற்று ஒன்று")]
        [InlineData(411, "நானூற்று பதினொன்று")]

        [InlineData(535, "ஐந்நூற்று முப்பத்து ஐந்து")]
        [InlineData(985, "தொள்ளாயிரத்து எண்பத்தி ஐந்து")]

        [InlineData(1000, "ஆயிரம்")]
        [InlineData(1535, "ஆயிரத்து ஐந்நூற்று முப்பத்து ஐந்து")]

        [InlineData(2000, "இரண்டாயிரம்")]
        [InlineData(3000, "மூன்றாயிரம்")]
        [InlineData(4000, "நான்காயிரம்")]
        [InlineData(5000, "ஐந்தாயிரம்")]
        [InlineData(6000, "ஆறாயிரம்")]
        [InlineData(7000, "ஏழாயிரம்")]
        [InlineData(8000, "எட்டாயிரம்")]
        [InlineData(8888, "எட்டாயிரத்து எண்ணூற்று எண்பத்தி எட்டு")]  
        [InlineData(9000, "ஒன்பதாயிரம்")]
        [InlineData(9999, "ஒன்பதாயிரத்து தொள்ளாயிரத்து தொண்ணூற்றி ஒன்பது")]

        [InlineData(10000, "பத்தாயிரம்")]
        [InlineData(20000, "இருபதாயிரம்")]
        [InlineData(20005, "இருபதாயிரத்து ஐந்து")]
        [InlineData(20205, "இருபதாயிரத்து இருநூற்று ஐந்து")]
        [InlineData(25435, "இருபத்து ஐந்தாயிரத்து நானூற்று முப்பத்து ஐந்து")]
        [InlineData(90995, "தொண்ணூறாயிரத்து தொள்ளாயிரத்து தொண்ணூற்றி ஐந்து")]

        [InlineData(100000, "ஒரு இலட்சம்")]
        [InlineData(1000000, "பத்து இலட்சம்")]

        [InlineData(10000000, "ஒரு கோடி")]
        [InlineData(100000000, "பத்து கோடி")]
        [InlineData(1000000000, "நூறு கோடி")]
        [InlineData(2000000000, "இருநூறு கோடி")]
        [InlineData(122, "நூற்று இருபத்து இரண்டு")]
        [InlineData(3501, "மூன்றாயிரத்து ஐந்நூற்று ஒன்று")]
        [InlineData(111, "நூற்று பதினொன்று")]
        [InlineData(1112, "ஆயிரத்து நூற்று பனிரெண்டு")]
        [InlineData(11213, "பதினொன்றாயிரத்து இருநூற்று பதிமூன்று")]
        [InlineData(121314, "ஒரு இலட்சத்து இருபத்து ஓராயிரத்து முன்னூற்று பதினான்கு")]
        [InlineData(2132415, "இருபத்து ஒன்று இலட்சத்து முப்பத்து இரண்டாயிரத்து நானூற்று பதினைந்து")]
        [InlineData(12345516, "ஒரு கோடியே இருபத்து மூன்று இலட்சத்து நாற்பத்தி ஐந்தாயிரத்து ஐந்நூற்று பதினாறு")]
        [InlineData(751633617, "எழுபத்தி ஐந்து கோடியே பதினாறு இலட்சத்து முப்பத்து மூன்றாயிரத்து அறுநூற்று பதினேழு")]
        [InlineData(1111111118, "நூற்று பதினொன்று கோடியே பதினொன்று இலட்சத்து பதினொன்றாயிரத்து நூற்று பதினெட்டு")]
        [InlineData(35484694489515, "முப்பத்து ஐந்து இலட்சத்து நாற்பத்தி எட்டாயிரத்து நானூற்று அறுபத்து ஒன்பது கோடியே நாற்பத்தி நான்கு இலட்சத்து எண்பத்தி ஒன்பதாயிரத்து ஐந்நூற்று பதினைந்து")]
        //[InlineData(8183162164626926, "எட்டு quadrillion கோடியே நாற்பத்தி ஆறு இலட்சத்து இருபத்து ஆறாயிரத்து தொள்ளாயிரத்து இருபத்து ஆறு")]
        //[InlineData(4564121926659524672, "நான்கு quintillion ஐந்நூற்று அறுபத்து நான்கு quadrillion கோடியே தொண்ணூற்றி ஐந்து இலட்சத்து இருபத்து நான்காயிரத்து அறுநூற்று எழுபத்தி இரண்டு")]
        [InlineData(-751633619, "கழித்தல் எழுபத்தி ஐந்து கோடியே பதினாறு இலட்சத்து முப்பத்து மூன்றாயிரத்து அறுநூற்று பத்தொன்பது")]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1, "ஒன்று")]
        [InlineData(3501, "மூன்றாயிரத்து ஐந்நூற்று ஒன்று")]
        public void ToWordsFeminine(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }
    

    }
}
