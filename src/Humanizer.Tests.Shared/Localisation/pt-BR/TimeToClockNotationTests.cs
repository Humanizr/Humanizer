#if NET6_0_OR_GREATER

using System;
using Xunit;
using Humanizer.Localisation.TimeToClockNotation;
using Humanizer;

namespace Humanizer.Tests.Localisation.ptBR
{
    [UseCulture("pt-BR")]
    public class TimeToClockNotationTests
    {
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

        [Theory]
        [InlineData(00, 00, "meia-noite")]
        [InlineData(04, 00, "quatro em ponto")]
        [InlineData(05, 01, "cinco em ponto")]
        [InlineData(06, 05, "seis e cinco")]
        [InlineData(07, 10, "sete e dez")]
        [InlineData(08, 15, "oito e quinze")]
        [InlineData(09, 20, "nove e vinte")]
        [InlineData(10, 25, "dez e vinte e cinco")]
        [InlineData(11, 30, "onze e meia")]
        [InlineData(12, 00, "meio-dia")]
        [InlineData(13, 23, "uma e vinte e cinco")]
        [InlineData(14, 32, "duas e meia")]
        [InlineData(15, 35, "três e trinta e cinco")]
        [InlineData(16, 40, "vinte para as cinco")]
        [InlineData(17, 45, "quinze para as seis")]
        [InlineData(18, 50, "dez para as sete")]
        [InlineData(19, 55, "cinco para as oito")]
        [InlineData(20, 59, "nove em ponto")]
        public void ConvertToRoundedClockNotationTimeOnlyStringPtBr(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}

#endif
