using Xunit;

namespace Humanizer.Tests.Localisation.tr
{
    [UseCulture("tr")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData("sıfır", 0)]
        [InlineData("bir", 1)]
        [InlineData("iki", 2)]
        [InlineData("on", 10)]
        [InlineData("yüz on iki", 112)]
        [InlineData("bin dört yüz kırk", 1440)]
        [InlineData("yirmi iki", 22)]
        [InlineData("on bir", 11)]
        [InlineData("üç bin beş yüz bir", 3501)]
        [InlineData("bir milyon bir", 1000001)]
        [InlineData("eksi bir milyon üç yüz kırk altı bin yedi yüz on bir", -1346711)]
        [InlineData("dokuz kentilyon iki yüz yirmi üç katrilyon üç yüz yetmiş iki trilyon otuz altı milyar sekiz yüz elli dört milyon yedi yüz yetmiş beş bin sekiz yüz yedi", 9223372036854775807)]
        public void ToWords(string expected, long number)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "sıfırıncı")]
        [InlineData(1, "birinci")]
        [InlineData(2, "ikinci")]
        [InlineData(3, "üçüncü")]
        [InlineData(4, "dördüncü")]
        [InlineData(5, "beşinci")]
        [InlineData(6, "altıncı")]
        [InlineData(7, "yedinci")]
        [InlineData(8, "sekizinci")]
        [InlineData(9, "dokuzuncu")]
        [InlineData(10, "onuncu")]
        [InlineData(11, "on birinci")]
        [InlineData(12, "on ikinci")]
        [InlineData(13, "on üçüncü")]
        [InlineData(14, "on dördüncü")]
        [InlineData(15, "on beşinci")]
        [InlineData(16, "on altıncı")]
        [InlineData(17, "on yedinci")]
        [InlineData(18, "on sekizinci")]
        [InlineData(19, "on dokuzuncu")]
        [InlineData(20, "yirminci")]
        [InlineData(21, "yirmi birinci")]
        [InlineData(30, "otuzuncu")]
        [InlineData(40, "kırkıncı")]
        [InlineData(50, "ellinci")]
        [InlineData(60, "altmışıncı")]
        [InlineData(70, "yetmişinci")]
        [InlineData(80, "sekseninci")]
        [InlineData(90, "doksanıncı")]
        [InlineData(100, "yüzüncü")]
        [InlineData(120, "yüz yirminci")]
        [InlineData(121, "yüz yirmi birinci")]
        [InlineData(200, "iki yüzüncü")]
        [InlineData(221, "iki yüz yirmi birinci")]
        [InlineData(300, "üç yüzüncü")]
        [InlineData(321, "üç yüz yirmi birinci")]
        [InlineData(1000, "bininci")]
        [InlineData(1001, "bin birinci")]
        [InlineData(10000, "on bininci")]
        [InlineData(100000, "yüz bininci")]
        [InlineData(1000000, "bir milyonuncu")]
        [InlineData(1022135, "bir milyon yirmi iki bin yüz otuz beşinci")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
