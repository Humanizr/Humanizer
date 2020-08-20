using Xunit;

namespace Humanizer.Tests.Localisation.hy
{
    [UseCulture("hy")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0-րդ")]
        [InlineData("1", "1-ին")]
        [InlineData("2", "2-րդ")]
        [InlineData("103", "103-րդ")]
        [InlineData("1001", "1001-րդ")]
        public void OrdinalizeStringMasculine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0-րդ")]
        [InlineData("1", "1-ին")]
        [InlineData("2", "2-րդ")]
        [InlineData("103", "103-րդ")]
        [InlineData("1001", "1001-րդ")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0-րդ")]
        [InlineData("1", "1-ին")]
        [InlineData("2", "2-րդ")]
        [InlineData("103", "103-րդ")]
        [InlineData("1001", "1001-րդ")]
        public void OrdinalizeStringNeuter(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Neuter), ordinalized);
        }
    }
}
