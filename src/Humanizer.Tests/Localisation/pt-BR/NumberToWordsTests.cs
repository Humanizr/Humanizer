using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ptBR
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("pt-BR") { }

        [Theory]
        [InlineData(1, "um")]
        [InlineData(10, "dez")]
        [InlineData(11, "onze")]
        [InlineData(122, "cento e vinte e dois")]
        [InlineData(3501, "três mil quinhentos e um")]
        [InlineData(100, "cem")]
        [InlineData(1000, "mil")]
        [InlineData(100000, "cem mil")]
        [InlineData(1000000, "um milhão")]
        [InlineData(10000000, "dez milhões")]
        [InlineData(100000000, "cem milhões")]
        [InlineData(1000000000, "um bilhão")]
        [InlineData(111, "cento e onze")]
        [InlineData(1111, "mil cento e onze")]
        [InlineData(111111, "cento e onze mil cento e onze")]
        [InlineData(1111111, "um milhão cento e onze mil cento e onze")]
        [InlineData(11111111, "onze milhões cento e onze mil cento e onze")]
        [InlineData(111111111, "cento e onze milhões cento e onze mil cento e onze")]
        [InlineData(1111111111, "um bilhão cento e onze milhões cento e onze mil cento e onze")]
        [InlineData(123, "cento e vinte e três")]
        [InlineData(1234, "mil duzentos e trinta e quatro")]
        [InlineData(8100, "oito mil e cem")]
        [InlineData(12345, "doze mil trezentos e quarenta e cinco")]
        [InlineData(123456, "cento e vinte e três mil quatrocentos e cinquenta e seis")]
        [InlineData(1234567, "um milhão duzentos e trinta e quatro mil quinhentos e sessenta e sete")]
        [InlineData(12345678, "doze milhões trezentos e quarenta e cinco mil seiscentos e setenta e oito")]
        [InlineData(123456789, "cento e vinte e três milhões quatrocentos e cinquenta e seis mil setecentos e oitenta e nove")]
        [InlineData(1234567890, "um bilhão duzentos e trinta e quatro milhões quinhentos e sessenta e sete mil oitocentos e noventa")]
        [InlineData(1999, "mil novecentos e noventa e nove")]
        [InlineData(2014, "dois mil e quatorze")]
        [InlineData(2048, "dois mil e quarenta e oito")]
        public void ToWordsPortuguese(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
