using System.Globalization;
using Xunit;

namespace Humanizer.Tests.Localisation.thTH
{
    [UseCulture("th-TH")]
    public class NumberToWordsTests
    {
        [InlineData(1, "หนึ่ง")]
        [InlineData(10, "สิบ")]
        [InlineData(11, "สิบเอ็ด")]
        [InlineData(20, "ยี่สิบ")]
        [InlineData(-122, "ลบหนึ่งร้อยยี่สิบสอง")]
        [InlineData(3501, "สามพันห้าร้อยหนึ่ง")]
        [InlineData(100, "หนึ่งร้อย")]
        [InlineData(1000, "หนึ่งพัน")]
        [InlineData(10000, "หนึ่งหมื่น")]
        [InlineData(-100000, "ลบหนึ่งแสน")]
        [InlineData(1000000, "หนึ่งล้าน")]
        [InlineData(10000000, "สิบล้าน")]
        [InlineData(100000000, "หนึ่งร้อยล้าน")]
        [InlineData(1000000000, "หนึ่งพันล้าน")]
        [InlineData(111, "หนึ่งร้อยสิบเอ็ด")]
        [InlineData(1111, "หนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [InlineData(-111111, "ลบหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [InlineData(1111111, "หนึ่งล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [InlineData(11111111, "สิบเอ็ดล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [InlineData(111111111, "หนึ่งร้อยสิบเอ็ดล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [InlineData(1111111111, "หนึ่งพันหนึ่งร้อยสิบเอ็ดล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
