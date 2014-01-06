using System;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ar
{
    public class TimeSpanHumanizeExtensionsTests : AmbientCulture
    {
        public TimeSpanHumanizeExtensionsTests() : base("ar") {         }

        [Theory]
        [InlineData(7, "أسبوع واحد")]
        [InlineData(14, "أسبوعين")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }


        [Theory]
        [InlineData(1, "يوم واحد")]
        [InlineData(2, "يومين")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "ساعة واحدة")]
        [InlineData(2, "ساعتين")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "دقيقة واحدة")]
        [InlineData(2, "دقيقتين")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "ثانية واحدة")]
        [InlineData(2, "ثانيتين")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "جزء من الثانية")]
        [InlineData(2, "جزئين من الثانية")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("حالاً", TimeSpan.Zero.Humanize());
        }
    }
}