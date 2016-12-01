using System;
using Xunit;

namespace Humanizer.Tests.Localisation.hr
{
    [UseCulture("hr-HR")]
    public class TimeSpanHumanizeTests
    {
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
