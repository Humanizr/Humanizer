using System;

using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.de
{
    [UseCulture("de-DE")]
    public class TimeUnitToSymbolTests
    {
        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(TimeUnit.Millisecond, "ms")]
        [InlineData(TimeUnit.Second, "s")]
        [InlineData(TimeUnit.Minute, "min")]
        [InlineData(TimeUnit.Hour, "h")]
        [InlineData(TimeUnit.Day, "d")]
        [InlineData(TimeUnit.Week, "Woche")]
        [InlineData(TimeUnit.Month, "M")]
        [InlineData(TimeUnit.Year, "a")]
        public void ToSymbol(TimeUnit unit, string expected)
        {
            Assert.Equal(expected, unit.ToSymbol());
        }
    }
}
