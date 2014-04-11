using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation
{
    public class NumerToWordsFactoryTests
    {
        [Theory]
        [InlineData("1000000000", 1000000000)]
        public void CanGetTwoLetterISOLanguageSpecificFactory(string notExpected, int number)
        {
            using (new AmbientCulture("ar"))
            {
                string result = number.ToWords();
                Assert.NotEqual(notExpected, result);
            }
        }

        [Theory]
        [InlineData("1000000000", 1000000000)]
        public void CanGetRFCStandardLanguageSpecificFactory(string notExpected, int number)
        {
            using (new AmbientCulture("pt-BR"))
            {
                string result = number.ToWords();
                Assert.NotEqual(notExpected, result);
            }
        }

        [Theory]
        [InlineData(1000000000)]
        public void CanGetCorrectRFCStandardLanguageSpecificFactory(int number)
        {
            string resultPtBR;
            using (new AmbientCulture("pt-BR"))
            {
                resultPtBR = number.ToWords();
            }

            string resultPtPT;
            using (new AmbientCulture("pt-PT"))
            {
                resultPtPT = number.ToWords();
            }

            Assert.NotEqual(resultPtBR, resultPtPT);
        }
    }
}