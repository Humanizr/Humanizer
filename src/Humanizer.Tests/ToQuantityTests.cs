﻿using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class ToQuantityTests
    {

        [Theory]
        [InlineData("case", 0, "0 cases")]
        [InlineData("case", 1, "1 case")]
        [InlineData("case", 5, "5 cases")]
        [InlineData("man", 0, "0 men")]
        [InlineData("man", 1, "1 man")]
        [InlineData("man", 2, "2 men")]
        public void ToQuantity(string word, int quatity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quatity));
        }
    }
}
