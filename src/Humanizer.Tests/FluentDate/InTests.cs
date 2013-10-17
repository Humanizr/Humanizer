using System;
using Xunit;

namespace Humanizer.Tests.FluentDate
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

        [Fact]
        public void InFiveDays()
        {
            var baseDate = On.January.The21st;
            var date = In.Five.DaysFrom(baseDate);
            Assert.Equal(baseDate.Year, date.Year);
            Assert.Equal(baseDate.Month, date.Month);
            Assert.Equal(baseDate.Day + 5, date.Day);
            Assert.Equal(baseDate.Hour, date.Hour);
            Assert.Equal(baseDate.Minute, date.Minute);
            Assert.Equal(baseDate.Second, date.Second);
            Assert.Equal(baseDate.Millisecond, date.Millisecond);
        }
    }
}
