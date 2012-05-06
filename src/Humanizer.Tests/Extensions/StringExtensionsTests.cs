// Copyright (C) 2012, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.using System;

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
    }
}
