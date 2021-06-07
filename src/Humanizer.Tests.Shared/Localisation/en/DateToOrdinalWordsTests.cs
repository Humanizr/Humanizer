using System;
using Xunit;

namespace Humanizer.Tests.Localisation.en
{
    public class DateToOrdinalWordsTests
    {
        [UseCulture("en-GB")]
        [Fact]
        public void OrdinalizeStringGb()
        {
            Assert.Equal("1st January 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
        }

        [UseCulture("en-US")]
        [Fact]
        public void OrdinalizeStringUs()
        {
            Assert.Equal("January 1st, 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
        }

#if NET6_0_OR_GREATER
        [UseCulture("en-GB")]
        [Fact]
        public void OrdinalizeDateOnlyStringGb()
        {
            Assert.Equal("1st January 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());
        }

        [UseCulture("en-US")]
        [Fact]
        public void OrdinalizeDateOnlyStringUs()
        {
            Assert.Equal("January 1st, 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());
        }
#endif
    }
}
