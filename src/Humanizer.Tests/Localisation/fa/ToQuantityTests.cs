using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fa
{
    public class ToQuantityTests : AmbientCulture
    {
        public ToQuantityTests()
            : base("fa") { }

        [Theory]
        [InlineData("مرد", 0, "0 مرد")]
        [InlineData("مرد", 1, "1 مرد")]
        [InlineData("مرد", 5, "5 مرد")]
        public void ToQuantity(string word, int quatity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quatity));
        }

        [Theory]
        [InlineData("مرد", 0, "مرد")]
        [InlineData("مرد", 1, "مرد")]
        [InlineData("مرد", 5, "مرد ها")]
        public void ToQuantityWithNoQuantity(string word, int quatity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quatity, ShowQuantityAs.None));
        }

        [Theory]
        [InlineData("مرد", 0, "0 مرد")]
        [InlineData("مرد", 1, "1 مرد")]
        [InlineData("مرد", 5, "5 مرد")]
        public void ToQuantityNumeric(string word, int quatity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quatity, ShowQuantityAs.Numeric));
        }

        [Theory]
        [InlineData("مرد", 2, "دو مرد")]
        [InlineData("مرد", 1, "یک مرد")]
        [InlineData("مرد", 1200, "یک هزار و دویست مرد")]
        public void ToQuantityWords(string word, int quatity, string expected)
        {
            Assert.Equal(expected, word.ToQuantity(quatity, ShowQuantityAs.Words));
        }
    }
}
