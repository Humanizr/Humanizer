using Xunit;

namespace Humanizer.Tests.Localisation.sr
{
    [UseCulture("sr")]
    public class NumberToWordsTest
    {

        [Theory]
        [InlineData(0, "нула")]
        [InlineData(1, "један")]
        [InlineData(2, "два")]
        [InlineData(3, "три")]
        [InlineData(4, "четири")]
        [InlineData(5, "пет")]
        [InlineData(6, "шест")]
        [InlineData(7, "седам")]
        [InlineData(8, "осам")]
        [InlineData(9, "девет")]
        [InlineData(10, "десет")]
        [InlineData(20, "двадесет")]
        [InlineData(30, "тридесет")]
        [InlineData(40, "четрдесет")]
        [InlineData(50, "петдесет")]
        [InlineData(60, "шестдесет")]
        [InlineData(70, "седамдесет")]
        [InlineData(80, "осамдесет")]
        [InlineData(90, "деветдесет")]
        [InlineData(100, "сто")]
        [InlineData(200, "двесто")]
        [InlineData(1000, "хиљаду")]
        [InlineData(10000, "десет хиљада")]
        [InlineData(100000, "сто хиљада")]
        [InlineData(1000000, "милион")]
        [InlineData(10000000, "десет милиона")]
        [InlineData(100000000, "сто милиона")]
        [InlineData(1000000000, "милијарда")]
        [InlineData(2000000000, "две милијарде")]
        [InlineData(15, "петнаест")]
        [InlineData(43, "четрдесет три")]
        [InlineData(81, "осамдесет један")]
        [InlineData(213, "двесто тринаест")]
        [InlineData(547, "петсто четрдесет седам")]
        public void ToWordsSr(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
