using System;
using Xunit;

namespace Humanizer.Tests.Localisation.hr
{
    [UseCulture("hr-HR")]
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
        [InlineData(31, "1 mjesec")]
        [InlineData(61, "2 mjeseca")]
        [InlineData(92, "3 mjeseca")]
        [InlineData(335, "11 mjeseci")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(1, "1 dan")]
        [InlineData(2, "2 dana")]
        [InlineData(3, "3 dana")]
        [InlineData(4, "4 dana")]
        [InlineData(5, "5 dana")]
        [InlineData(7, "1 tjedan")]
        [InlineData(14, "2 tjedna")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }
    }
}
