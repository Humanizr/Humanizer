﻿using System;
using System.Globalization;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class DefaultFormatterTests
    {

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
