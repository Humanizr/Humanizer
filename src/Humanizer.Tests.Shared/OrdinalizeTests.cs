using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class OrdinalizeTests
    {

        [Theory]
        [InlineData("0", "0th")]
        [InlineData("1", "1st")]
        [InlineData("2", "2nd")]
        [InlineData("3", "3rd")]
        [InlineData("4", "4th")]
        [InlineData("5", "5th")]
        [InlineData("6", "6th")]
        [InlineData("7", "7th")]
        [InlineData("8", "8th")]
        [InlineData("9", "9th")]
        [InlineData("10", "10th")]
        [InlineData("11", "11th")]
        [InlineData("12", "12th")]
        [InlineData("13", "13th")]
        [InlineData("14", "14th")]
        [InlineData("20", "20th")]
        [InlineData("21", "21st")]
        [InlineData("22", "22nd")]
        [InlineData("23", "23rd")]
        [InlineData("24", "24th")]
        [InlineData("100", "100th")]
        [InlineData("101", "101st")]
        [InlineData("102", "102nd")]
        [InlineData("103", "103rd")]
        [InlineData("104", "104th")]
        [InlineData("110", "110th")]
        [InlineData("1000", "1000th")]
        [InlineData("1001", "1001st")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(0, "0th")]
        [InlineData(1, "1st")]
        [InlineData(2, "2nd")]
        [InlineData(3, "3rd")]
        [InlineData(4, "4th")]
        [InlineData(5, "5th")]
        [InlineData(6, "6th")]
        [InlineData(7, "7th")]
        [InlineData(8, "8th")]
        [InlineData(9, "9th")]
        [InlineData(10, "10th")]
        [InlineData(11, "11th")]
        [InlineData(12, "12th")]
        [InlineData(13, "13th")]
        [InlineData(14, "14th")]
        [InlineData(20, "20th")]
        [InlineData(21, "21st")]
        [InlineData(22, "22nd")]
        [InlineData(23, "23rd")]
        [InlineData(24, "24th")]
        [InlineData(100, "100th")]
        [InlineData(101, "101st")]
        [InlineData(102, "102nd")]
        [InlineData(103, "103rd")]
        [InlineData(104, "104th")]
        [InlineData(110, "110th")]
        [InlineData(1000, "1000th")]
        [InlineData(1001, "1001st")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(8)]
        public void OrdinalizeNumberGenderIsImmaterial(int number)
        {
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("8")]
        public void OrdinalizeStringGenderIsImmaterial(string number)
        {
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }
    }
}
