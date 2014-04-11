using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class NumerToWordsFactoryTests
    {
        [Fact]
        public void CanGetRFCStandardLanguageSpecificFactory()
        {

            using (new AmbientCulture("pt-BR"))
            {
                string retorno = 1000000000.ToWords();
                Assert.NotEqual("1000000000",retorno);
            }
        }

        [Fact]
        public void CanGetTwoLetterISOLanguageSpecificFactory()
        {

            using (new AmbientCulture("ar"))
            {
                string retorno = 1000000000.ToWords();
                Assert.NotEqual("1000000000", retorno);
            }
        }

        [Fact]
        public void CanGetTwoLetterISOLanguageSpecificFactory()
        {

            using (new AmbientCulture("ar"))
            {
                string retorno = 1000000000.ToWords();
                Assert.NotEqual("1000000000", retorno);
            }
        }
    }
}
