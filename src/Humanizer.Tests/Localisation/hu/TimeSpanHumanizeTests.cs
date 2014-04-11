using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.hu
{
    public class TimeSpanHumanizeTests : AmbientCulture
    {
        public TimeSpanHumanizeTests() : base("hu-HU") { }

        [Theory]
        [InlineData(14, "2 hét")]
        [InlineData(7, "1 hét")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 nap")]
        [InlineData(2, "2 nap")]
        [InlineData(1, "1 nap")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 óra")]
        [InlineData(1, "1 óra")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 perc")]
        [InlineData(1, "1 perc")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(135, "2 perc")]
        [InlineData(60, "1 perc")]
        [InlineData(2, "2 másodperc")]
        [InlineData(1, "1 másodperc")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 másodperc")]
        [InlineData(1400, "1 másodperc")]
        [InlineData(2, "2 ezredmásodperc")]
        [InlineData(1, "1 ezredmásodperc")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "nincs idő")]
        [InlineData(0, 2, "nincs idő")]
        [InlineData(10, 2, "10 ezredmásodperc")]
        [InlineData(1400, 2, "1 másodperc, 400 ezredmásodperc")]
        [InlineData(2500, 2, "2 másodperc, 500 ezredmásodperc")]
        [InlineData(120000, 2, "2 perc")]
        [InlineData(62000, 2, "1 perc, 2 másodperc")]
        [InlineData(62020, 2, "1 perc, 2 másodperc")]
        [InlineData(62020, 3, "1 perc, 2 másodperc, 20 ezredmásodperc")]
        [InlineData(3600020, 4, "1 óra, 20 ezredmásodperc")]
        [InlineData(3600020, 3, "1 óra, 20 ezredmásodperc")]
        [InlineData(3600020, 2, "1 óra, 20 ezredmásodperc")]
        [InlineData(3600020, 1, "1 óra")]
        [InlineData(3603001, 2, "1 óra, 3 másodperc")]
        [InlineData(3603001, 3, "1 óra, 3 másodperc, 1 ezredmásodperc")]
        [InlineData(86400000, 3, "1 nap")]
        [InlineData(86400000, 2, "1 nap")]
        [InlineData(86400000, 1, "1 nap")]
        [InlineData(86401000, 1, "1 nap")]
        [InlineData(86401000, 2, "1 nap, 1 másodperc")]
        [InlineData(86401200, 2, "1 nap, 1 másodperc")]
        [InlineData(86401200, 3, "1 nap, 1 másodperc, 200 ezredmásodperc")]
        [InlineData(1296000000, 1, "2 hét")]
        [InlineData(1296000000, 2, "2 hét, 1 nap")]
        [InlineData(1299600000, 2, "2 hét, 1 nap")]
        [InlineData(1299600000, 3, "2 hét, 1 nap, 1 óra")]
        [InlineData(1299630020, 3, "2 hét, 1 nap, 1 óra")]
        [InlineData(1299630020, 4, "2 hét, 1 nap, 1 óra, 30 másodperc")]
        [InlineData(1299630020, 5, "2 hét, 1 nap, 1 óra, 30 másodperc, 20 ezredmásodperc")]
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
            Assert.Equal("nincs idő", actual);
        }
    }
}
