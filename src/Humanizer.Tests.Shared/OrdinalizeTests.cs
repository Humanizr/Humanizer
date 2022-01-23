using System.Globalization;

using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en-US")]
    public class OrdinalizeTests
    {
        [Theory]
        [InlineData("0", "0th")]
        [InlineData("1", "1st")]
        [InlineData("2", "2nd")]
        [InlineData("3", "3rd")]
        [InlineData("4", "4th")]
        [InlineData("5", "5th")]
        [InlineData("6", "6th")]
        [InlineData("7", "7th")]
        [InlineData("8", "8th")]
        [InlineData("9", "9th")]
        [InlineData("10", "10th")]
        [InlineData("11", "11th")]
        [InlineData("12", "12th")]
        [InlineData("13", "13th")]
        [InlineData("14", "14th")]
        [InlineData("20", "20th")]
        [InlineData("21", "21st")]
        [InlineData("22", "22nd")]
        [InlineData("23", "23rd")]
        [InlineData("24", "24th")]
        [InlineData("100", "100th")]
        [InlineData("101", "101st")]
        [InlineData("102", "102nd")]
        [InlineData("103", "103rd")]
        [InlineData("104", "104th")]
        [InlineData("110", "110th")]
        [InlineData("1000", "1000th")]
        [InlineData("1001", "1001st")]
        public void OrdinalizeString(string number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(0, "0th")]
        [InlineData(1, "1st")]
        [InlineData(2, "2nd")]
        [InlineData(3, "3rd")]
        [InlineData(4, "4th")]
        [InlineData(5, "5th")]
        [InlineData(6, "6th")]
        [InlineData(7, "7th")]
        [InlineData(8, "8th")]
        [InlineData(9, "9th")]
        [InlineData(10, "10th")]
        [InlineData(11, "11th")]
        [InlineData(12, "12th")]
        [InlineData(13, "13th")]
        [InlineData(14, "14th")]
        [InlineData(20, "20th")]
        [InlineData(21, "21st")]
        [InlineData(22, "22nd")]
        [InlineData(23, "23rd")]
        [InlineData(24, "24th")]
        [InlineData(100, "100th")]
        [InlineData(101, "101st")]
        [InlineData(102, "102nd")]
        [InlineData(103, "103rd")]
        [InlineData(104, "104th")]
        [InlineData(110, "110th")]
        [InlineData(1000, "1000th")]
        [InlineData(1001, "1001st")]
        public void OrdinalizeNumber(int number, string ordinalized)
        {
            Assert.Equal(number.Ordinalize(), ordinalized);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(8)]
        public void OrdinalizeNumberGenderIsImmaterial(int number)
        {
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("8")]
        public void OrdinalizeStringGenderIsImmaterial(string number)
        {
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }

        [Theory]
        [InlineData("en-US", "1", "1st")]
        [InlineData("nl-NL", "1", "1e")]
        public void OrdinalizeStringWithCultureOverridesCurrentCulture(string cultureName, string number, string ordinalized)
        {
            var culture = new CultureInfo(cultureName);
            Assert.Equal(number.Ordinalize(culture), ordinalized);
        }

        [Theory]
        [InlineData("en-US", 1, "1st")]
        [InlineData("nl-NL", 1, "1e")]
        public void OrdinalizeNumberWithCultureOverridesCurrentCulture(string cultureName, int number, string ordinalized)
        {
            var culture = new CultureInfo(cultureName);
            Assert.Equal(number.Ordinalize(culture), ordinalized);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(8)]
        public void OrdinalizeNumberWithOverridenCultureGenderIsImmaterial(int number)
        {
            var culture = new CultureInfo("nl-NL");
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine, culture);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine, culture);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("8")]
        public void OrdinalizeStringWithOverridenGenderIsImmaterial(string number)
        {
            var culture = new CultureInfo("nl-NL");
            var masculineOrdinalized = number.Ordinalize(GrammaticalGender.Masculine, culture);
            var feminineOrdinalized = number.Ordinalize(GrammaticalGender.Feminine, culture);
            Assert.Equal(masculineOrdinalized, feminineOrdinalized);
        }

        [Theory]
        [InlineData(1, WordForm.Normal, "es-ES", "1.º")]
        [InlineData(1, WordForm.Abbreviation, "es-ES", "1.er")]
        [InlineData(1, WordForm.Normal, "en-US", "1st")]
        [InlineData(1, WordForm.Abbreviation, "en-US", "1st")]
        public void OrdinalizeNumberWithOverridenCultureAndSpecificForm(int number, WordForm wordForm, string cultureName, string expected)
        {
            var culture = new CultureInfo(cultureName);
            Assert.Equal(expected, number.Ordinalize(culture, wordForm));
            Assert.Equal(expected, number.ToString(culture).Ordinalize(culture, wordForm));
        }

        [Theory]
        [InlineData(1, WordForm.Normal, GrammaticalGender.Masculine, "es-ES", "1.º")]
        [InlineData(1, WordForm.Abbreviation, GrammaticalGender.Masculine, "es-ES", "1.er")]
        [InlineData(1, WordForm.Normal, GrammaticalGender.Feminine, "es-ES", "1.ª")]
        [InlineData(1, WordForm.Abbreviation, GrammaticalGender.Feminine, "es-ES", "1.ª")]
        [InlineData(1, WordForm.Normal, GrammaticalGender.Masculine, "en-US", "1st")]
        [InlineData(1, WordForm.Normal, GrammaticalGender.Feminine, "en-US", "1st")]
        public void OrdinalizeNumberWithOverridenCultureAndGenderAndForm(
            int number,
            WordForm wordForm,
            GrammaticalGender gender,
            string cultureName,
            string expected)
        {
            var culture = new CultureInfo(cultureName);
            Assert.Equal(expected, number.Ordinalize(gender, culture, wordForm));
            Assert.Equal(expected, number.ToString(culture).Ordinalize(gender, culture, wordForm));
        }
    }
}
