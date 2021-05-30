using Xunit;

namespace Humanizer.Tests.Localisation.az
{
    [UseCulture("az")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData("sıfır", 0)]
        [InlineData("bir", 1)]
        [InlineData("iki", 2)]
        [InlineData("on", 10)]
        [InlineData("yüz on iki", 112)]
        [InlineData("min dörd yüz qırx", 1440)]
        [InlineData("iyirmi iki", 22)]
        [InlineData("on bir", 11)]
        [InlineData("üç min beş yüz bir", 3501)]
        [InlineData("bir milyon bir", 1000001)]
        [InlineData("mənfi bir milyon üç yüz qırx altı min yeddi yüz on bir", -1346711)]
        public void ToWords(string expected, int number)
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
        [InlineData(7, "yeddinci")]
        [InlineData(8, "səkkizinci")]
        [InlineData(9, "doqquzuncu")]
        [InlineData(10, "onuncu")]
        [InlineData(11, "on birinci")]
        [InlineData(12, "on ikinci")]
        [InlineData(13, "on üçüncü")]
        [InlineData(14, "on dördüncü")]
        [InlineData(15, "on beşinci")]
        [InlineData(16, "on altıncı")]
        [InlineData(17, "on yeddinci")]
        [InlineData(18, "on səkkizinci")]
        [InlineData(19, "on doqquzuncu")]
        [InlineData(20, "iyirminci")]
        [InlineData(21, "iyirmi birinci")]
        [InlineData(30, "otuzuncu")]
        [InlineData(40, "qırxıncı")]
        [InlineData(50, "əllinci")]
        [InlineData(60, "altmışıncı")]
        [InlineData(70, "yetmişinci")]
        [InlineData(80, "səksəninci")]
        [InlineData(90, "doxsanıncı")]
        [InlineData(100, "yüzüncü")]
        [InlineData(120, "yüz iyirminci")]
        [InlineData(121, "yüz iyirmi birinci")]
        [InlineData(200, "iki yüzüncü")]
        [InlineData(221, "iki yüz iyirmi birinci")]
        [InlineData(300, "üç yüzüncü")]
        [InlineData(321, "üç yüz iyirmi birinci")]
        [InlineData(1000, "mininci")]
        [InlineData(1001, "min birinci")]
        [InlineData(10000, "on mininci")]
        [InlineData(100000, "yüz mininci")]
        [InlineData(1000000, "bir milyonuncu")]
        [InlineData(1022135, "bir milyon iyirmi iki min yüz otuz beşinci")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
