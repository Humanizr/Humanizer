using System;
using Xunit;

namespace Humanizer.Tests.Localisation.en
{
    public class TimeSpanHyphenatedAgeTests
    {
        [UseCulture("en-CA")]
        [Fact]
        public void HyphenatedAgeCa()
        {
            Assert.Equal("5-day-old", TimeSpan.FromDays(5).ToHyphenatedAge());
        }

        [UseCulture("en-GB")]
        [Fact]
        public void HyphenatedAgeGb()
        {
            Assert.Equal("three-week-old", TimeSpan.FromDays(21).ToHyphenatedAge(toWords: true));
        }
    }
}
