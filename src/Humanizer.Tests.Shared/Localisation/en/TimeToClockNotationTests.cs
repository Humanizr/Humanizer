#if NET6_0_OR_GREATER

using System;
using Xunit;

namespace Humanizer.Tests.Localisation.en
{
    public class TimeToClockNotationTests
    {
        [UseCulture("en-US")]
        [Theory]
        [InlineData(00, 00, "midnight")]
        [InlineData(04, 00, "four o'clock")]
        [InlineData(05, 01, "five one")]
        [InlineData(06, 05, "five past six")]
        [InlineData(07, 10, "ten past seven")]
        [InlineData(08, 15, "a quarter past eight")]
        [InlineData(09, 20, "twenty past nine")]
        [InlineData(10, 25, "twenty-five past ten")]
        [InlineData(11, 30, "half past eleven")]
        [InlineData(12, 00, "noon")]
        [InlineData(15, 35, "three thirty-five")]
        [InlineData(16, 40, "twenty to five")]
        [InlineData(17, 45, "a quarter to six")]
        [InlineData(18, 50, "ten to seven")]
        [InlineData(19, 55, "five to eight")]
        [InlineData(20, 59, "eight fifty-nine")]
        public void ConvertToClockNotationTimeOnlyStringEnUs(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
            Assert.Equal(expectedResult, actualResult);
        }

        [UseCulture("pt-BR")]
        [Theory]
        [InlineData(00, 00, "meia-noite")]
        [InlineData(04, 00, "quatro em ponto")]
        [InlineData(05, 01, "cinco e um")]
        [InlineData(06, 05, "seis e cinco")]
        [InlineData(07, 10, "sete e dez")]
        [InlineData(08, 15, "oito e quinze")]
        [InlineData(09, 20, "nove e vinte")]
        [InlineData(10, 25, "dez e vinte e cinco")]
        [InlineData(11, 30, "onze e meia")]
        [InlineData(12, 00, "meio-dia")]
        [InlineData(15, 35, "três e trinta e cinco")]
        [InlineData(16, 40, "vinte para as cinco")]
        [InlineData(17, 45, "quinze para as seis")]
        [InlineData(18, 50, "dez para as sete")]
        [InlineData(19, 55, "cinco para as oito")]
        [InlineData(20, 59, "oito e cinquenta e nove")]
        public void ConvertToClockNotationTimeOnlyStringPtBr(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}

#endif
