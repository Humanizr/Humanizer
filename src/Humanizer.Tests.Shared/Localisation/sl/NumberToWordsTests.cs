using Xunit;

namespace Humanizer.Tests.Localisation.sl
{
    [UseCulture("sl-SI")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(0, "nič")]
        [InlineData(1, "ena")]
        [InlineData(2, "dva")]
        [InlineData(3, "tri")]
        [InlineData(4, "štiri")]
        [InlineData(5, "pet")]
        [InlineData(6, "šest")]
        [InlineData(7, "sedem")]
        [InlineData(8, "osem")]
        [InlineData(9, "devet")]
        [InlineData(10, "deset")]
        [InlineData(20, "dvajset")]
        [InlineData(30, "trideset")]
        [InlineData(40, "štirideset")]
        [InlineData(50, "petdeset")]
        [InlineData(60, "šestdeset")]
        [InlineData(70, "sedemdeset")]
        [InlineData(80, "osemdeset")]
        [InlineData(90, "devetdeset")]
        [InlineData(100, "sto")]
        [InlineData(200, "dvesto")]
        [InlineData(1000, "tisoč")]
        [InlineData(10000, "deset tisoč")]
        [InlineData(100000, "sto tisoč")]
        [InlineData(1000000, "milijon")]
        [InlineData(10000000, "deset milijonov")]
        [InlineData(100000000, "sto milijonov")]
        [InlineData(1000000000, "milijarda")]
        [InlineData(2000000000, "dve milijardi")]
        [InlineData(122, "sto dvaindvajset")]
        [InlineData(3501, "tri tisoč petsto ena")]
        [InlineData(111, "sto enajst")]
        [InlineData(1112, "tisoč sto dvanajst")]
        [InlineData(11213, "enajst tisoč dvesto trinajst")]
        [InlineData(121314, "sto enaindvajset tisoč tristo štirinajst")]
        [InlineData(2132415, "dva milijona sto dvaintrideset tisoč štiristo petnajst")]
        [InlineData(12345516, "dvanajst milijonov tristo petinštirideset tisoč petsto šestnajst")]
        [InlineData(751633617, "sedemsto enainpetdeset milijonov šeststo triintrideset tisoč šeststo sedemnajst")]
        [InlineData(1111111118, "milijarda sto enajst milijonov sto enajst tisoč sto osemnajst")]
        [InlineData(-751633619, "minus sedemsto enainpetdeset milijonov šeststo triintrideset tisoč šeststo devetnajst")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
