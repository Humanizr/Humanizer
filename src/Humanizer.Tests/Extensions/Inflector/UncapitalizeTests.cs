using Xunit;

namespace Humanizer.Tests.Extensions.Inflector
{
    public class UncapitalizeTests : InflectorTestBase
    {
        [Fact]
        public void Uncapitalize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Uncapitalize(), pair.Value);
            }
        }

        public UncapitalizeTests()
        {
            //Just lowers the first char and leaves the rest alone
            TestData.Add("some title", "some title");
            TestData.Add("some Title", "some Title");
            TestData.Add("SOMETITLE", "sOMETITLE");
            TestData.Add("someTitle", "someTitle");
            TestData.Add("some title goes here", "some title goes here");
            TestData.Add("some TITLE", "some TITLE");
        }
    }
}