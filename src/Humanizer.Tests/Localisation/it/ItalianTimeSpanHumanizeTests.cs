using System;
using Xunit;

namespace Humanizer.Tests.Localisation.it
{
    public class ItalianTimeSpanHumanizeTests : AmbientCulture
    {
        public ItalianTimeSpanHumanizeTests() : base("it") { }

        [Fact]
        public void ItalianTwoWeeks()
        {
            Assert.Equal("2 settimane", TimeSpan.FromDays(14).Humanize());
        }

        [Fact]
        public void ItalianOneWeek()
        {
            Assert.Equal("una settimana", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void ItalianSixDays()
        {
            Assert.Equal("6 giorni", TimeSpan.FromDays(6).Humanize());
        }

        [Fact]
        public void ItalianTwoDays()
        {
            Assert.Equal("2 giorni", TimeSpan.FromDays(2).Humanize());
        }

        [Fact]
        public void ItalianOneDay()
        {
            Assert.Equal("un giorno", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void ItalianTwoHours()
        {
            Assert.Equal("2 ore", TimeSpan.FromHours(2).Humanize());
        }

        [Fact]
        public void ItalianOneHour()
        {
            Assert.Equal("un'ora", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void ItalianTwoMinutes()
        {
            Assert.Equal("2 minuti", TimeSpan.FromMinutes(2).Humanize());
        }

        [Fact]
        public void ItalianOneMinute()
        {
            Assert.Equal("un minuto", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void ItalianTwoSeconds()
        {
            Assert.Equal("2 secondi", TimeSpan.FromSeconds(2).Humanize());
        }

        [Fact]
        public void ItalianOneSecond()
        {
            Assert.Equal("un secondo", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void ItalianTwoMilliseconds()
        {
            Assert.Equal("2 millisecondi", TimeSpan.FromMilliseconds(2).Humanize());
        }

        [Fact]
        public void ItalianOneMillisecond()
        {
            Assert.Equal("un millisecondo", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Fact]
        public void ItalianNoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            var notime = TimeSpan.Zero;
            Assert.Equal("pochissimo tempo", notime.Humanize());
        }
    }
}