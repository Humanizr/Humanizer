using Xunit;

namespace Humanizer.Tests.Extensions.Inflector
{
    public class DasherizeTests : InflectorTestBase
    {
        [Fact]
        public void Dasherize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Dasherize(), pair.Value);
            }
        }

        public DasherizeTests()
        {
            //Just replaces underscore with a dash
            TestData.Add("some_title", "some-title");
            TestData.Add("some-title", "some-title");
            TestData.Add("some_title_goes_here", "some-title-goes-here");
            TestData.Add("some_title and_another", "some-title and-another");
        }
    }
}