using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ar
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("ar") { }

        [Theory]
        [InlineData("صفر", 0)]
        [InlineData("واحد", 1)]
        [InlineData("اثنان", 2)]
        [InlineData("اثنان و عشرون", 22)]
        [InlineData("أحد عشر", 11)]
        [InlineData("ثلاثة آلاف و خمس مئة و واحد", 3501)]
        [InlineData("مليون و واحد", 1000001)]
        public void ToWordsArabic(string expected, int number)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "الصفر")]
        [InlineData(1, "الأول")]
        [InlineData(2, "الثاني")]
        [InlineData(3, "الثالث")]
        [InlineData(4, "الرابع")]
        [InlineData(5, "الخامس")]
        [InlineData(6, "السادس")]
        [InlineData(7, "السابع")]
        [InlineData(8, "الثامن")]
        [InlineData(9, "التاسع")]
        [InlineData(10, "العاشر")]
        [InlineData(11, "الحادي عشر")]
        [InlineData(12, "الثاني عشر")]
        [InlineData(13, "الثالث عشر")]
        [InlineData(14, "الرابع عشر")]
        [InlineData(15, "الخامس عشر")]
        [InlineData(16, "السادس عشر")]
        [InlineData(17, "السابع عشر")]
        [InlineData(18, "الثامن عشر")]
        [InlineData(19, "التاسع عشر")]
        [InlineData(20, "العشرون")]
        [InlineData(21, "الحادي و العشرون")]
        [InlineData(22, "الثاني و العشرون")]
        [InlineData(30, "الثلاثون")]
        [InlineData(40, "الأربعون")]
        [InlineData(50, "الخمسون")]
        [InlineData(60, "الستون")]
        [InlineData(70, "السبعون")]
        [InlineData(80, "الثمانون")]
        [InlineData(90, "التسعون")]
        [InlineData(95, "الخامس و التسعون")]
        [InlineData(96, "السادس و التسعون")]
        [InlineData(100, "المئة")]
        [InlineData(120, "العشرون بعد المئة")]
        [InlineData(121, "الحادي و العشرون بعد المئة")]
        [InlineData(200, "المئتان")]
        [InlineData(221, "الحادي و العشرون بعد المئتان")]
        [InlineData(300, "الثلاث مئة")]
        [InlineData(321, "الحادي و العشرون بعد الثلاث مئة")]
        [InlineData(327, "السابع و العشرون بعد الثلاث مئة")]
        [InlineData(1000, "الألف")]
        [InlineData(1001, "الأول بعد الألف")]
        [InlineData(1021, "الحادي و العشرون بعد الألف")]
        [InlineData(10000, "العشرة آلاف")]
        [InlineData(10121, "الحادي و العشرون بعد العشرة آلاف و مئة")]
        [InlineData(100000, "المئة ألف")]
        [InlineData(1000000, "المليون")]
        [InlineData(1020135, "الخامس و الثلاثون بعد المليون و عشرون ألفاً و مئة")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
