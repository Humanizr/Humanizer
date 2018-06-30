using Xunit;

namespace Humanizer.Tests.Localisation.it
{
    [UseCulture("it")]
    public class OrdinalizerTests
    {

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1°")]
        [InlineData(11, "11°")]
        [InlineData(111, "111°")]
        public void GenderlessNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize());
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData("1", "1°")]
        [InlineData("11", "11°")]
        [InlineData("111", "111°")]
        public void GenderlessText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize());
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1°")]
        [InlineData(11, "11°")]
        [InlineData(111, "111°")]
        public void MasculineNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData("1", "1°")]
        [InlineData("11", "11°")]
        [InlineData("111", "111°")]
        public void MasculineText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1ª")]
        [InlineData(11, "11ª")]
        [InlineData(111, "111ª")]
        public void FeminineNumber(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData("0", "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData("1", "1ª")]
        [InlineData("11", "11ª")]
        [InlineData("111", "111ª")]
        public void FeminineText(string number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine));
        }
    }
}
