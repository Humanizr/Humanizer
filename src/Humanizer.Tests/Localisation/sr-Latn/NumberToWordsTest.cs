using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.sr_Latn
{
    public class NumberToWordsTest : AmbientCulture
    {
        public NumberToWordsTest() : base("sr-Latn") { }

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
            Assert.Equal(expected, number.ToWords(new CultureInfo("sr-Latn")));
        }

        [Theory]
        [InlineData(0, "нула")]
        [InlineData(1, "један")]
        [InlineData(2, "два")]
        [InlineData(3, "три")]
        [InlineData(4, "четири")]
        [InlineData(5, "пет")]
        [InlineData(6, "шест")]
        [InlineData(7, "седам")]
        [InlineData(8, "осам")]
        [InlineData(9, "девет")]
        [InlineData(10, "десет")]
        [InlineData(20, "двадесет")]
        [InlineData(30, "тридесет")]
        [InlineData(40, "четрдесет")]
        [InlineData(50, "петдесет")]
        [InlineData(60, "шестдесет")]
        [InlineData(70, "седамдесет")]
        [InlineData(80, "осамдесет")]
        [InlineData(90, "деветдесет")]
        [InlineData(100, "сто")]
        [InlineData(200, "двесто")]
        [InlineData(1000, "хиљаду")]
        [InlineData(10000, "десет хиљада")]
        [InlineData(100000, "сто хиљада")]
        [InlineData(1000000, "милион")]
        [InlineData(10000000, "десет милиона")]
        [InlineData(100000000, "сто милиона")]
        [InlineData(1000000000, "милијарда")]
        [InlineData(2000000000, "две милијарде")]
        [InlineData(15, "петнаест")]
        [InlineData(43, "четрдесет три")]
        [InlineData(81, "осамдесет један")]
        [InlineData(213, "двесто тринаест")]
        [InlineData(547, "петсто четрдесет седам")]
        public void ToWordsSr(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
