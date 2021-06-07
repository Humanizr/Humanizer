#if NET6_0_OR_GREATER

using System;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class DateOnlyHumanizeTests
    {

        [Fact]
        public void DefaultStrategy_SameDate()
        {
            Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

            var inputTime = new DateOnly(2015, 07, 05);
            var baseTime = new DateOnly(2015, 07, 05);

            const string expectedResult = "now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_MonthApart()
        {
            Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

            var inputTime = new DateOnly(2015, 08, 05);
            var baseTime = new DateOnly(2015, 07, 05);

            const string expectedResult = "one month from now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_DaysAgo()
        {
            Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

            var inputTime = new DateOnly(2015, 07, 02);
            var baseTime = new DateOnly(2015, 07, 05);

            const string expectedResult = "3 days ago";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void PrecisionStrategy_NextDay()
        {
            Configurator.DateOnlyHumanizeStrategy = new PrecisionDateOnlyHumanizeStrategy(0.75);

            var inputTime = new DateOnly(2015, 07, 05);
            var baseTime = new DateOnly(2015, 07, 04);

            const string expectedResult = "tomorrow";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void Never()
        {
            DateOnly? never = null;
            Assert.Equal("never", never.Humanize());
        }

        [Fact]
        public void Nullable_ExpectSame()
        {
            DateOnly? never = new DateOnly(2015, 12, 7);

            Assert.Equal(never.Value.Humanize(), never.Humanize());
        }
    }
}

#endif
