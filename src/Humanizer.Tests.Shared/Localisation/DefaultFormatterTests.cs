using System;
using System.Globalization;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class DefaultFormatterTests
    {
        [Theory]
        [InlineData(TimeUnit.Month, 1)]
        [InlineData(TimeUnit.Month, 2)]
        [InlineData(TimeUnit.Month, 10)]
        [InlineData(TimeUnit.Year, 1)]
        [InlineData(TimeUnit.Year, 2)]
        [InlineData(TimeUnit.Year, 10)]
        public void TimeSpanHumanizeThrowsExceptionForTimeUnitsLargerThanWeek(TimeUnit timeUnit, int unit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DefaultFormatter(CultureInfo.InvariantCulture.Name).TimeSpanHumanize(timeUnit, unit));
        }

        [Fact]
        [UseCulture("es")]
        public void HandlesNotImplementedCollectionFormattersGracefully()
        {
            var a = new[] {DateTime.UtcNow, DateTime.UtcNow.AddDays(10)};
            var b = a.Humanize();

            Assert.Equal(a[0] + " & " + a[1], b);
        }
    }
}
