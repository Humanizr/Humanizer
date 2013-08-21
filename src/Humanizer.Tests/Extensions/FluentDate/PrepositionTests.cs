using System;
using Xunit;

namespace Humanizer.Tests.Extensions.FluentDate
{
    public class PrepositionTests
    {
        [Fact]
        public void AtMidnight()
        {
            var now = DateTime.Now;
            var midnight = now.AtMidnight();
            Assert.Equal(now.Year, midnight.Year);
            Assert.Equal(now.Month, midnight.Month);
            Assert.Equal(now.Day, midnight.Day);
            Assert.Equal(0, midnight.Hour);
            Assert.Equal(0, midnight.Minute);
            Assert.Equal(0, midnight.Second);
            Assert.Equal(0, midnight.Millisecond);
        }

        [Fact]
        public void AtNoon()
        {
            var now = DateTime.Now;
            var noon = now.AtNoon();
            Assert.Equal(now.Year, noon.Year);
            Assert.Equal(now.Month, noon.Month);
            Assert.Equal(now.Day, noon.Day);
            Assert.Equal(12, noon.Hour);
            Assert.Equal(0, noon.Minute);
            Assert.Equal(0, noon.Second);
            Assert.Equal(0, noon.Millisecond);
        }

        [Fact]
        public void InYear()
        {
            var now = DateTime.Now;
            var in2011 = now.In(2011);
            Assert.Equal(2011, in2011.Year);
            Assert.Equal(now.Month, in2011.Month);
            Assert.Equal(now.Day, in2011.Day);
            Assert.Equal(now.Hour, in2011.Hour);
            Assert.Equal(now.Minute, in2011.Minute);
            Assert.Equal(now.Second, in2011.Second);
            Assert.Equal(now.Millisecond, in2011.Millisecond);
        }
    }
}
