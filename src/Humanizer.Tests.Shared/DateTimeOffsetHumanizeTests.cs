using System;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class DateTimeOffsetHumanizeTests
    {

        [Fact]
        public void DefaultStrategy_SameOffset()
        {
            Configurator.DateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();

            var inputTime = new DateTimeOffset(2015, 07, 05, 04, 0, 0, TimeSpan.Zero);
            var baseTime = new DateTimeOffset(2015, 07, 05, 03, 0, 0, TimeSpan.Zero);

            const string expectedResult = "an hour from now";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_DifferentOffsets()
        {
            Configurator.DateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();

            var inputTime = new DateTimeOffset(2015, 07, 05, 03, 0, 0, new TimeSpan(2, 0, 0));
            var baseTime = new DateTimeOffset(2015, 07, 05, 02, 30, 0, new TimeSpan(1, 0, 0));

            const string expectedResult = "30 minutes ago";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void PrecisionStrategy_SameOffset()
        {
            Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(0.75);

            var inputTime = new DateTimeOffset(2015, 07, 05, 04, 0, 0, TimeSpan.Zero);
            var baseTime = new DateTimeOffset(2015, 07, 04, 05, 0, 0, TimeSpan.Zero);

            const string expectedResult = "tomorrow";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void PrecisionStrategy_DifferentOffsets()
        {
            Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(0.75);

            var inputTime = new DateTimeOffset(2015, 07, 05, 03, 45, 0, new TimeSpan(2, 0, 0));
            var baseTime = new DateTimeOffset(2015, 07, 05, 02, 30, 0, new TimeSpan(-5, 0, 0));

            const string expectedResult = "6 hours ago";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Never()
        {
            DateTimeOffset? never = null;
            Assert.Equal("never", never.Humanize());
        }

        [Fact]
        public void Nullable_ExpectSame()
        {
            DateTimeOffset? never = new DateTimeOffset(2015, 12, 7, 9, 0, 0, TimeSpan.FromHours(1));

            Assert.Equal(never.Value.Humanize(), never.Humanize());
        }
    }
}
