using System;
using Xunit;

namespace Humanizer.Tests.Localisation.lv
{
    [UseCulture("lv")]
    public class DateToOrdinalWordsTests
    {
        [Fact]
        public void OrdinalizeString()
        {
            Assert.Equal("1 janvāris 2015", new DateTime(2015, 1, 1).ToOrdinalWords());
        }
    }
}
