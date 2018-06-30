using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-ES")]
    public class NumberToWordsFeminineTests
    {

        [Theory]
        [InlineData(1, "una")]
        [InlineData(21, "veintiuna")]
        [InlineData(31, "treinta y una")]
        [InlineData(81, "ochenta y una")]
        [InlineData(500, "quinientas")]
        [InlineData(701, "setecientas una")]
        [InlineData(3500, "tres mil quinientas")]
        [InlineData(200121, "doscientas mil ciento veintiuna")]
        [InlineData(200000121, "doscientos millones ciento veintiuna")]
        [InlineData(1000001, "un millón una")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

    }
}
