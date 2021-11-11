#if NET6_0_OR_GREATER

using System;
using Xunit;
using Humanizer.Localisation.TimeToClockNotation;
using Humanizer;

namespace Humanizer.Tests.Localisation.en
{
    [UseCulture("en")]
    public class TimeToClockNotationTests
    {
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

        [Theory]
        [InlineData(00, 00, "midnight")]
        [InlineData(04, 00, "four o'clock")]
        [InlineData(05, 01, "five o'clock")]
        [InlineData(06, 05, "five past six")]
        [InlineData(07, 10, "ten past seven")]
        [InlineData(08, 15, "a quarter past eight")]
        [InlineData(09, 20, "twenty past nine")]
        [InlineData(10, 25, "twenty-five past ten")]
        [InlineData(11, 30, "half past eleven")]
        [InlineData(12, 00, "noon")]
        [InlineData(13, 23, "twenty-five past one")]
        [InlineData(14, 32, "half past two")]
        [InlineData(15, 35, "three thirty-five")]
        [InlineData(16, 40, "twenty to five")]
        [InlineData(17, 45, "a quarter to six")]
        [InlineData(18, 50, "ten to seven")]
        [InlineData(19, 55, "five to eight")]
        [InlineData(20, 59, "nine o'clock")]
        public void ConvertToRoundedClockNotationTimeOnlyStringEnUs(int hours, int minutes, string expectedResult)
        {
            var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}

#endif
