using Xunit;

namespace Humanizer.Tests
{
    public class StringHumanizeTests
    {
        [Theory]
        [InlineData("PascalCaseInputStringIsTurnedIntoSentence", "Pascal case input string is turned into sentence")]
        [InlineData("WhenIUseAnInputAHere", "When I use an input a here")]
        [InlineData("10IsInTheBegining", "10 is in the begining")]
        [InlineData("NumberIsFollowedByLowerCase5th", "Number is followed by lower case 5th")]
        [InlineData("NumberIsAtTheEnd100", "Number is at the end 100")]
        [InlineData("XIsFirstWordInTheSentence", "X is first word in the sentence")]
        [InlineData("XIsFirstWordInTheSentence ThenThereIsASpace", "X is first word in the sentence then there is a space")]
        [InlineData("ContainsSpecial?)@Characters", "Contains special characters")]
        [InlineData("a", "A")]
        [InlineData("A", "A")]
        [InlineData("?)@", "")]
        [InlineData("?", "")]
        [InlineData("", "")]
        [InlineData("JeNeParlePasFrançais", "Je ne parle pas français")]
        public void CanHumanizeStringInPascalCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize());
        }

        [Theory, UseCulture("tr-TR")]
        [InlineData("istanbul", "İstanbul")]
        [InlineData("diyarbakır", "Diyarbakır")]
        public void CanHumanizeStringInPascalCaseInTurkish(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize());
        }

        [Theory, UseCulture("ar")]
        [InlineData("جمهورية ألمانيا الاتحادية", "جمهورية ألمانيا الاتحادية")]
        public void CanHumanizeOtherUnicodeLetter(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize());
        }

        [Theory]
        [InlineData("Underscored_input_string_is_turned_into_sentence", "Underscored input string is turned into sentence")]
        [InlineData("Underscored_input_String_is_turned_INTO_sentence", "Underscored input String is turned INTO sentence")]
        [InlineData("TEST 1 - THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1 -THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1- THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1_ THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1 _THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1 _ THIS IS A TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1 - THIS_IS_A_TEST", "TEST 1 THIS IS A TEST")]
        [InlineData("TEST 1 - THIS is A Test", "TEST 1 THIS is A test")]
        public void CanHumanizeStringWithUnderscoresAndDashes(string input, string expectedReseult)
        {
            Assert.Equal(expectedReseult, input.Humanize());
        }

        [Theory]
        [InlineData("HTML", "HTML")]
        [InlineData("TheHTMLLanguage", "The HTML language")]
        [InlineData("HTMLIsTheLanguage", "HTML is the language")]
        [InlineData("TheLanguage IsHTML", "The language is HTML")]
        [InlineData("TheLanguageIsHTML", "The language is HTML")]
        [InlineData("HTML5", "HTML 5")]
        [InlineData("1HTML", "1 HTML")]
        public void CanHumanizeStringWithAcronyms(string input, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Humanize());
        }

        [Theory]
        [InlineData("CanReturnTitleCase", "Can Return Title Case")]
        [InlineData("Can_return_title_Case", "Can Return Title Case")]
        [InlineData("In titles use lower case for prepositions an article or conjunctions", "In Titles Use Lower Case for Prepositions an Article or Conjunctions")]
        [InlineData("Title_humanization_Honors_ALLCAPS", "Title Humanization Honors ALLCAPS")]
        [InlineData("MühldorferStraße23", "Mühldorfer Straße 23")]
        [InlineData("mühldorfer_STRAẞE_23", "Mühldorfer STRAẞE 23")]
        [InlineData("CAN RETURN TITLE CASE", "Can Return Title Case")]
        public void CanHumanizeIntoTitleCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.Title));
        }

        [Theory]
        [InlineData("CanReturnLowerCase", "can return lower case")]
        [InlineData("LOWERCASE", "lowercase")]
        [InlineData("STRAẞE", "straße")]
        public void CanHumanizeIntoLowerCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.LowerCase));
        }

        [Theory]
        [InlineData("CanReturnSentenceCase", "Can return sentence case")]
        [InlineData("", "")]
        [InlineData("égoïste", "Égoïste")]
        public void CanHumanizeIntoSentenceCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.Sentence));
        }

        [Theory]
        [InlineData("CanHumanizeIntoUpperCase", "CAN HUMANIZE INTO UPPER CASE")]
        [InlineData("Can_Humanize_into_Upper_case", "CAN HUMANIZE INTO UPPER CASE")]
        [InlineData("coûts_privés", "COÛTS PRIVÉS")]
        public void CanHumanizeIntoUpperCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.AllCaps));
        }
    }
}
