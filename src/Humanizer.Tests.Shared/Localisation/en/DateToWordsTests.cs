using System;
using Xunit;

namespace Humanizer.Tests.Localisation.en
{
    public class DateToWordsTests
    {
        [UseCulture("en-GB")]
        [Fact]
        public void ConvertDateToWordsGbString()
        {
            Assert.Equal("the first of January two thousand and twenty-two", new DateTime(2022, 1, 1).ToWords());
        }

        [UseCulture("en-US")]
        [Fact]
        public void ConvertDateToWordsUsString()
        {
            Assert.Equal("January first, two thousand and twenty-two", new DateTime(2022, 1, 1).ToWords());
        }

#if NET6_0_OR_GREATER
        [UseCulture("en-GB")]
        [Fact]
        public void ConvertDateOnlyToWordsGbString()
        {
            Assert.Equal("the first of January two thousand and twenty-two", new DateOnly(2022, 1, 1).ToWords());
        }

        [UseCulture("en-US")]
        [Fact]
        public void ConvertDateOnlyToWordsUsString()
        {
            Assert.Equal("January first, two thousand and twenty-two", new DateOnly(2022, 1, 1).ToWords());
        }
#endif
    }
}
