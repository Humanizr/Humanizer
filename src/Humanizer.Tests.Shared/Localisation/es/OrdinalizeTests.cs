using System.Globalization;

using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-ES")]
    public class OrdinalizeTests
    {
        [Theory]
        [InlineData(1, "1.º")]
        [InlineData(3, "3.º")]
        public void OrdinalizeDefaultGender(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(-1, "1.º")]
        [InlineData(int.MinValue, "0")]
        public void OrdinalizeZeroOrNegativeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(1, WordForm.Abbreviation, "1.er")]
        [InlineData(1, WordForm.Normal, "1.º")]
        [InlineData(2, WordForm.Abbreviation, "2.º")]
        [InlineData(2, WordForm.Normal, "2.º")]
        [InlineData(3, WordForm.Abbreviation, "3.er")]
        [InlineData(3, WordForm.Normal, "3.º")]
        [InlineData(21, WordForm.Abbreviation, "21.er")]
        [InlineData(21, WordForm.Normal, "21.º")]
        public void OrdinalizeWithWordForm(int number, WordForm wordForm, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(wordForm));
            Assert.Equal(expected, number.ToString(CultureInfo.CurrentUICulture).Ordinalize(wordForm));
        }

        [Theory]
        [InlineData(1, GrammaticalGender.Masculine, WordForm.Abbreviation, "1.er")]
        [InlineData(1, GrammaticalGender.Masculine, WordForm.Normal, "1.º")]
        [InlineData(1, GrammaticalGender.Feminine, WordForm.Abbreviation, "1.ª")]
        [InlineData(1, GrammaticalGender.Feminine, WordForm.Normal, "1.ª")]
        [InlineData(1, GrammaticalGender.Neuter, WordForm.Abbreviation, "1.er")]
        [InlineData(1, GrammaticalGender.Neuter, WordForm.Normal, "1.º")]
        public void OrdinalizeWithWordFormAndGender(int number, GrammaticalGender gender, WordForm wordForm, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(gender, wordForm));
            Assert.Equal(expected, number.ToString(CultureInfo.CurrentUICulture).Ordinalize(gender, wordForm));
        }

        [Theory]
        [InlineData("1", "1.º")]
        [InlineData("2", "2.º")]
        [InlineData("3", "3.º")]
        [InlineData("4", "4.º")]
        [InlineData("5", "5.º")]
        [InlineData("6", "6.º")]
        [InlineData("23", "23.º")]
        [InlineData("100", "100.º")]
        [InlineData("101", "101.º")]
        [InlineData("102", "102.º")]
        [InlineData("103", "103.º")]
        [InlineData("1001", "1001.º")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData("0", "0")]
        [InlineData("1", "1.ª")]
        [InlineData("2", "2.ª")]
        [InlineData("3", "3.ª")]
        [InlineData("4", "4.ª")]
        [InlineData("5", "5.ª")]
        [InlineData("6", "6.ª")]
        [InlineData("23", "23.ª")]
        [InlineData("100", "100.ª")]
        [InlineData("101", "101.ª")]
        [InlineData("102", "102.ª")]
        [InlineData("103", "103.ª")]
        [InlineData("1001", "1001.ª")]
        public void OrdinalizeStringFeminine(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1.º")]
        [InlineData(2, "2.º")]
        [InlineData(3, "3.º")]
        [InlineData(4, "4.º")]
        [InlineData(5, "5.º")]
        [InlineData(6, "6.º")]
        [InlineData(10, "10.º")]
        [InlineData(23, "23.º")]
        [InlineData(100, "100.º")]
        [InlineData(101, "101.º")]
        [InlineData(102, "102.º")]
        [InlineData(103, "103.º")]
        [InlineData(1001, "1001.º")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Masculine), ordinalized);
        }

        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1.ª")]
        [InlineData(2, "2.ª")]
        [InlineData(3, "3.ª")]
        [InlineData(4, "4.ª")]
        [InlineData(5, "5.ª")]
        [InlineData(6, "6.ª")]
        [InlineData(10, "10.ª")]
        [InlineData(23, "23.ª")]
        [InlineData(100, "100.ª")]
        [InlineData(101, "101.ª")]
        [InlineData(102, "102.ª")]
        [InlineData(103, "103.ª")]
        [InlineData(1001, "1001.ª")]
        public void OrdinalizeNumberFeminine(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(GrammaticalGender.Feminine), ordinalized);
        }
    }
}
