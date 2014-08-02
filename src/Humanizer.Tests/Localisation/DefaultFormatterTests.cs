﻿
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using System;
using System.Globalization;
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
    }
}
