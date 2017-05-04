using System;
using Xunit;

namespace Humanizer.Tests.Localisation.sv
{
    [UseCulture("sv-SE")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [InlineData(1, "1 millisekund")]
        [InlineData(2, "2 millisekunder")]
        public void Milliseconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 sekund")]
        [InlineData(2, "2 sekunder")]
        public void Seconds(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 minut")]
        [InlineData(2, "2 minuter")]
        public void Minutes(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 timma")]
        [InlineData(2, "2 timmar")]
        public void Hours(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 dag")]
        [InlineData(2, "2 dagar")]
        public void Days(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number).Humanize());
        }

        [Theory]
        [InlineData(1, "1 vecka")]
        [InlineData(2, "2 veckor")]
        public void Weeks(int number, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(number * 7).Humanize());
        }


        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "en månad")]
        [InlineData(61, "2 månader")]
        [InlineData(92, "3 månader")]
        [InlineData(335, "11 månader")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "ett år")]
        [InlineData(731, "2 år")]
        [InlineData(1096, "3 år")]
        [InlineData(4018, "11 år")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }
    }
}
