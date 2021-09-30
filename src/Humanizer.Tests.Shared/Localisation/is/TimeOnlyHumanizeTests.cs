#if NET6_0_OR_GREATER

using System;

using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;

using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    [UseCulture("is")]
    public class TimeOnlyHumanizeTests
    {
        [Theory]
        [InlineData("13:07:05", "13:07:05", "núna")]
        [InlineData("13:08:05", "1:08:05", "eftir 12 klukkustundir")]
        [InlineData("13:08:05", "13:38:05", "fyrir 30 mínútum")]
        [InlineData("13:07:02", "17:07:05", "fyrir 4 klukkustundum")]
        public void DefaultStrategy(string inputTime, string timeToCompareAgainst, string expectedResult)
        {
            Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

            var parsedInputTime = TimeOnly.Parse(inputTime);
            var parsedBaseTime = TimeOnly.Parse(timeToCompareAgainst);
            var actualResult = parsedInputTime.Humanize(parsedBaseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("18:10:49", "13:07:04", "eftir 5 klukkustundir", 0.75)]
        [InlineData("13:10:49", "13:07:04", "eftir 4 mínútur", 0.5)]
        [InlineData("18:10:49", "13:07:04", "eftir 5 klukkustundir", 1.0)]
        public void PrecisionStrategy(string inputTime, string timeToCompareAgainst, string expectedResult, double precision)
        {
            Configurator.TimeOnlyHumanizeStrategy = new PrecisionTimeOnlyHumanizeStrategy(precision);

            var parsedInputTime = TimeOnly.Parse(inputTime);
            var parsedBaseTime = TimeOnly.Parse(timeToCompareAgainst);
            var actualResult = parsedInputTime.Humanize(parsedBaseTime);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void TestNever()
        {
            Assert.Equal("aldrei", ((TimeOnly?)null).Humanize());
        }
    }
}

#endif
