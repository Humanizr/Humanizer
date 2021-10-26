#if NET6_0_OR_GREATER

using System;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class TimeOnlyHumanizeTests
    {

        [Fact]
        public void DefaultStrategy_SameTime()
        {
            Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

            var inputTime = new TimeOnly(13, 07, 05);
            var baseTime = new TimeOnly(13, 07, 05);

            const string expectedResult = "now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_HoursApart()
        {
            Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

            var inputTime = new TimeOnly(13, 08, 05);
            var baseTime = new TimeOnly(1, 08, 05);

            const string expectedResult = "12 hours from now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_HoursAgo()
        {
            Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

            var inputTime = new TimeOnly(13, 07, 02);
            var baseTime = new TimeOnly(17, 07, 05);

            const string expectedResult = "4 hours ago";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void PrecisionStrategy_NextDay()
        {
            Configurator.TimeOnlyHumanizeStrategy = new PrecisionTimeOnlyHumanizeStrategy(0.75);

            var inputTime = new TimeOnly(18, 10, 49);
            var baseTime = new TimeOnly(13, 07, 04);

            const string expectedResult = "5 hours from now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void Never()
        {
            TimeOnly? never = null;
            Assert.Equal("never", never.Humanize());
        }

        [Fact]
        public void Nullable_ExpectSame()
        {
            TimeOnly? never = new TimeOnly(23, 12, 7);

            Assert.Equal(never.Value.Humanize(), never.Humanize());
        }
    }
}

#endif
