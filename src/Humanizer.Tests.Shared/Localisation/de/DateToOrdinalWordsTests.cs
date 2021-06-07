using System;
using Xunit;

namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de")]
    public class DateToOrdinalWordsTests
    {
        [Fact]
        public void OrdinalizeString()
        {
            Assert.Equal("1. Januar 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void OrdinalizeDateOnlyString()
        {
            Assert.Equal("1. Januar 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());
        }
#endif
    }
}
