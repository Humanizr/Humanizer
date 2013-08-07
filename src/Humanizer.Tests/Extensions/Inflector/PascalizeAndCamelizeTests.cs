using Xunit;

namespace Humanizer.Tests.Extensions.Inflector
{
    public class PascalizeAndCamelizeTests : InflectorTestBase
    {
        [Fact]
        public void Pascalize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Pascalize(), pair.Value);
            }
        }

        /// <summary>
        /// Same as pascalize, except first char is lowercase
        /// </summary>
        [Fact]
        public void Camelize()
        {
            foreach (var pair in TestData)
            {
                var lowercaseFirstChar = pair.Value.Substring(0, 1).ToLower() + pair.Value.Substring(1);
                Assert.Equal(pair.Key.Camelize(), lowercaseFirstChar);
            }
        }

        public PascalizeAndCamelizeTests()
        {
            TestData.Add("customer", "Customer");
            TestData.Add("CUSTOMER", "CUSTOMER");
            TestData.Add("CUStomer", "CUStomer");
            TestData.Add("customer_name", "CustomerName");
            TestData.Add("customer_first_name", "CustomerFirstName");
            TestData.Add("customer_first_name_goes_here", "CustomerFirstNameGoesHere");
            TestData.Add("customer name", "Customer name");
        }
    }
}