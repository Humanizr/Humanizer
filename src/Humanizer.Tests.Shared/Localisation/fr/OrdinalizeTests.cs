using Xunit;

namespace Humanizer.Tests.Localisation.fr
{
    [UseCulture("fr")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0ème")]
        [InlineData("1", "1er")]
        [InlineData("2", "2ème")]
        [InlineData("3", "3ème")]
        [InlineData("4", "4ème")]
        [InlineData("5", "5ème")]
        [InlineData("6", "6ème")]
        [InlineData("23", "23ème")]
        [InlineData("100", "100ème")]
        [InlineData("101", "101ème")]
        [InlineData("102", "102ème")]
        [InlineData("103", "103ème")]
        [InlineData("1001", "1001ème")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0ème")]
        [InlineData("1", "1ère")]
        [InlineData("2", "2ème")]
        [InlineData("3", "3ème")]
        [InlineData("4", "4ème")]
        [InlineData("5", "5ème")]
        [InlineData("6", "6ème")]
        [InlineData("23", "23ème")]
        [InlineData("100", "100ème")]
        [InlineData("101", "101ème")]
        [InlineData("102", "102ème")]
        [InlineData("103", "103ème")]
        [InlineData("1001", "1001ème")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0ème")]
        [InlineData("1", "1er")]
        [InlineData("2", "2ème")]
        [InlineData("3", "3ème")]
        [InlineData("4", "4ème")]
        [InlineData("5", "5ème")]
        [InlineData("6", "6ème")]
        [InlineData("23", "23ème")]
        [InlineData("100", "100ème")]
        [InlineData("101", "101ème")]
        [InlineData("102", "102ème")]
        [InlineData("103", "103ème")]
        [InlineData("1001", "1001ème")]
        public void OrdinalizeStringNeuter(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Neuter), ordinalized);
        }

        [Theory]
        [InlineData(0, "0ème")]
        [InlineData(1, "1er")]
        [InlineData(2, "2ème")]
        [InlineData(3, "3ème")]
        [InlineData(4, "4ème")]
        [InlineData(5, "5ème")]
        [InlineData(6, "6ème")]
        [InlineData(10, "10ème")]
        [InlineData(23, "23ème")]
        [InlineData(100, "100ème")]
        [InlineData(101, "101ème")]
        [InlineData(102, "102ème")]
        [InlineData(103, "103ème")]
        [InlineData(1001, "1001ème")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0ème")]
        [InlineData(1, "1ère")]
        [InlineData(2, "2ème")]
        [InlineData(3, "3ème")]
        [InlineData(4, "4ème")]
        [InlineData(5, "5ème")]
        [InlineData(6, "6ème")]
        [InlineData(10, "10ème")]
        [InlineData(23, "23ème")]
        [InlineData(100, "100ème")]
        [InlineData(101, "101ème")]
        [InlineData(102, "102ème")]
        [InlineData(103, "103ème")]
        [InlineData(1001, "1001ème")]
        public void OrdinalizeNumberFeminine(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0ème")]
        [InlineData(1, "1er")]
        [InlineData(2, "2ème")]
        [InlineData(3, "3ème")]
        [InlineData(4, "4ème")]
        [InlineData(5, "5ème")]
        [InlineData(6, "6ème")]
        [InlineData(10, "10ème")]
        [InlineData(23, "23ème")]
        [InlineData(100, "100ème")]
        [InlineData(101, "101ème")]
        [InlineData(102, "102ème")]
        [InlineData(103, "103ème")]
        [InlineData(1001, "1001ème")]
        public void OrdinalizeNumberNeuter(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Neuter), ordinalized);
        }
    }
}
