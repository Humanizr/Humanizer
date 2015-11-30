﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
