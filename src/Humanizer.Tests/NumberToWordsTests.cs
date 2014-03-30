using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class NumberToWordsTests
    {
        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(122, "one hundred and twenty-two")]
        [InlineData(3501, "three thousand five hundred and one")]
        [InlineData(100, "one hundred")]
        [InlineData(1000, "one thousand")]
        [InlineData(100000, "one hundred thousand")]
        [InlineData(1000000, "one million")]
        [InlineData(10000000, "ten million")]
        [InlineData(100000000, "one hundred million")]
        [InlineData(1000000000, "one billion")]
        [InlineData(111, "one hundred and eleven")]
        [InlineData(1111, "one thousand one hundred and eleven")]
        [InlineData(111111, "one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111, "one million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(11111111, "eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(111111111, "one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(1111111111, "one billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven")]
        [InlineData(123, "one hundred and twenty-three")]
        [InlineData(1234, "one thousand two hundred and thirty-four")]
        [InlineData(12345, "twelve thousand three hundred and forty-five")]
        [InlineData(123456, "one hundred and twenty-three thousand four hundred and fifty-six")]
        [InlineData(1234567, "one million two hundred and thirty-four thousand five hundred and sixty-seven")]
        [InlineData(12345678, "twelve million three hundred and forty-five thousand six hundred and seventy-eight")]
        [InlineData(123456789, "one hundred and twenty-three million four hundred and fifty-six thousand seven hundred and eighty-nine")]
        [InlineData(1234567890, "one billion two hundred and thirty-four million five hundred and sixty-seven thousand eight hundred and ninety")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }


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
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-ir");
            Assert.Equal(expected, number.ToWords());
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
        }
    }
}
