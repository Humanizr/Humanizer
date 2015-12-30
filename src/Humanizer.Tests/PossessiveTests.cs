using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class PossessiveTests
    {
        [Theory]
        [InlineData("Joe", "Joe's")]
        [InlineData("Car", "Car's")]
        [InlineData("Jones", "Jones'")]
        [InlineData("Joneses", "Joneses'")]
        [InlineData("Desks", "Desks'")]
        [InlineData("his", "his")]
        [InlineData("Yours", "Yours")]
        [InlineData("Illinois", "Illinois's")]
        [InlineData("corps", "corps's")]
        public void ApplyPossessive(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.ToPossessive());
        }
    }
}
