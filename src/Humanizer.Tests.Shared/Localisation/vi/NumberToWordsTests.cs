using Xunit;

namespace Humanizer.Tests.vi
{
    [UseCulture("vi")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "không")]
        [InlineData(1, "một")]
        [InlineData(2, "hai")]
        [InlineData(3, "ba")]
        [InlineData(4, "bốn")]
        [InlineData(5, "năm")]
        [InlineData(6, "sáu")]
        [InlineData(7, "bảy")]
        [InlineData(8, "tám")]
        [InlineData(9, "chín")]
        [InlineData(10, "mười")]
        [InlineData(11, "mười một")]
        [InlineData(14, "mười bốn")]
        [InlineData(15, "mười lăm")]
        [InlineData(21, "hai mươi mốt")]
        [InlineData(24, "hai mươi tư")]
        [InlineData(25, "hai mươi lăm")]
        [InlineData(50, "năm mươi")]
        [InlineData(55, "năm mươi lăm")]
        [InlineData(100, "một trăm")]
        [InlineData(105, "một trăm linh năm")]
        [InlineData(110, "một trăm mười")]
        [InlineData(114, "một trăm mười bốn")]
        [InlineData(115, "một trăm mười lăm")]
        [InlineData(134, "một trăm ba mươi tư")]
        [InlineData(500, "năm trăm")]
        [InlineData(505, "năm trăm linh năm")]
        [InlineData(555, "năm trăm năm mươi lăm")]
        [InlineData(1000, "một nghìn")]
        [InlineData(1005, "một nghìn linh năm")]
        [InlineData(1115, "một nghìn một trăm mười lăm")]
        [InlineData(10005, "mười nghìn linh năm")]
        [InlineData(10115, "mười nghìn một trăm mười lăm")]
        [InlineData(11115, "mười một nghìn một trăm mười lăm")]
        [InlineData(30005, "ba mươi nghìn linh năm")]
        [InlineData(100005, "một trăm nghìn linh năm")]
        [InlineData(1000000, "một triệu")]
        [InlineData(100001005, "một trăm triệu một nghìn linh năm")]
        [InlineData(1000000000, "một tỉ")]
        [InlineData(1111111111111111, "một triệu một trăm mười một nghìn một trăm mười một tỉ một trăm mười một triệu một trăm mười một nghìn một trăm mười một")]
        [InlineData(5101101101101151101, "năm tỉ một trăm linh một triệu một trăm linh một nghìn một trăm linh một tỉ một trăm linh một triệu một trăm năm mươi mốt nghìn một trăm linh một")]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "thứ không")]
        [InlineData(1, "thứ nhất")]
        [InlineData(2, "thứ nhì")]
        [InlineData(3, "thứ ba")]
        [InlineData(4, "thứ tư")]
        [InlineData(5, "thứ năm")]
        [InlineData(6, "thứ sáu")]
        [InlineData(7, "thứ bảy")]
        [InlineData(8, "thứ tám")]
        [InlineData(9, "thứ chín")]
        [InlineData(10, "thứ mười")]
        [InlineData(11, "thứ mười một")]
        [InlineData(14, "thứ mười bốn")]
        [InlineData(15, "thứ mười lăm")]
        [InlineData(21, "thứ hai mươi mốt")]
        [InlineData(24, "thứ hai mươi tư")]
        [InlineData(25, "thứ hai mươi lăm")]
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }
    }
}
