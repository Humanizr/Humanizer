using System;
using Xunit;

namespace Humanizer.Tests.FluentDate
{
    public class OnTests
    {
        [Fact]
        public void OnJanuaryThe23rd()
        {
            Assert.Equal(new DateTime(DateTime.Now.Year, 1, 23), On.January.The23rd);
        }

        [Fact]
        public void OnDecemberThe4th()
        {
            Assert.Equal(new DateTime(DateTime.Now.Year, 12, 4), On.December.The4th);
        }

        [Fact]
        public void OnFebruaryThe()
        {
            Assert.Equal(new DateTime(DateTime.Now.Year, 2, 11), On.February.The(11));
        }
    }
}
