using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.srLatn
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("sr-Latn") { }

        [Theory]
        [InlineData(35, "5 nedelja")]
        [InlineData(14, "2 nedelje")]
        [InlineData(7, "1 nedelja")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 dana")]
        [InlineData(2, "2 dana")]
        [InlineData(1, "1 dan")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5, "5 sati")]
        [InlineData(2, "2 sata")]
        [InlineData(1, "1 sat")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minuta")]
        [InlineData(1, "1 minut")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(135, "2 minuta")]
        [InlineData(60, "1 minut")]
        [InlineData(5, "5 sekundi")]
        [InlineData(3, "3 sekunde")]
        [InlineData(2, "2 sekunde")]
        [InlineData(1, "1 sekunda")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 sekunde")]
        [InlineData(1400, "1 sekunda")]
        [InlineData(2, "2 milisekunde")]
        [InlineData(1, "1 milisekunda")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "bez proteklog vremena")]
        [InlineData(0, 2, "bez proteklog vremena")]
        [InlineData(10, 2, "10 milisekundi")]
        [InlineData(1400, 2, "1 sekunda, 400 milisekundi")]
        [InlineData(2500, 2, "2 sekunde, 500 milisekundi")]
        [InlineData(120000, 2, "2 minuta")]
        [InlineData(62000, 2, "1 minut, 2 sekunde")]
        [InlineData(62020, 2, "1 minut, 2 sekunde")]
        [InlineData(62020, 3, "1 minut, 2 sekunde, 20 milisekundi")]
        [InlineData(3600020, 4, "1 sat, 20 milisekundi")]
        [InlineData(3600020, 3, "1 sat, 20 milisekundi")]
        [InlineData(3600020, 2, "1 sat, 20 milisekundi")]
        [InlineData(3600020, 1, "1 sat")]
        [InlineData(3603001, 2, "1 sat, 3 sekunde")]
        [InlineData(3603001, 3, "1 sat, 3 sekunde, 1 milisekunda")]
        [InlineData(86400000, 3, "1 dan")]
        [InlineData(86400000, 2, "1 dan")]
        [InlineData(86400000, 1, "1 dan")]
        [InlineData(86401000, 1, "1 dan")]
        [InlineData(86401000, 2, "1 dan, 1 sekunda")]
        [InlineData(86401200, 2, "1 dan, 1 sekunda")]
        [InlineData(86401200, 3, "1 dan, 1 sekunda, 200 milisekundi")]
        [InlineData(1296000000, 1, "2 nedelje")]
        [InlineData(1296000000, 2, "2 nedelje, 1 dan")]
        [InlineData(1299600000, 2, "2 nedelje, 1 dan")]
        [InlineData(1299600000, 3, "2 nedelje, 1 dan, 1 sat")]
        [InlineData(1299630020, 3, "2 nedelje, 1 dan, 1 sat")]
        [InlineData(1299630020, 4, "2 nedelje, 1 dan, 1 sat, 30 sekundi")]
        [InlineData(1299630020, 5, "2 nedelje, 1 dan, 1 sat, 30 sekundi, 20 milisekundi")]
        public void TimeSpanWithPrecesion(int milliseconds, int precesion, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precesion);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("bez proteklog vremena", actual);
        }
    }
}