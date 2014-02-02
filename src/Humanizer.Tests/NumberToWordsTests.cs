using Xunit;

namespace Humanizer.Tests
{
    public class NumberToWordsTests
    {
        [Fact]
        public void ToWords()
        {
            Assert.Equal("one", 1.ToWords());
            Assert.Equal("ten", 10.ToWords());
            Assert.Equal("eleven", 11.ToWords());
            Assert.Equal("one hundred and twenty-two", 122.ToWords());
            Assert.Equal("three thousand five hundred and one", 3501.ToWords());
        }

        [Fact]
        public void RoundNumbersHaveNoSpaceAtTheEnd()
        {
            Assert.Equal("one hundred", 100.ToWords());
            Assert.Equal("one thousand", 1000.ToWords());
            Assert.Equal("one hundred thousand", 100000.ToWords());
            Assert.Equal("one million", 1000000.ToWords());
        }

        [Fact]
        public void ToWodrsArabic()
        {
            Assert.Equal("صفر", 0.ToArabicWords());
            Assert.Equal("واحد", 1.ToArabicWords());
            Assert.Equal("اثنان", 2.ToArabicWords());
            Assert.Equal("اثنان و عشرون", 22.ToArabicWords());
            Assert.Equal("أحد عشر", 11.ToArabicWords());
            Assert.Equal("مائة و اثنان و عشرون", 122.ToArabicWords());
            Assert.Equal("ثلاثة آلاف و خمسمائة و واحد", 3501.ToArabicWords());
            Assert.Equal("سبعة و ثلاثون", 37.ToArabicWords());
            Assert.Equal("ثلاثة عشر", 13.ToArabicWords());
            Assert.Equal("مئتان و خمسون", 250.ToArabicWords());
            Assert.Equal("سبعة و ثلاثون ألفاً و تسعمائة و واحد و ثمانون", 37981.ToArabicWords());

        }
    }
}
