using Xunit;

namespace Humanizer.Tests.Localisation.pt
{
    [UseCulture("pt")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1º")]
        [InlineData("2", "2º")]
        [InlineData("3", "3º")]
        [InlineData("4", "4º")]
        [InlineData("5", "5º")]
        [InlineData("6", "6º")]
        [InlineData("23", "23º")]
        [InlineData("100", "100º")]
        [InlineData("101", "101º")]
        [InlineData("102", "102º")]
        [InlineData("103", "103º")]
        [InlineData("1001", "1001º")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1ª")]
        [InlineData("2", "2ª")]
        [InlineData("3", "3ª")]
        [InlineData("4", "4ª")]
        [InlineData("5", "5ª")]
        [InlineData("6", "6ª")]
        [InlineData("23", "23ª")]
        [InlineData("100", "100ª")]
        [InlineData("101", "101ª")]
        [InlineData("102", "102ª")]
        [InlineData("103", "103ª")]
        [InlineData("1001", "1001ª")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1º")]
        [InlineData(2, "2º")]
        [InlineData(3, "3º")]
        [InlineData(4, "4º")]
        [InlineData(5, "5º")]
        [InlineData(6, "6º")]
        [InlineData(10, "10º")]
        [InlineData(23, "23º")]
        [InlineData(100, "100º")]
        [InlineData(101, "101º")]
        [InlineData(102, "102º")]
        [InlineData(103, "103º")]
        [InlineData(1001, "1001º")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1ª")]
        [InlineData(2, "2ª")]
        [InlineData(3, "3ª")]
        [InlineData(4, "4ª")]
        [InlineData(5, "5ª")]
        [InlineData(6, "6ª")]
        [InlineData(10, "10ª")]
        [InlineData(23, "23ª")]
        [InlineData(100, "100ª")]
        [InlineData(101, "101ª")]
        [InlineData(102, "102ª")]
        [InlineData(103, "103ª")]
        [InlineData(1001, "1001ª")]
        public void OrdinalizeNumberFeminine(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }
    }
}
