using System;

using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.fr
{
    [UseCulture("fr")]
    public class TimeUnitToSymbolTests
    {
        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(TimeUnit.Millisecond, "ms")]
        [InlineData(TimeUnit.Second, "s")]
        [InlineData(TimeUnit.Minute, "min")]
        [InlineData(TimeUnit.Hour, "h")]
        [InlineData(TimeUnit.Day, "j")]
        [InlineData(TimeUnit.Week, "semaine")]
        [InlineData(TimeUnit.Month, "mois")]
        [InlineData(TimeUnit.Year, "a")]
        public void ToSymbol(TimeUnit unit, string expected)
        {
            Assert.Equal(expected, unit.ToSymbol());
        }
    }
}