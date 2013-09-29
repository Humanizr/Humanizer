using Xunit;

namespace Humanizer.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void PascalCaseInputStringIsTurnedIntoSentence()
        {
            Assert.Equal(
                "Pascal case input string is turned into sentence",
                "PascalCaseInputStringIsTurnedIntoSentence".Humanize());
        }

        [Fact]
        public void WhenInputStringContainsConsequtiveCaptialLetters_ThenTheyAreTurnedIntoOneLetterWords()
        {
            Assert.Equal(
                "When I use an input a here",
                "WhenIUseAnInputAHere".Humanize());
        }

        [Fact]
        public void WhenInputStringStartsWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.Equal("10 is in the begining", "10IsInTheBegining".Humanize());
        }

        [Fact]
        public void WhenInputStringEndWithANumber_ThenNumberIsDealtWithLikeAWord()
        {
            Assert.Equal("Number is at the end 100", "NumberIsAtTheEnd100".Humanize());
        }

        [Fact]
        public void UnderscoredInputStringIsTurnedIntoSentence()
        {
            Assert.Equal(
                "Underscored input string is turned into sentence",
                "Underscored_input_string_is_turned_into_sentence".Humanize());
        }

        [Fact]
        public void UnderscoredInputStringPreservesCasing()
        {
            Assert.Equal(
                "Underscored input String is turned INTO sentence",
                "Underscored_input_String_is_turned_INTO_sentence".Humanize());
        }

        [Fact]
        public void OneLetterWordInTheBeginningOfStringIsTurnedIntoAWord()
        {
            Assert.Equal(
                "X is first word in the sentence",
                "XIsFirstWordInTheSentence".Humanize());
        }

        [Fact]
        public void AcronymsAreLeftIntact()
        {
            Assert.Equal(
                "HTML",
                "HTML".Humanize());
        }

        [Fact]
        public void AcronymsAreSeparatedFromOtherWordsInTheMiddle()
        {
            Assert.Equal(
                "The HTML language",
                "TheHTMLLanguage".Humanize());
        }

        [Fact]
        public void AcronymsAreSeparatedFromOtherWordsInTheStart()
        {
            Assert.Equal(
                "HTML is the language",
                "HTMLIsTheLanguage".Humanize());
        }

        [Fact]
        public void AcronymsAreSeparatedFromOtherWordsInTheEnd()
        {
            Assert.Equal(
                "The language is HTML",
                "TheLanguageIsHTML".Humanize());
        }

        [Fact]
        public void AcronymsAreSeparatedFromNumbersInTheEnd()
        {
            Assert.Equal(
                "HTML 5",
                "HTML5".Humanize());
        }

        [Fact]
        public void AcronymsAreSeparatedFromNumbersInTheStart()
        {
            Assert.Equal(
                "1 HTML",
                "1HTML".Humanize());
        }

        [Fact]
        public void CanHumanizeIntoTitleCaseWithoutUsingUnderscores()
        {
            Assert.Equal(
                "Can Return Title Case",
                "CanReturnTitleCase".Humanize(LetterCasing.Title));
        }

        [Fact]
        public void CanHumanizeIntoTitleCaseWhenUsingUnderscores()
        {
            Assert.Equal(
                "Can Return Title Case",
                "Can_return_title_Case".Humanize(LetterCasing.Title));
        }

        [Fact]
        public void TitleHumanizationHonorsAllCaps()
        {
            Assert.Equal(
                "Title Humanization Honors ALLCAPS",
                "Title_humanization_Honors_ALLCAPS".Humanize(LetterCasing.Title));
        }

        [Fact]
        public void CanHumanizeIntoLowerCase()
        {
            Assert.Equal(
                "can return lower case",
                "CanReturnLowerCase".Humanize(LetterCasing.LowerCase));
        }

        [Fact]
        public void CanHumanizeIntoSentenceCase()
        {
            Assert.Equal(
                "Can return sentence case",
                "CanReturnSentenceCase".Humanize(LetterCasing.Sentence));
        }

        [Fact]
        public void SentenceCasingWorksOnEmptyStrings()
        {
            Assert.Equal(
                "",
                "".Humanize(LetterCasing.Sentence));
        }

        [Fact]
        public void CanHumanizeIntoLowerCaseEvenWhenUsingAllCaps()
        {
            Assert.Equal(
                "lowercase",
                "LOWERCASE".Humanize(LetterCasing.LowerCase));
        }

        [Fact]
        public void CanHumanizeIntoUpperCase()
        {
            Assert.Equal(
                "CAN HUMANIZE INTO UPPER CASE",
                "CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps));
        }

        [Fact]
        public void CanTurnIntoUpperCasewhenUsingUnderscores()
        {
            Assert.Equal(
                "CAN HUMANIZE INTO UPPER CASE",
                "Can_Humanize_into_Upper_case".Humanize(LetterCasing.AllCaps));
        }
    }
}
