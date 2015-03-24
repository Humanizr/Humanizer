using Xunit;
using Xunit.Extensions;
using System;

namespace Humanizer.Tests
{
    public class FrequencyHumanizeTest
    {

        [Theory]
        [InlineData()]
        public void EveryTwelveHours(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every(new TimeSpan(12, 0, 0)).Humanize() == "roughly every twelve hours");
        }

        [Theory]
        [InlineData()]
        public void EveryThreeDaysAndAHalf(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every(new TimeSpan(3, 8, 0, 0, 0)).Humanize() == "roughly every 3 days and a half");
        }

        [Theory]
        [InlineData()]
        public void EveryDay(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every().Day().Humanize() == "every day");
        }

        [Theory]
        [InlineData()]
        public void EveryMonth(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every().Month().Humanize() == "every month");
        }

        [Theory]
        [InlineData()]
        public void EveryYear(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every().Year().Humanize() == "every year");
        }

        [Theory]
        [InlineData()]
        public void Every(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every().Year().StartingOn(new DateTime(2014, 2, 3)).EndingOn(new DateTime(2018, 8, 3)).Humanize() == 
                "every 3rd of february from 3rd of february 2014 till 3rd of august 2018");
        }

        [Theory]
        [InlineData()]
        public void EveryTimespan(TimeSpan timespan, string expectedSentence)
        {
            Assert.True(Frequency.Every(new DateTime(2015, 12, 05)).Humanize() == "every year on 5th of december");
        }
        
    }
}
