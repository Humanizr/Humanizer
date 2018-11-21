using System;
using Xunit;

namespace Humanizer.Tests.Localisation.mt
{

    [UseCulture("mt")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "sena")]
        [InlineData(731, "sentejn")]
        [InlineData(1096, "3 snin")]
        [InlineData(4018, "11 snin")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }


        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "xahar")]
        [InlineData(61, "xahrejn")]
        [InlineData(92, "3 xhur")]
        [InlineData(335, "11 xhur")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "ġimgħa")]
        [InlineData(14, "ġimgħatejn")]
        [InlineData(21, "3 ġimgħat")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "ġurnata")]
        [InlineData(2, "jumejn")]
        [InlineData(3, "3 jiem")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "siegħa")]
        [InlineData(2, "sagħtejn")]
        [InlineData(3, "3 siegħat")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "minuta")]
        [InlineData(2, "2 minuti")]
        [InlineData(3, "3 minuti")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "sekonda")]
        [InlineData(2, "2 sekondi")]
        [InlineData(3, "3 sekondi")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "millisekonda")]
        [InlineData(2, "2 millisekondi")]
        [InlineData(3, "3 millisekondi")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 millisekondi", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("xejn", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
