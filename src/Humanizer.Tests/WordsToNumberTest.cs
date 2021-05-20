using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class WordsToNumberTest
    {
        [InlineData("one", 1)]
        [InlineData("minus five", -5)]
        [InlineData("eleven", 11)]
        [InlineData("ninety five", 95)]
        [InlineData("hundred five", 105)]
        [InlineData("one hundred ninety six", 196)]
        [Theory]
        public void ToNumber(string words, int expectedNumber)
        {
            Assert.Equal(expectedNumber, words.ToNumber());
        }
    }
}
