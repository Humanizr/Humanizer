using System;
using Xunit;
namespace Humanizer.Tests.Localisation.el
{
    [UseCulture("el")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "1 χρόνο")]
        [InlineData(731, "2 χρόνια")]
        [InlineData(1096, "3 χρόνια")]
        [InlineData(4018, "11 χρόνια")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "1 μήνα")]
        [InlineData(61, "2 μήνες")]
        [InlineData(92, "3 μήνες")]
        [InlineData(335, "11 μήνες")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "1 βδομάδα")]
        [InlineData(14, "2 βδομάδες")]
        [InlineData(21, "3 βδομάδες")]
        [InlineData(77, "11 βδομάδες")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }


        [Theory]
        [InlineData(1, "1 μέρα")]
        [InlineData(2, "2 μέρες")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "1 ώρα")]
        [InlineData(2, "2 ώρες")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "1 λεπτό")]
        [InlineData(2, "2 λεπτά")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }


        [Theory]
        [InlineData(1, "1 δευτερόλεπτο")]
        [InlineData(2, "2 δευτερόλεπτα")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "1 χιλιοσtό του δευτερολέπτου")]
        [InlineData(2, "2 χιλιοστά του δευτερολέπτου")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 χιλιοστά του δευτερολέπτου", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            Assert.Equal("μηδέν χρόνος", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
