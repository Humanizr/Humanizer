using Humanize.Extensions;
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
    }
}
