using System;
using Xunit;

namespace Humanizer.Tests.Localisation.sl
{
    [UseCulture("sl-SI")]
    public class TimeSpanHumanizeTests 
    {

        [Theory]
        [InlineData(7, "1 teden")]
        [InlineData(14, "2 tedna")]
        [InlineData(21, "3 tedne")]
        [InlineData(28, "4 tedne")]
        [InlineData(35, "5 tednov")]
        [InlineData(77, "11 tednov")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dan")]
        [InlineData(2, "2 dneva")]
        [InlineData(3, "3 dni")]
        [InlineData(4, "4 dni")]
        [InlineData(5, "5 dni")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 ura")]
        [InlineData(2, "2 uri")]
        [InlineData(3, "3 ure")]
        [InlineData(4, "4 ure")]
        [InlineData(5, "5 ur")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minuta")]
        [InlineData(2, "2 minuti")]
        [InlineData(3, "3 minute")]
        [InlineData(4, "4 minute")]
        [InlineData(5, "5 minut")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "1 sekunda")]
        [InlineData(2, "2 sekundi")]
        [InlineData(3, "3 sekunde")]
        [InlineData(4, "4 sekunde")]
        [InlineData(5, "5 sekund")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 milisekunda")]
        [InlineData(2, "2 milisekundi")]
        [InlineData(3, "3 milisekunde")]
        [InlineData(4, "4 milisekunde")]
        [InlineData(5, "5 milisekund")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("nič časa", TimeSpan.Zero.Humanize());
        }
    }
}
