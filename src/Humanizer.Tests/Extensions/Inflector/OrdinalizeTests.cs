using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Extensions.Inflector
{
    public class OrdinalizeTests : InflectorTestBase
    {
        [Fact]
        public void Ordinalize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Ordinalize(), pair.Value);
            }
        }

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
        [Theory]
        public void OrdanizeNumbersTest(int number, string ordanized)
        {
            Assert.Equal(number.Ordinalize(), ordanized);
        }

        public OrdinalizeTests()
        {
            TestData.Add("0", "0th");
            TestData.Add("1", "1st");
            TestData.Add("2", "2nd");
            TestData.Add("3", "3rd");
            TestData.Add("4", "4th");
            TestData.Add("5", "5th");
            TestData.Add("6", "6th");
            TestData.Add("7", "7th");
            TestData.Add("8", "8th");
            TestData.Add("9", "9th");
            TestData.Add("10", "10th");
            TestData.Add("11", "11th");
            TestData.Add("12", "12th");
            TestData.Add("13", "13th");
            TestData.Add("14", "14th");
            TestData.Add("20", "20th");
            TestData.Add("21", "21st");
            TestData.Add("22", "22nd");
            TestData.Add("23", "23rd");
            TestData.Add("24", "24th");
            TestData.Add("100", "100th");
            TestData.Add("101", "101st");
            TestData.Add("102", "102nd");
            TestData.Add("103", "103rd");
            TestData.Add("104", "104th");
            TestData.Add("110", "110th");
            TestData.Add("1000", "1000th");
            TestData.Add("1001", "1001st");
        }
    }
}