using System;
using Xunit;

namespace Humanizer.Tests.Localisation.roRO
{


    /// <summary>
    /// Test that for values bigger than 19 "de" is added between the numeral
    /// and the time unit: http://ebooks.unibuc.ro/filologie/NForascu-DGLR/numerale.htm.
    /// There is no test for months since there are only 12 of them in a year.
    /// </summary>
    [UseCulture("ro-RO")]
    public class TimeSpanHumanizerTests
    {

        [Theory]
        [InlineData(1, "1 milisecundă")]
        [InlineData(14, "14 milisecunde")]
        [InlineData(21, "21 de milisecunde")]
        [InlineData(3000, "3 secunde")]
        public void Milliseconds(int milliseconds, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "0 secunde", true)]
        [InlineData(0, "0 de secunde")]
        [InlineData(1, "1 secundă")]
        [InlineData(14, "14 secunde")]
        [InlineData(21, "21 de secunde")]
        [InlineData(156, "2 minute")]
        public void Seconds(int seconds, string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize(minUnit: Humanizer.Localisation.TimeUnit.Second,
                toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 minut")]
        [InlineData(14, "14 minute")]
        [InlineData(21, "21 de minute")]
        [InlineData(156, "2 ore")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 oră")]
        [InlineData(14, "14 ore")]
        [InlineData(21, "21 de ore")]
        [InlineData(48, "2 zile")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 zi")]
        [InlineData(6, "6 zile")]
        [InlineData(7, "1 săptămână")]
        [InlineData(14, "2 săptămâni")]
        [InlineData(21, "3 săptămâni")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "1 săptămână")]
        [InlineData(14, "14 săptămâni")]
        [InlineData(21, "21 de săptămâni")]
        public void Weeks(int weeks, string expected)
        {
            var actual = TimeSpan.FromDays(7 * weeks).Humanize();
            Assert.Equal(expected, actual);
        }


        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 lună")]
        [InlineData(61, "2 luni")]
        [InlineData(92, "3 luni")]
        [InlineData(335, "11 luni")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 an")]
        [InlineData(731, "2 ani")]
        [InlineData(1096, "3 ani")]
        [InlineData(4018, "11 ani")]
        [InlineData(7500, "20 de ani")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 de milisecunde", TimeSpan.Zero.Humanize());
        }

        [Fact, CustomDescription("The name of this test is confusing because has no sense. Instead should be read as an interval with duration zero and not the absence of time.")]
        public void NoTimeToWords()
        {
            // Usage in Romanian: "Timp execuție: 0 secunde."
            // Should be equivalent with TimeSpan.FromSeconds(0).Humanize()
            Assert.Equal("0 secunde", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
