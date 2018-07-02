using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-ES")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1.º")]
        [InlineData("2", "2.º")]
        [InlineData("3", "3.º")]
        [InlineData("4", "4.º")]
        [InlineData("5", "5.º")]
        [InlineData("6", "6.º")]
        [InlineData("23", "23.º")]
        [InlineData("100", "100.º")]
        [InlineData("101", "101.º")]
        [InlineData("102", "102.º")]
        [InlineData("103", "103.º")]
        [InlineData("1001", "1001.º")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1.ª")]
        [InlineData("2", "2.ª")]
        [InlineData("3", "3.ª")]
        [InlineData("4", "4.ª")]
        [InlineData("5", "5.ª")]
        [InlineData("6", "6.ª")]
        [InlineData("23", "23.ª")]
        [InlineData("100", "100.ª")]
        [InlineData("101", "101.ª")]
        [InlineData("102", "102.ª")]
        [InlineData("103", "103.ª")]
        [InlineData("1001", "1001.ª")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1.º")]
        [InlineData(2, "2.º")]
        [InlineData(3, "3.º")]
        [InlineData(4, "4.º")]
        [InlineData(5, "5.º")]
        [InlineData(6, "6.º")]
        [InlineData(10, "10.º")]
        [InlineData(23, "23.º")]
        [InlineData(100, "100.º")]
        [InlineData(101, "101.º")]
        [InlineData(102, "102.º")]
        [InlineData(103, "103.º")]
        [InlineData(1001, "1001.º")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1.ª")]
        [InlineData(2, "2.ª")]
        [InlineData(3, "3.ª")]
        [InlineData(4, "4.ª")]
        [InlineData(5, "5.ª")]
        [InlineData(6, "6.ª")]
        [InlineData(10, "10.ª")]
        [InlineData(23, "23.ª")]
        [InlineData(100, "100.ª")]
        [InlineData(101, "101.ª")]
        [InlineData(102, "102.ª")]
        [InlineData(103, "103.ª")]
        [InlineData(1001, "1001.ª")]
        public void OrdinalizeNumberFeminine(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }
    }
}
