using System;
using System.Globalization;
using System.Threading;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using Xunit;
using Xunit.Extensions;

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
        public void Issue_392_A_collection_formatter_for_the_current_culture_has_not_been_implemented_yet()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es");

            // start: code from issue report
            var a = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
            var b = a.Humanize(); // THROWS!
            // end: code from issue report

            Assert.Equal(a[0] + " & " + a[1], b);

            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUiCulture;
        }
    }
}