using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class StringHumanizeTests
    {
        [Theory]
        [InlineData("PascalCaseInputStringIsTurnedIntoSentence", "Pascal case input string is turned into sentence")]
        [InlineData("WhenIUseAnInputAHere", "When I use an input a here")]
        [InlineData("10IsInTheBegining", "10 is in the begining")]
        [InlineData("NumberIsAtTheEnd100", "Number is at the end 100")]
        [InlineData("XIsFirstWordInTheSentence", "X is first word in the sentence")]
        public void CanHumanizeStringInPascalCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize());
        }

        [Theory]
        [InlineData("Underscored_input_string_is_turned_into_sentence", "Underscored input string is turned into sentence")]
        [InlineData("Underscored_input_String_is_turned_INTO_sentence", "Underscored input String is turned INTO sentence")]
        public void CanHumanizeStringWithUnderscores(string input, string expectedReseult)
        {
            Assert.Equal(expectedReseult, input.Humanize());
        }

        [Theory]
        [InlineData("HTML", "HTML")]
        [InlineData("TheHTMLLanguage", "The HTML language")]
        [InlineData("HTMLIsTheLanguage", "HTML is the language")]
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
        [InlineData("Title_humanization_Honors_ALLCAPS", "Title Humanization Honors ALLCAPS")]
        public void CanHumanizeIntoTileCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.Title));
        }

        [Theory]
        [InlineData("CanReturnLowerCase", "can return lower case")]
        [InlineData("LOWERCASE", "lowercase")]
        public void CanHumanizeIntoLowerCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.LowerCase));
        }

        [Theory]
        [InlineData("CanReturnSentenceCase", "Can return sentence case")]
        [InlineData("", "")]
        public void CanHumanizeIntoSentenceCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.Sentence));
        }
        
        [Theory]
        [InlineData("CanHumanizeIntoUpperCase", "CAN HUMANIZE INTO UPPER CASE")]
        [InlineData("Can_Humanize_into_Upper_case", "CAN HUMANIZE INTO UPPER CASE")]
        public void CanHumanizeIntoUpperCase(string input, string expectedResult)
        {
            Assert.Equal(expectedResult, input.Humanize(LetterCasing.AllCaps));
        }
    }
}