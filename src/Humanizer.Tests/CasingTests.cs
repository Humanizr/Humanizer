using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class CasingTests
    {
        [Theory]
        [InlineData("lower case statement", "Lower Case Statement")]
        [InlineData("Sentence casing", "Sentence Casing")]
        [InlineData("honors UPPER case", "Honors UPPER Case")]
        [InlineData("Title Case", "Title Case")]
        public void ToTitle(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.ToTitle());
        }

        //Makes an underscored lowercase string
        [InlineData("some_title", "some-title")]
        [InlineData("some-title", "some-title")]
        [InlineData("some_title_goes_here", "some-title-goes-here")]
        [InlineData("some_title and_another", "some-title and-another")]
        [Theory]
        public void Dasherize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Dasherize());
        }

        [Theory]
        [InlineData("customer", "Customer")]
        [InlineData("CUSTOMER", "CUSTOMER")]
        [InlineData("CUStomer", "CUStomer")]
        [InlineData("customer_name", "CustomerName")]
        [InlineData("customer_first_name", "CustomerFirstName")]
        [InlineData("customer_first_name_goes_here", "CustomerFirstNameGoesHere")]
        [InlineData("customer name", "Customer name")]
        public void Pascalize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Pascalize());
        }

        // Same as pascalize, except first char is lowercase
        [Theory]
        [InlineData("customer", "customer")]
        [InlineData("CUSTOMER", "cUSTOMER")]
        [InlineData("CUStomer", "cUStomer")]
        [InlineData("customer_name", "customerName")]
        [InlineData("customer_first_name", "customerFirstName")]
        [InlineData("customer_first_name_goes_here", "customerFirstNameGoesHere")]
        [InlineData("customer name", "customer name")]
        public void Camelize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Camelize());
        }

        [Theory]
        [InlineData("SomeTitle", "some_title")]
        [InlineData("someTitle", "some_title")]
        [InlineData("some title", "some_title")]
        [InlineData("some title that will be underscored", "some_title_that_will_be_underscored")]
        [InlineData("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored")]
        public void Underscore(string input, string expectedOuput)
        {
            Assert.Equal(expectedOuput, input.Underscore());
        }
    }
}