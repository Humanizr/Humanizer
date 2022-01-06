using System;

using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class DefaultFormatterTests
    {

        [Fact]
        [UseCulture("iv")]
        public void HandlesNotImplementedCollectionFormattersGracefully()
        {
            var a = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
            var b = a.Humanize();

            Assert.Equal(a[0] + " & " + a[1], b);
        }
    }
}
