using System;
using Xunit;

namespace Humanizer.Tests
{
    public class NumberToTimeSpanTests
    {
        [Fact]
        public void Weeks()
        {
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(14), now.Add(2.Weeks()));
        }

        [Fact]
        public void Days()
        {
            Assert.Equal(new TimeSpan(2, 0, 0, 0), 2.Days());
        }

        [Fact]
        public void Hours()
        {
            Assert.Equal(new TimeSpan(0, 3, 0, 0), 3.Hours());
        }

        [Fact]
        public void Minutes()
        {
            Assert.Equal(new TimeSpan(0, 0, 4, 0), 4.Minutes());
        }

        [Fact]
        public void Seconds()
        {
            Assert.Equal(new TimeSpan(0, 0, 0, 5), 5.Seconds());
        }

        [Fact]
        public void Milliseconds()
        {
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 6), 6.Milliseconds());
        }
    }
}
