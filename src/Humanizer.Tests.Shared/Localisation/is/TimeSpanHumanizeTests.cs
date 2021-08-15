using System;
using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    [UseCulture("is")]
        public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 ár")]
        [InlineData(731, "2 ár")]
        [InlineData(1096, "3 ár")]
        [InlineData(4018, "11 ár")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "eitt ár")]
        [InlineData(731, "tvö ár")]
        [InlineData(1096, "þrjú ár")]
        [InlineData(4018, "ellefu ár")]
        public void YearsToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year,toWords:true));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 mánuður")]
        [InlineData(61, "2 mánuðir")]
        [InlineData(92, "3 mánuðir")]
        [InlineData(335, "11 mánuðir")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "einn mánuður")]
        [InlineData(61, "tveir mánuðir")]
        [InlineData(92, "þrír mánuðir")]
        [InlineData(335, "ellefu mánuðir")]
        public void MonthsToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year,toWords:true));
        }

        [Theory]
        [InlineData(7, "ein vika")]
        [InlineData(14, "2 vikur")]
        [InlineData(21, "3 vikur")]
        [InlineData(77, "11 vikur")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(7, "ein vika")]
        [InlineData(14, "tvær vikur")]
        [InlineData(21, "þrjár vikur")]
        [InlineData(77, "ellefu vikur")]
        public void WeeksToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));
        }


        [Theory]
        [InlineData(1, "einn dagur")]
        [InlineData(2, "2 dagar")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "einn dagur")]
        [InlineData(2, "tveir dagar")]
        public void DaysToWords(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "ein klukkustund")]
        [InlineData(2, "2 klukkustundir")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "ein klukkustund")]
        [InlineData(2, "tvær klukkustundir")]
        public void HoursToWords(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "ein mínúta")]
        [InlineData(2, "2 mínútur")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "ein mínúta")]
        [InlineData(2, "tvær mínútur")]
        public void MinutesToWords(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: true));
        }


        [Theory]
        [InlineData(1, "ein sekúnda")]
        [InlineData(2, "2 sekúndur")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "ein sekúnda")]
        [InlineData(2, "tvær sekúndur")]
        public void SecondsToWords(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1, "ein millisekúnda")]
        [InlineData(2, "2 millisekúndur")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Theory]
        [InlineData(1, "ein millisekúnda")]
        [InlineData(2, "tvær millisekúndur")]
        public void MillisecondsToWords(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: true));
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 millisekúndur", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("engin stund", TimeSpan.Zero.Humanize(toWords: true));
        }

        [Theory]
        [InlineData(1299630020, 5, "tvær vikur, einn dagur, ein klukkustund, þrjátíu sekúndur, tuttugu millisekúndur")]
        public void TimeSpanWithNumbersConvertedToWords(int milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, toWords: true);
            Assert.Equal(expected, actual);
        }
    }
}
