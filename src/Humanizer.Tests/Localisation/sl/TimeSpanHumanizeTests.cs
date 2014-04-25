using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sl
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("sl-SI") {}

        [Theory]
        [InlineData(7, "1 teden")]
        [InlineData(14, "2 tedna")]
        [InlineData(21, "3 tednov")]
        [InlineData(28, "4 tednov")]
        [InlineData(35, "5 tednov")]
        [InlineData(77, "11 tednov")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dan")]
        [InlineData(2, "2 dni")]
        [InlineData(3, "3 dni")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 ura")]
        [InlineData(2, "2 uri")]
        [InlineData(3, "3 ur")]
        [InlineData(4, "4 ur")]
        [InlineData(5, "5 ur")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minuta")]
        [InlineData(2, "2 minuti")]
        [InlineData(3, "3 minut")]
        [InlineData(4, "4 minut")]
        [InlineData(5, "5 minut")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "1 sekunda")]
        [InlineData(2, "2 sekundi")]
        [InlineData(3, "3 sekund")]
        [InlineData(4, "4 sekund")]
        [InlineData(5, "5 sekund")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 milisekunda")]
        [InlineData(2, "2 milisekundi")]
        [InlineData(3, "3 milisekund")]
        [InlineData(4, "4 milisekund")]
        [InlineData(5, "5 milisekund")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("ni časa", TimeSpan.Zero.Humanize());
        }
    }
}