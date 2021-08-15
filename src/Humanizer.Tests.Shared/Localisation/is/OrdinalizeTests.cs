using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    [UseCulture("is")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0.")]
        [InlineData("1", "1.")]
        [InlineData("2", "2.")]
        [InlineData("3", "3.")]
        [InlineData("4", "4.")]
        [InlineData("5", "5.")]
        [InlineData("6", "6.")]
        [InlineData("23", "23.")]
        [InlineData("100", "100.")]
        [InlineData("101", "101.")]
        [InlineData("102", "102.")]
        [InlineData("103", "103.")]
        [InlineData("1001", "1001.")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(ordinalized, number.Ordinalize());
        }
    }
}
