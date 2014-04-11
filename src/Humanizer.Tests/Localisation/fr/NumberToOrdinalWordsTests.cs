using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fr
{
    public class NumberToOrdinalWordsTests : AmbientCulture
    {
        public NumberToOrdinalWordsTests() : base("fr-FR") { }

        [Theory]
        [InlineData(0, "zéroième")]
        [InlineData(1, "premier")]
        [InlineData(2, "deuxième")]
        [InlineData(3, "troisième")]
        [InlineData(4, "quatrième")]
        [InlineData(5, "cinquième")]
        [InlineData(6, "sixième")]
        [InlineData(7, "septième")]
        [InlineData(8, "huitième")]
        [InlineData(9, "neuvième")]
        [InlineData(10, "dixième")]
        [InlineData(11, "onzième")]
        [InlineData(12, "douzième")]
        [InlineData(13, "treizième")]
        [InlineData(14, "quatorzième")]
        [InlineData(15, "quinzième")]
        [InlineData(16, "seizième")]
        [InlineData(17, "dix-septième")]
        [InlineData(18, "dix-huitième")]
        [InlineData(19, "dix-neuvième")]
        [InlineData(20, "vingtième")]
        [InlineData(21, "vingt et unième")]
        [InlineData(22, "vingt-deuxième")]
        [InlineData(30, "trentième")]
        [InlineData(40, "quarantième")]
        [InlineData(50, "cinquantième")]
        [InlineData(60, "soixantième")]
        [InlineData(70, "soixante-dixième")]
        [InlineData(80, "quatre-vingtième")]
        [InlineData(90, "quatre-vingt-dixième")]
        [InlineData(95, "quatre-vingt-quinzième")]
        [InlineData(96, "quatre-vingt-seizième")]
        [InlineData(100, "centième")]
        [InlineData(120, "cent vingtième")]
        [InlineData(121, "cent vingt et unième")]
        [InlineData(1000, "millième")]
        [InlineData(1001, "mille unième")]
        [InlineData(1021, "mille vingt et unième")]
        [InlineData(10000, "dix millième")]
        [InlineData(10121, "dix mille cent vingt et unième")]
        [InlineData(100000, "cent millième")]
        [InlineData(1000000, "millionième")]
        [InlineData(1000000000, "milliardième")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
