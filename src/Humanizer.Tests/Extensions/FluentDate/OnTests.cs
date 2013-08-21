using System;
using Xunit;

namespace Humanizer.Tests.Extensions.FluentDate
{
    public class OnTests
    {
        [Fact]
        public void OnJanuaryThe23rd()
        {
            var jan23rd = On.January.The23rd;
            Assert.Equal(DateTime.Now.Year, jan23rd.Year);
            Assert.Equal(1, jan23rd.Month);
            Assert.Equal(23, jan23rd.Day);
            Assert.Equal(0, jan23rd.Hour);
            Assert.Equal(0, jan23rd.Minute);
            Assert.Equal(0, jan23rd.Second);
            Assert.Equal(0, jan23rd.Millisecond);
        }

        [Fact]
        public void OnDecemberThe4th()
        {
            var dec4th = On.December.The4th;
            Assert.Equal(DateTime.Now.Year, dec4th.Year);
            Assert.Equal(12, dec4th.Month);
            Assert.Equal(4, dec4th.Day);
            Assert.Equal(0, dec4th.Hour);
            Assert.Equal(0, dec4th.Minute);
            Assert.Equal(0, dec4th.Second);
            Assert.Equal(0, dec4th.Millisecond);
        }
    }
}
