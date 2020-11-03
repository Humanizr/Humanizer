using Xunit;

namespace Humanizer.Tests.Localisation.roRO
{
    [UseCulture("ro-RO")]
    public class OrdinalizerTests
    {

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData(1, "primul")]
        [InlineData(2, "al 2-lea")]
        [InlineData(3, "al 3-lea")]
        [InlineData(10, "al 10-lea")]
        public void GenderlessNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize());
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData("1", "primul")]
        [InlineData("2", "al 2-lea")]
        [InlineData("3", "al 3-lea")]
        [InlineData("10", "al 10-lea")]
        public void GenderlessText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize());
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData(1, "primul")]
        [InlineData(2, "al 2-lea")]
        [InlineData(3, "al 3-lea")]
        [InlineData(10, "al 10-lea")]
        public void MasculineNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData("1", "primul")]
        [InlineData("2", "al 2-lea")]
        [InlineData("3", "al 3-lea")]
        [InlineData("10", "al 10-lea")]
        public void MasculineText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData(1, "prima")]
        [InlineData(2, "a 2-a")]
        [InlineData(10, "a 10-a")]
        public void FeminineNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 (zero) in Romanian.
        [InlineData("1", "prima")]
        [InlineData("2", "a 2-a")]
        [InlineData("10", "a 10-a")]
        public void FeminineText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine));
        }
    }
}
