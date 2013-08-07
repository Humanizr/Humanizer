using Xunit;

namespace Humanizer.Tests.Extensions.Inflector
{
    public class UnderscoreTests : InflectorTestBase
    {
        [Fact]
        public void Underscore()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Underscore(), pair.Value);
            }
        }

        public UnderscoreTests()
        {
            //Makes an underscored lowercase string
            TestData.Add("SomeTitle", "some_title");
            TestData.Add("someTitle", "some_title");
            TestData.Add("some title", "some_title");
            TestData.Add("some title that will be underscored", "some_title_that_will_be_underscored");
            TestData.Add("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored");
        }
    }
}