﻿using System.Globalization;
using Xunit;

namespace Humanizer.Tests.Localisation.invariant
{
    [UseCulture("")]
    public class ToQuantityTests 
    {
        [Theory]
        [InlineData("case", 0, "0 cases")]
        [InlineData("case", 1, "1 case")]
        [InlineData("case", 5, "5 cases")]
        [InlineData("man", 0, "0 men")]
        [InlineData("man", 1, "1 man")]
        [InlineData("man", 2, "2 men")]
        [InlineData("men", 2, "2 men")]
        [InlineData("process", 2, "2 processes")]
        [InlineData("process", 1, "1 process")]
        [InlineData("processes", 2, "2 processes")]
        [InlineData("processes", 1, "1 process")]
        public void ToQuantity(string word, int quantity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quantity));
        }
        
        [Theory]
        [InlineData("case", 0, "cases")]
        [InlineData("case", 1, "case")]
        [InlineData("case", 5, "cases")]
        [InlineData("man", 0, "men")]
        [InlineData("man", 1, "man")]
        [InlineData("man", 2, "men")]
        [InlineData("men", 2, "men")]
        [InlineData("process", 2, "processes")]
        [InlineData("process", 1, "process")]
        [InlineData("processes", 2, "processes")]
        [InlineData("processes", 1, "process")]
        public void ToQuantityWithNoQuantity(string word, int quantity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quantity, ShowQuantityAs.None));
        }
        
        [Theory]
        [InlineData("case", 0, "0 cases")]
        [InlineData("case", 1, "1 case")]
        [InlineData("case", 5, "5 cases")]
        [InlineData("man", 0, "0 men")]
        [InlineData("man", 1, "1 man")]
        [InlineData("man", 2, "2 men")]
        [InlineData("men", 2, "2 men")]
        [InlineData("process", 2, "2 processes")]
        [InlineData("process", 1, "1 process")]
        [InlineData("processes", 2, "2 processes")]
        [InlineData("processes", 1, "1 process")]
        public void ToQuantityNumeric(string word, int quantity, string expected)
        {
// ReSharper disable once RedundantArgumentDefaultValue
            Assert.Equal(expected, word.ToQuantity(quantity, ShowQuantityAs.Numeric));
        }
        
        [Theory]
        [InlineData("case", 0, "0 cases")]
        [InlineData("case", 1, "1 case")]
        [InlineData("case", 5, "5 cases")]
        [InlineData("man", 0, "0 men")]
        [InlineData("man", 1, "1 man")]
        [InlineData("man", 2, "2 men")]
        [InlineData("men", 2, "2 men")]
        [InlineData("process", 2, "2 processes")]
        [InlineData("process", 1, "1 process")]
        [InlineData("processes", 2, "2 processes")]
        [InlineData("processes", 1200, "1200 processes")]
        [InlineData("processes", 1, "1 process")]
        public void ToQuantityWords(string word, int quantity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quantity, ShowQuantityAs.Words));
        }
    }
}
