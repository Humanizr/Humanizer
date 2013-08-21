using System;
using Xunit;
namespace Humanizer.Tests.Extensions.FluentDate
{
    public class InTests
    {
        [Fact]
        public void InJanuary()
        {
            var inJan = In.January;
            Assert.Equal(DateTime.Now.Year, inJan.Year);
            Assert.Equal(1, inJan.Month);
            Assert.Equal(1, inJan.Day);
            Assert.Equal(0, inJan.Hour);
            Assert.Equal(0, inJan.Minute);
            Assert.Equal(0, inJan.Second);
            Assert.Equal(0, inJan.Millisecond);
        }

        [Fact]
        public void InJanuaryOf2009()
        {
            var inJan = In.JanuaryOf(2009);
            Assert.Equal(2009, inJan.Year);
            Assert.Equal(1, inJan.Month);
            Assert.Equal(1, inJan.Day);
            Assert.Equal(0, inJan.Hour);
            Assert.Equal(0, inJan.Minute);
            Assert.Equal(0, inJan.Second);
            Assert.Equal(0, inJan.Millisecond);
        }

        [Fact]
        public void InFebruary()
        {
            var inFeb = In.February;
            Assert.Equal(DateTime.Now.Year, inFeb.Year);
            Assert.Equal(2, inFeb.Month);
            Assert.Equal(1, inFeb.Day);
            Assert.Equal(0, inFeb.Hour);
            Assert.Equal(0, inFeb.Minute);
            Assert.Equal(0, inFeb.Second);
            Assert.Equal(0, inFeb.Millisecond);
        }

        [Fact]
        public void InTheYear()
        {
            var date = In.TheYear(2009);
            Assert.Equal(2009, date.Year);
            Assert.Equal(1, date.Month);
            Assert.Equal(1, date.Day);
            Assert.Equal(0, date.Hour);
            Assert.Equal(0, date.Minute);
            Assert.Equal(0, date.Second);
            Assert.Equal(0, date.Millisecond);

        }
    }
}
