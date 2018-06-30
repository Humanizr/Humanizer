using Xunit;

namespace Humanizer.Tests.Localisation.sr_Latn
{
    [UseCulture("sr-Latn")]
    public class NumberToWordsTest
    {

        [Theory]
        [InlineData(0, "nula")]
        [InlineData(1, "jedan")]
        [InlineData(2, "dva")]
        [InlineData(3, "tri")]
        [InlineData(4, "četiri")]
        [InlineData(5, "pet")]
        [InlineData(6, "šest")]
        [InlineData(7, "sedam")]
        [InlineData(8, "osam")]
        [InlineData(9, "devet")]
        [InlineData(10, "deset")]
        [InlineData(20, "dvadeset")]
        [InlineData(30, "trideset")]
        [InlineData(40, "četrdeset")]
        [InlineData(50, "petdeset")]
        [InlineData(60, "šestdeset")]
        [InlineData(70, "sedamdeset")]
        [InlineData(80, "osamdeset")]
        [InlineData(90, "devetdeset")]
        [InlineData(100, "sto")]
        [InlineData(200, "dvesto")]
        [InlineData(1000, "hiljadu")]
        [InlineData(10000, "deset hiljada")]
        [InlineData(100000, "sto hiljada")]
        [InlineData(1000000, "milion")]
        [InlineData(10000000, "deset miliona")]
        [InlineData(100000000, "sto miliona")]
        [InlineData(1000000000, "milijarda")]
        [InlineData(2000000000, "dve milijarde")]
        [InlineData(15, "petnaest")]
        [InlineData(43, "četrdeset tri")]
        [InlineData(81, "osamdeset jedan")]
        [InlineData(213, "dvesto trinaest")]
        [InlineData(547, "petsto četrdeset sedam")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
