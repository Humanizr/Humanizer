using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fa
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("fa") { }

        [InlineData(1, "یک")]
        [InlineData(10, "ده")]
        [InlineData(11, "یازده")]
        [InlineData(122, "صد و بیست و دو")]
        [InlineData(3501, "سه هزار و پانصد و یک")]
        [InlineData(100, "صد")]
        [InlineData(1000, "یک هزار")]
        [InlineData(100000, "صد هزار")]
        [InlineData(1000000, "یک میلیون")]
        [InlineData(10000000, "ده میلیون")]
        [InlineData(100000000, "صد میلیون")]
        [InlineData(1000000000, "یک میلیارد")]
        [InlineData(111, "صد و یازده")]
        [InlineData(1111, "یک هزار و صد و یازده")]
        [InlineData(111111, "صد و یازده هزار و صد و یازده")]
        [InlineData(1111111, "یک میلیون و صد و یازده هزار و صد و یازده")]
        [InlineData(11111111, "یازده میلیون و صد و یازده هزار و صد و یازده")]
        [InlineData(111111111, "صد و یازده میلیون و صد و یازده هزار و صد و یازده")]
        [InlineData(1111111111, "یک میلیارد و صد و یازده میلیون و صد و یازده هزار و صد و یازده")]
        [InlineData(123, "صد و بیست و سه")]
        [InlineData(1234, "یک هزار و دویست و سی و چهار")]
        [InlineData(12345, "دوازده هزار و سیصد و چهل و پنج")]
        [InlineData(123456, "صد و بیست و سه هزار و چهارصد و پنجاه و شش")]
        [InlineData(1234567, "یک میلیون و دویست و سی و چهار هزار و پانصد و شصت و هفت")]
        [InlineData(12345678, "دوازده میلیون و سیصد و چهل و پنج هزار و ششصد و هفتاد و هشت")]
        [InlineData(123456789, "صد و بیست و سه میلیون و چهارصد و پنجاه و شش هزار و هفتصد و هشتاد و نه")]
        [InlineData(1234567890, "یک میلیارد و دویست و سی و چهار میلیون و پانصد و شصت و هفت هزار و هشتصد و نود")]
        [Theory]
        public void ToWordsFarsi(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
