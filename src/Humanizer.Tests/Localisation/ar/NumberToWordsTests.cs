using Xunit;

namespace Humanizer.Tests.Localisation.ar
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("ar") { }

        [Fact]
        public void ToWords()
        {
            Assert.Equal("صفر", 0.ToWords());
            Assert.Equal("واحد", 1.ToWords());
            Assert.Equal("اثنان", 2.ToWords());
            Assert.Equal("اثنان و عشرون", 22.ToWords());
            Assert.Equal("أحد عشر", 11.ToWords());
            Assert.Equal("مائة و اثنان و عشرون", 122.ToWords());
            Assert.Equal("ثلاثة آلاف و خمسمائة و واحد", 3501.ToWords());
            Assert.Equal("سبعة و ثلاثون", 37.ToWords());
            Assert.Equal("ثلاثة عشر", 13.ToWords());
            Assert.Equal("مئتان و خمسون", 250.ToWords());
            Assert.Equal("سبعة و ثلاثون ألفاً و تسعمائة و واحد و ثمانون", 37981.ToWords());
            Assert.Equal("مليون", 1000000.ToWords());
        }
    }
}
