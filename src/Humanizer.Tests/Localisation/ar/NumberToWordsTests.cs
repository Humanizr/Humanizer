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
    }
}
