using System;
using Xunit;

namespace Humanizer.Tests.FluentDate
{
    public class PrepositionTests
    {
        [Fact]
        public void AtMidnight()
        {
            var now = DateTime.Now;
            var midnight = now.AtMidnight();
            Assert.Equal(new DateTime(now.Year, now.Month, now.Day), midnight);
        }

        [Fact]
        public void AtNoon()
        {
            var now = DateTime.Now;
            var noon = now.AtNoon();
            Assert.Equal(new DateTime(now.Year, now.Month, now.Day, 12, 0, 0), noon);
        }

        [Fact]
        public void InYear()
        {
            var now = DateTime.Now;
            var in2011 = now.In(2011);
            Assert.Equal(new DateTime(2011, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond), in2011);
        }
    }
}
