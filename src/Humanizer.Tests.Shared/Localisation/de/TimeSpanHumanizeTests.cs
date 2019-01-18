using System;
using Xunit;
namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de-DE")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 Jahr")]
        [InlineData(731, "2 Jahre")]
        [InlineData(1096, "3 Jahre")]
        [InlineData(4018, "11 Jahre")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "ein Jahr")]
        [InlineData(731, "zwei Jahre")]
        [InlineData(1096, "drei Jahre")]
        [InlineData(4018, "elf Jahre")]
        public void YearsToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year,toWords:true));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 Monat")]
        [InlineData(61, "2 Monate")]
        [InlineData(92, "3 Monate")]
        [InlineData(335, "11 Monate")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "ein Monat")]
        [InlineData(61, "zwei Monate")]
        [InlineData(92, "drei Monate")]
        [InlineData(335, "elf Monate")]
        public void MonthsToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year,toWords:true));
        }

        [Theory]
        [InlineData(7, "1 Woche")]
        [InlineData(14, "2 Wochen")]
        [InlineData(21, "3 Wochen")]
        [InlineData(77, "11 Wochen")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(7, "eine Woche")]
        [InlineData(14, "zwei Wochen")]
        [InlineData(21, "drei Wochen")]
        [InlineData(77, "elf Wochen")]
        public void WeeksToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));
        }


        [Theory]
        [InlineData(1, "1 Tag")]
        [InlineData(2, "2 Tage")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "ein Tag")]
        [InlineData(2, "zwei Tage")]
        public void DaysToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "1 Stunde")]
        [InlineData(2, "2 Stunden")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "eine Stunde")]
        [InlineData(2, "zwei Stunden")]
        public void HoursToWords(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "1 Minute")]
        [InlineData(2, "2 Minuten")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "eine Minute")]
        [InlineData(2, "zwei Minuten")]
        public void MinutesToWords(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: true));
        }


        [Theory]
        [InlineData(1, "1 Sekunde")]
        [InlineData(2, "2 Sekunden")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "eine Sekunde")]
        [InlineData(2, "zwei Sekunden")]
        public void SecondsToWords(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "1 Millisekunde")]
        [InlineData(2, "2 Millisekunden")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Theory]
        [InlineData(1, "eine Millisekunde")]
        [InlineData(2, "zwei Millisekunden")]
        public void MillisecondsToWords(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: true));
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 Millisekunden", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("Keine Zeit", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
