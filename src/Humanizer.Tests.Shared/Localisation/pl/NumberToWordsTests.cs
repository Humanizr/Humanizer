using Xunit;

namespace Humanizer.Tests.Localisation.pl
{
    [UseCulture("pl")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "jeden")]
        [InlineData(2, "dwa")]
        [InlineData(3, "trzy")]
        [InlineData(4, "cztery")]
        [InlineData(5, "pięć")]
        [InlineData(6, "sześć")]
        [InlineData(7, "siedem")]
        [InlineData(8, "osiem")]
        [InlineData(9, "dziewięć")]
        [InlineData(10, "dziesięć")]
        [InlineData(11, "jedenaście")]
        [InlineData(12, "dwanaście")]
        [InlineData(13, "trzynaście")]
        [InlineData(14, "czternaście")]
        [InlineData(15, "piętnaście")]
        [InlineData(16, "szesnaście")]
        [InlineData(17, "siedemnaście")]
        [InlineData(18, "osiemnaście")]
        [InlineData(19, "dziewiętnaście")]
        [InlineData(20, "dwadzieścia")]
        [InlineData(30, "trzydzieści")]
        [InlineData(40, "czterdzieści")]
        [InlineData(50, "pięćdziesiąt")]
        [InlineData(60, "sześćdziesiąt")]
        [InlineData(70, "siedemdziesiąt")]
        [InlineData(80, "osiemdziesiąt")]
        [InlineData(90, "dziewięćdziesiąt")]
        [InlineData(100, "sto")]
        [InlineData(112, "sto dwanaście")]
        [InlineData(128, "sto dwadzieścia osiem")]
        [InlineData(1000, "tysiąc")]
        [InlineData(2000, "dwa tysiące")]
        [InlineData(5000, "pięć tysięcy")]
        [InlineData(10000, "dziesięć tysięcy")]
        [InlineData(12000, "dwanaście tysięcy")]
        [InlineData(20000, "dwadzieścia tysięcy")]
        [InlineData(22000, "dwadzieścia dwa tysiące")]
        [InlineData(25000, "dwadzieścia pięć tysięcy")]
        [InlineData(31000, "trzydzieści jeden tysięcy")]
        [InlineData(34000, "trzydzieści cztery tysiące")]
        [InlineData(100000, "sto tysięcy")]
        [InlineData(500000, "pięćset tysięcy")]
        [InlineData(1000000, "milion")]
        [InlineData(2000000, "dwa miliony")]
        [InlineData(5000000, "pięć milionów")]
        [InlineData(1000000000, "miliard")]
        [InlineData(2000000000, "dwa miliardy")]
        [InlineData(1501001892, "miliard pięćset jeden milionów tysiąc osiemset dziewięćdziesiąt dwa")]
        [InlineData(2147483647, "dwa miliardy sto czterdzieści siedem milionów czterysta osiemdziesiąt trzy tysiące sześćset czterdzieści siedem")]
        [InlineData(-1501001892, "minus miliard pięćset jeden milionów tysiąc osiemset dziewięćdziesiąt dwa")]
        [InlineData(long.MaxValue, 
            "dziewięć trylionów " +
            "dwieście dwadzieścia trzy biliardy " +
            "trzysta siedemdziesiąt dwa biliony " + 
            "trzydzieści sześć miliardów " +
            "osiemset pięćdziesiąt cztery miliony " +
            "siedemset siedemdziesiąt pięć tysięcy " +
            "osiemset siedem")]
        [InlineData(long.MinValue, 
            "minus dziewięć trylionów " +
            "dwieście dwadzieścia trzy biliardy " +
            "trzysta siedemdziesiąt dwa biliony " + 
            "trzydzieści sześć miliardów " +
            "osiemset pięćdziesiąt cztery miliony " +
            "siedemset siedemdziesiąt pięć tysięcy " +
            "osiemset osiem")]
        public void ToWordsPolish(long number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(-1, "minus jeden", GrammaticalGender.Masculine)]
        [InlineData(-1, "minus jedna", GrammaticalGender.Feminine)]
        [InlineData(-1, "minus jedno", GrammaticalGender.Neuter)]
        [InlineData(-2, "minus dwa", GrammaticalGender.Masculine)]
        [InlineData(-2, "minus dwie", GrammaticalGender.Feminine)]
        [InlineData(-2, "minus dwa", GrammaticalGender.Neuter)]
        [InlineData(1, "jeden", GrammaticalGender.Masculine)]
        [InlineData(1, "jedna", GrammaticalGender.Feminine)]
        [InlineData(1, "jedno", GrammaticalGender.Neuter)]
        [InlineData(2, "dwa", GrammaticalGender.Masculine)]
        [InlineData(2, "dwie", GrammaticalGender.Feminine)]
        [InlineData(2, "dwa", GrammaticalGender.Neuter)]
        [InlineData(121, "sto dwadzieścia jeden", GrammaticalGender.Masculine)]
        [InlineData(121, "sto dwadzieścia jeden", GrammaticalGender.Feminine)]
        [InlineData(121, "sto dwadzieścia jeden", GrammaticalGender.Neuter)]
        [InlineData(122, "sto dwadzieścia dwa", GrammaticalGender.Masculine)]
        [InlineData(122, "sto dwadzieścia dwie", GrammaticalGender.Feminine)]
        [InlineData(122, "sto dwadzieścia dwa", GrammaticalGender.Neuter)]
        [InlineData(-2542, "minus dwa tysiące pięćset czterdzieści dwa", GrammaticalGender.Masculine)]
        [InlineData(-2542, "minus dwa tysiące pięćset czterdzieści dwie", GrammaticalGender.Feminine)]
        [InlineData(-2542, "minus dwa tysiące pięćset czterdzieści dwa", GrammaticalGender.Neuter)]
        [InlineData(1000001, "milion jeden", GrammaticalGender.Feminine)]
        [InlineData(-1000001, "minus milion jeden", GrammaticalGender.Feminine)]
        [InlineData(1000002, "milion dwa", GrammaticalGender.Masculine)]
        [InlineData(1000002, "milion dwie", GrammaticalGender.Feminine)]
        [InlineData(1000002, "milion dwa", GrammaticalGender.Neuter)]
        public void ToWordsPolishWithGender(int number, string expected, GrammaticalGender gender)
        {
            Assert.Equal(expected, number.ToWords(gender));
        }
    }
}
