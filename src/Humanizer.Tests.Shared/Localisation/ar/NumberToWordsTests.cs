using Xunit;

namespace Humanizer.Tests.Localisation.ar
{
    [UseCulture("ar")]
    public class NumberToWordsTests
    {
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
        [InlineData("ناقص واحد", -1)]
        [InlineData("ناقص اثنان", -2)]
        [InlineData("ناقص اثنان و عشرون", -22)]
        [InlineData("ناقص أحد عشر", -11)]
        [InlineData("ناقص ثلاثة آلاف و خمس مئة و واحد", -3501)]
        [InlineData("ناقص مليون و واحد", -1000001)]
        public void ToWordsArabicNegative(string expected, int number)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(1L, "واحد")]
        [InlineData(11L, "أحد عشر")]
        [InlineData(111L, "مئة و أحد عشر")]
        [InlineData(1111L, "ألف و مئة و أحد عشر")]
        [InlineData(11111L, "أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(111111L, "مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(1111111L, "مليون و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(11111111L, "أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(111111111L, "مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(1111111111L, "مليار و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(11111111111L, "أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(111111111111L, "مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(1111111111111L, "تريليون و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(11111111111111L, "أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(111111111111111L, "مئة و أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(1111111111111111L, "كوادريليون و مئة و أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(11111111111111111L, "أحد عشر كوادريليوناً و مئة و أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(111111111111111111L, "مئة و أحد عشر كوادريليوناً و مئة و أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(1111111111111111111L, "كوينتليون و مئة و أحد عشر كوادريليوناً و مئة و أحد عشر تريليوناً و مئة و أحد عشر ملياراً و مئة و أحد عشر مليوناً و مئة و أحد عشر ألفاً و مئة و أحد عشر")]
        [InlineData(10000000001L, "عشرة مليارات و واحد")]
        [InlineData(8750000500001L, "ثمانية تريليونات و سبع مئة و خمسون ملياراً و خمس مئة ألفاً و واحد")]
        [InlineData(-10000000001L, "ناقص عشرة مليارات و واحد")]
        [InlineData(-8750000500001L, "ناقص ثمانية تريليونات و سبع مئة و خمسون ملياراً و خمس مئة ألفاً و واحد")]
        public void ToWordsArabicLong(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData("صفر", 0)]
        [InlineData("واحدة", 1)]
        [InlineData("اثنتان", 2)]
        [InlineData("اثنتان و عشرون", 22)]
        [InlineData("إحدى عشرة", 11)]
        [InlineData("ثلاثة آلاف و خمس مئة و واحدة", 3501)]
        [InlineData("مليون و واحدة", 1000001)]
        public void ToWordsArabicFeminine(string expected, long number)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData("عشرة مليارات و واحدة", 10000000001)]
        [InlineData("ثمانية تريليونات و سبع مئة و خمسون ملياراً و خمس مئة ألفاً و واحدة", 8750000500001)]
        public void ToWordsArabicLongFeminine(string expected, long number)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(122, "مئة و اثنتان و عشرون", GrammaticalGender.Feminine)]
        [InlineData(3501, "ثلاثة آلاف و خمس مئة و واحدة", GrammaticalGender.Feminine)]
        [InlineData(3501, "ثلاثة آلاف و خمس مئة و واحد", GrammaticalGender.Neuter)]
        public void ToWordsWithGender(long number, string expected, GrammaticalGender gender)
        {
            Assert.Equal(expected, number.ToWords(gender));
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

        [Theory]
        [InlineData(0, "الصفر")]
        [InlineData(1, "الأولى")]
        [InlineData(2, "الثانية")]
        [InlineData(3, "الثالثة")]
        [InlineData(4, "الرابعة")]
        [InlineData(5, "الخامسة")]
        [InlineData(6, "السادسة")]
        [InlineData(7, "السابعة")]
        [InlineData(8, "الثامنة")]
        [InlineData(9, "التاسعة")]
        [InlineData(10, "العاشرة")]
        [InlineData(11, "الحادية عشرة")]
        [InlineData(12, "الثانية عشرة")]
        [InlineData(13, "الثالثة عشرة")]
        [InlineData(14, "الرابعة عشرة")]
        [InlineData(15, "الخامسة عشرة")]
        [InlineData(16, "السادسة عشرة")]
        [InlineData(17, "السابعة عشرة")]
        [InlineData(18, "الثامنة عشرة")]
        [InlineData(19, "التاسعة عشرة")]
        [InlineData(20, "العشرون")]
        [InlineData(21, "الحادية و العشرون")]
        [InlineData(22, "الثانية و العشرون")]
        [InlineData(30, "الثلاثون")]
        [InlineData(40, "الأربعون")]
        [InlineData(50, "الخمسون")]
        [InlineData(60, "الستون")]
        [InlineData(70, "السبعون")]
        [InlineData(80, "الثمانون")]
        [InlineData(90, "التسعون")]
        [InlineData(95, "الخامسة و التسعون")]
        [InlineData(96, "السادسة و التسعون")]
        [InlineData(100, "المئة")]
        [InlineData(120, "العشرون بعد المئة")]
        [InlineData(121, "الحادية و العشرون بعد المئة")]
        [InlineData(200, "المئتان")]
        [InlineData(221, "الحادية و العشرون بعد المئتان")]
        [InlineData(300, "الثلاث مئة")]
        [InlineData(321, "الحادية و العشرون بعد الثلاث مئة")]
        [InlineData(327, "السابعة و العشرون بعد الثلاث مئة")]
        [InlineData(1000, "الألف")]
        [InlineData(1001, "الأولى بعد الألف")]
        [InlineData(1021, "الحادية و العشرون بعد الألف")]
        [InlineData(10000, "العشرة آلاف")]
        [InlineData(10121, "الحادية و العشرون بعد العشرة آلاف و مئة")]
        [InlineData(100000, "المئة ألف")]
        [InlineData(1000000, "المليون")]
        [InlineData(1020135, "الخامسة و الثلاثون بعد المليون و عشرون ألفاً و مئة")]
        public void ToOrdinalWordsWithFeminineGender(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords(GrammaticalGender.Feminine));
        }
    }
}
