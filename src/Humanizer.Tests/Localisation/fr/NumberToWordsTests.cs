using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.fr
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("fr-FR") { }

        [Theory]
        [InlineData(0, "zéro")]
        [InlineData(1, "un")]
        [InlineData(10, "dix")]
        [InlineData(11, "onze")]
        [InlineData(15, "quinze")]
        [InlineData(17, "dix-sept")]
        [InlineData(25, "vingt-cinq")]
        [InlineData(31, "trente et un")]
        [InlineData(71, "soixante et onze")]
        [InlineData(81, "quatre-vingt-un")]
        [InlineData(122, "cent vingt-deux")]
        [InlineData(3501, "trois mille cinq cent un")]
        [InlineData(100, "cent")]
        [InlineData(1000, "mille")]
        [InlineData(100000, "cent mille")]
        [InlineData(1000000, "un million")]
        [InlineData(10000000, "dix millions")]
        [InlineData(100000000, "cent millions")]
        [InlineData(1000000000, "un milliard")]
        [InlineData(111, "cent onze")]
        [InlineData(1111, "mille cent onze")]
        [InlineData(111111, "cent onze mille cent onze")]
        [InlineData(1111111, "un million cent onze mille cent onze")]
        [InlineData(11111111, "onze millions cent onze mille cent onze")]
        [InlineData(111111111, "cent onze millions cent onze mille cent onze")]
        [InlineData(1111111111, "un milliard cent onze millions cent onze mille cent onze")]
        [InlineData(123, "cent vingt-trois")]
        [InlineData(1234, "mille deux cent trente-quatre")]
        [InlineData(12345, "douze mille trois cent quarante-cinq")]
        [InlineData(123456, "cent vingt-trois mille quatre cent cinquante-six")]
        [InlineData(1234567, "un million deux cent trente-quatre mille cinq cent soixante-sept")]
        [InlineData(12345678, "douze millions trois cent quarante-cinq mille six cent soixante-dix-huit")]
        [InlineData(123456789, "cent vingt-trois millions quatre cent cinquante-six mille sept cent quatre-vingt-neuf")]
        [InlineData(1234567890, "un milliard deux cent trente-quatre millions cinq cent soixante-sept mille huit cent quatre-vingt-dix")]
        [InlineData(1234567899, "un milliard deux cent trente-quatre millions cinq cent soixante-sept mille huit cent quatre-vingt-dix-neuf")]
        [InlineData(223, "deux cent vingt-trois")]
        [InlineData(2234, "deux mille deux cent trente-quatre")]
        [InlineData(22345, "vingt-deux mille trois cent quarante-cinq")]
        [InlineData(223456, "deux cent vingt-trois mille quatre cent cinquante-six")]
        [InlineData(2234567, "deux millions deux cent trente-quatre mille cinq cent soixante-sept")]
        [InlineData(22345678, "vingt-deux millions trois cent quarante-cinq mille six cent soixante-dix-huit")]
        [InlineData(223456789, "deux cent vingt-trois millions quatre cent cinquante-six mille sept cent quatre-vingt-neuf")]
        [InlineData(2147483646, "deux milliards cent quarante-sept millions quatre cent quatre-vingt-trois mille six cent quarante-six")]
        [InlineData(1999, "mille neuf cent quatre-vingt-dix-neuf")]
        [InlineData(2014, "deux mille quatorze")]
        [InlineData(2048, "deux mille quarante-huit")]
        public void ToWordsFrench(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
