using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.nl
{
    public class OrdinalizeTests : AmbientCulture
    {
        public OrdinalizeTests()
            : base("nl")
        {
        }

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1e")]
        [InlineData("2", "2e")]
        [InlineData("3", "3e")]
        [InlineData("4", "4e")]
        [InlineData("5", "5e")]
        [InlineData("6", "6e")]
        [InlineData("23", "23e")]
        [InlineData("100", "100e")]
        [InlineData("101", "101e")]
        [InlineData("102", "102e")]
        [InlineData("103", "103e")]
        [InlineData("1001", "1001e")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }        
     }
}