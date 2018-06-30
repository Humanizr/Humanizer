using Xunit;

namespace Humanizer.Tests.Localisation.zhCN
{
    [UseCulture("zh-CN")]
    public class NumberToWordsTests
    {
        [InlineData(1, "一")]
        [InlineData(2, "二")]
        [InlineData(3, "三")]
        [InlineData(4, "四")]
        [InlineData(-5, "负 五")]
        [InlineData(6, "六")]
        [InlineData(7, "七")]
        [InlineData(8, "八")]
        [InlineData(9, "九")]
        [InlineData(10, "十")]
        [InlineData(13, "十三")]
        [InlineData(15, "十五")]
        [InlineData(19, "十九")]
        [InlineData(28, "二十八")]
        [InlineData(37, "三十七")]
        [InlineData(46, "四十六")]
        [InlineData(55, "五十五")]
        [InlineData(64, "六十四")]
        [InlineData(73, "七十三")]
        [InlineData(82, "八十二")]
        [InlineData(-91, "负 九十一")]
        [InlineData(100, "一百")]
        [InlineData(507, "五百零七")]
        [InlineData(719, "七百一十九")]
        [InlineData(1356, "一千三百五十六")]
        [InlineData(20089, "二万零八十九")]
        [InlineData(335478, "三十三万五千四百七十八")]
        [InlineData(4214599, "四百二十一万四千五百九十九")]
        [InlineData(-54367865, "负 五千四百三十六万七千八百六十五")]
        [InlineData(650004076, "六亿五千万四千零七十六")]
        [InlineData(7156404367L, "七十一亿五千六百四十万四千三百六十七")]
        [InlineData(89043267890L, "八百九十亿四千三百二十六万七千八百九十")]
        [InlineData(500007893401L, "五千亿零七百八十九万三千四百零一")]
        [InlineData(500000003401L, "五千亿零三千四百零一")]
        [InlineData(500000000001L, "五千亿零一")]
        [InlineData(500000000000L, "五千亿")]
        [InlineData(6067823149088L, "六兆零六百七十八亿二千三百一十四万九千零八十八")]
        [Theory]
        public void ToWords(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1, "第 一")]
        [InlineData(15, "第 十五")]
        [InlineData(10000, "第 一万")]
        [InlineData(31234, "第 三万一千二百三十四")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
