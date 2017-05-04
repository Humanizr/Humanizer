using System;
using Xunit;
namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de-DE")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "Ein Jahr")]
        [InlineData(731, "2 Jahre")]
        [InlineData(1096, "3 Jahre")]
        [InlineData(4018, "11 Jahre")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "Ein Monat")]
        [InlineData(61, "2 Monate")]
        [InlineData(92, "3 Monate")]
        [InlineData(335, "11 Monate")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "Eine Woche")]
        [InlineData(14, "2 Wochen")]
        [InlineData(21, "3 Wochen")]
        [InlineData(77, "11 Wochen")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }


        [Theory]
        [InlineData(1, "Ein Tag")]
        [InlineData(2, "2 Tage")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "Eine Stunde")]
        [InlineData(2, "2 Stunden")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "Eine Minute")]
        [InlineData(2, "2 Minuten")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "Eine Sekunde")]
        [InlineData(2, "2 Sekunden")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "Eine Millisekunde")]
        [InlineData(2, "2 Millisekunden")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("Keine Zeit", TimeSpan.Zero.Humanize());
        }
    }
}
