#if NET6_0_OR_GREATER

using System;
using Xunit;

namespace Humanizer.Tests.Localisation.fr
{
    [UseCulture("fr")]
    public class TimeOnlyHumanizeTests
    {
        [Fact]
        public void DefaultStrategy_SameTime()
        {
            var inputTime = new TimeOnly(13, 07, 05);
            var baseTime = new TimeOnly(13, 07, 05);

            const string expectedResult = "maintenant";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_HoursApart()
        {
            var inputTime = new TimeOnly(13, 08, 05);
            var baseTime = new TimeOnly(1, 08, 05);

            const string expectedResult = "dans 12 heures";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void DefaultStrategy_HoursAgo()
        {
            var inputTime = new TimeOnly(13, 07, 02);
            var baseTime = new TimeOnly(17, 07, 05);

            const string expectedResult = "il y a 4 heures";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void PrecisionStrategy_NextDay()
        {
            var inputTime = new TimeOnly(18, 10, 49);
            var baseTime = new TimeOnly(13, 07, 04);

            const string expectedResult = "dans 5 heures";
            var actualResult = inputTime.Humanize(baseTime);

            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void Never()
        {
            TimeOnly? never = null;
            Assert.Equal("jamais", never.Humanize());
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
