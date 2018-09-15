using System;
using Xunit;

namespace Humanizer.Tests.Localisation.srLatn
{
    [UseCulture("sr-Latn")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 godina")]
        [InlineData(731, "2 godine")]
        [InlineData(1096, "3 godine")]
        [InlineData(4018, "11 godina")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 mesec")]
        [InlineData(61, "2 meseca")]
        [InlineData(92, "3 meseca")]
        [InlineData(335, "11 meseci")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

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
        [InlineData(2, "2 dana")]
        [InlineData(1, "1 dan")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
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
        [InlineData(2, "2 sekunde")]
        [InlineData(1, "1 sekunda")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 milisekunde")]
        [InlineData(1, "1 milisekunda")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoTime()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize();
            Assert.Equal("0 milisekundi", actual);
        }

        [Fact]
        public void NoTimeToWords()
        {
            var noTime = TimeSpan.Zero;
            var actual = noTime.Humanize(toWords: true);
            Assert.Equal("bez proteklog vremena", actual);
        }
    }
}
