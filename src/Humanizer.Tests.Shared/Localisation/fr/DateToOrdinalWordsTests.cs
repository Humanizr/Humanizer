using System;
using Xunit;

namespace Humanizer.Tests.Localisation.fr
{
    [UseCulture("fr")]
    public class DateToOrdinalWordsTests
    {
        [Fact]
        public void OrdinalizeString()
        {
            Assert.Equal("1er janvier 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
            Assert.Equal("2 mars 2020", new DateTime(2020, 3, 2).ToOrdinalWords());
            Assert.Equal("31 octobre 2021", new DateTime(2021, 10, 31).ToOrdinalWords());
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void OrdinalizeDateOnlyString()
        {
            Assert.Equal("1er janvier 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());
            Assert.Equal("2 mars 2020", new DateOnly(2020, 3, 2).ToOrdinalWords());
            Assert.Equal("31 octobre 2021", new DateOnly(2021, 10, 31).ToOrdinalWords());
        }
#endif
    }
}
