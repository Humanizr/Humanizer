#if NET6_0_OR_GREATER

using System;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class TimeOnlyHumanizeClockStrategyTests
    {
        [Theory]
        [InlineData(00, 00, "midnight")]
        [InlineData(04, 00, "four o'clock")]
        [InlineData(05, 01, "one past five")]
        [InlineData(06, 05, "five past six")]
        [InlineData(10, 15, "a quarter past ten")]
        [InlineData(11, 30, "half past eleven")]
        [InlineData(12, 00, "noon")]
        [InlineData(14, 32, "twenty-eight to three")]
        [InlineData(15, 35, "twenty-five to four")]
        [InlineData(16, 40, "twenty to five")]
        [InlineData(17, 45, "a quarter to six")]
        [InlineData(18, 50, "ten to seven")]
        [InlineData(19, 55, "five to eight")]
        [InlineData(20, 59, "one to nine")]
        public void ClockTime(int hours, int minutes, string expectedResult)
        {
            Configurator.TimeOnlyHumanizeStrategy = new ClockTimeTimeOnlyHumanizeStrategy();

            var inputTime = new TimeOnly(hours, minutes);

            var actualResult = inputTime.Humanize();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}

#endif
