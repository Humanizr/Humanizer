using System;
using Xunit;

namespace Humanizer.Tests.Localisation.he
{
    [UseCulture("he")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "שנה")]
        [InlineData(731, "שנתיים")]
        [InlineData(1096, "3 שנים")]
        [InlineData(4018, "11 שנים")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "חודש")]
        [InlineData(61, "חודשיים")]
        [InlineData(92, "3 חודשים")]
        [InlineData(335, "11 חודשים")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "שבוע")]
        [InlineData(14, "שבועיים")]
        [InlineData(21, "3 שבועות")]
        [InlineData(77, "11 שבועות")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "יום")]
        [InlineData(2, "יומיים")]
        [InlineData(3, "3 ימים")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "שעה")]
        [InlineData(2, "שעתיים")]
        [InlineData(3, "3 שעות")]
        [InlineData(11, "11 שעות")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "דקה")]
        [InlineData(2, "שתי דקות")]
        [InlineData(3, "3 דקות")]
        [InlineData(11, "11 דקות")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "שנייה")]
        [InlineData(2, "שתי שניות")]
        [InlineData(3, "3 שניות")]
        [InlineData(11, "11 שניות")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "אלפית שנייה")]
        [InlineData(2, "שתי אלפיות שנייה")]
        [InlineData(3, "3 אלפיות שנייה")]
        [InlineData(11, "11 אלפיות שנייה")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 אלפיות שנייה", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("אין זמן", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
