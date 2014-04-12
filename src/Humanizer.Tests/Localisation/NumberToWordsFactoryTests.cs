using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class NumberToWordsFactoryTests
    {
        [Fact]
        public void CanGetTwoLetterIsoLanguageSpecificFactory()
        {
            using (new AmbientCulture("ar"))
            {
                string result = 1000000000.ToWords();
                Assert.NotEqual("1000000000", result);
            }
        }

        [Fact]
        public void CanGetRfcStandardLanguageSpecificFactory()
        {
            using (new AmbientCulture("pt-BR"))
            {
                string result = 1000000000.ToWords();
                Assert.NotEqual("1000000000", result);
            }
        }

        [Fact]
        public void CanGetCorrectRfcStandardLanguageSpecificFactory()
        {
            string resultPtBr;
            const int number = 1000000000;
            using (new AmbientCulture("pt-BR"))
            {
                resultPtBr = number.ToWords();
            }

            string resultPtPt;
            using (new AmbientCulture("pt-PT"))
            {
                resultPtPt = number.ToWords();
            }

            Assert.NotEqual(resultPtBr, resultPtPt);
        }
    }
}
