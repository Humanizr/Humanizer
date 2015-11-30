using Xunit;

namespace Humanizer.Tests.Localisation.ruRU
{
    [UseCulture("ru-RU")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0-й")]
        [InlineData("1", "1-й")]
        [InlineData("2", "2-й")]
        [InlineData("3", "3-й")]
        [InlineData("4", "4-й")]
        [InlineData("5", "5-й")]
        [InlineData("6", "6-й")]
        [InlineData("23", "23-й")]
        [InlineData("100", "100-й")]
        [InlineData("101", "101-й")]
        [InlineData("102", "102-й")]
        [InlineData("103", "103-й")]
        [InlineData("1001", "1001-й")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0-я")]
        [InlineData("1", "1-я")]
        [InlineData("2", "2-я")]
        [InlineData("3", "3-я")]
        [InlineData("4", "4-я")]
        [InlineData("5", "5-я")]
        [InlineData("6", "6-я")]
        [InlineData("23", "23-я")]
        [InlineData("100", "100-я")]
        [InlineData("101", "101-я")]
        [InlineData("102", "102-я")]
        [InlineData("103", "103-я")]
        [InlineData("1001", "1001-я")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0-е")]
        [InlineData("1", "1-е")]
        [InlineData("2", "2-е")]
        [InlineData("3", "3-е")]
        [InlineData("4", "4-е")]
        [InlineData("5", "5-е")]
        [InlineData("6", "6-е")]
        [InlineData("23", "23-е")]
        [InlineData("100", "100-е")]
        [InlineData("101", "101-е")]
        [InlineData("102", "102-е")]
        [InlineData("103", "103-е")]
        [InlineData("1001", "1001-е")]
        public void OrdinalizeStringNeuter(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Neuter), ordinalized);
        }

        [Theory]
        [InlineData(0, "0-й")]
        [InlineData(1, "1-й")]
        [InlineData(2, "2-й")]
        [InlineData(3, "3-й")]
        [InlineData(4, "4-й")]
        [InlineData(5, "5-й")]
        [InlineData(6, "6-й")]
        [InlineData(10, "10-й")]
        [InlineData(23, "23-й")]
        [InlineData(100, "100-й")]
        [InlineData(101, "101-й")]
        [InlineData(102, "102-й")]
        [InlineData(103, "103-й")]
        [InlineData(1001, "1001-й")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0-я")]
        [InlineData(1, "1-я")]
        [InlineData(2, "2-я")]
        [InlineData(3, "3-я")]
        [InlineData(4, "4-я")]
        [InlineData(5, "5-я")]
        [InlineData(6, "6-я")]
        [InlineData(10, "10-я")]
        [InlineData(23, "23-я")]
        [InlineData(100, "100-я")]
        [InlineData(101, "101-я")]
        [InlineData(102, "102-я")]
        [InlineData(103, "103-я")]
        [InlineData(1001, "1001-я")]
        public void OrdinalizeNumberFeminine(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0-е")]
        [InlineData(1, "1-е")]
        [InlineData(2, "2-е")]
        [InlineData(3, "3-е")]
        [InlineData(4, "4-е")]
        [InlineData(5, "5-е")]
        [InlineData(6, "6-е")]
        [InlineData(23, "23-е")]
        [InlineData(100, "100-е")]
        [InlineData(101, "101-е")]
        [InlineData(102, "102-е")]
        [InlineData(103, "103-е")]
        [InlineData(1001, "1001-е")]
        public void OrdinalizeNumberNeuter(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Neuter), ordinalized);
        }
    }
}
