using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class TimeUnitToSymbolTests
    {
        [Theory]
        [InlineData(TimeUnit.Millisecond, "ms")]
        [InlineData(TimeUnit.Second, "s")]
        [InlineData(TimeUnit.Minute, "min")]
        [InlineData(TimeUnit.Hour, "h")]
        [InlineData(TimeUnit.Day, "d")]
        [InlineData(TimeUnit.Week, "week")]
        [InlineData(TimeUnit.Month, "mo")]
        [InlineData(TimeUnit.Year, "y")]
        public void ToSymbol(TimeUnit unit, string expected)
        {
            Assert.Equal(expected, unit.ToSymbol());
        }
    }
}
