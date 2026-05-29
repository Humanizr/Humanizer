namespace Humanizer.Tests.Localisation.bg;

[UseCulture("bg")]
public class BulgarianNumberToWordsTests
{
    static readonly CultureInfo Bg = new("bg");

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "един")]
    [InlineData(1, GrammaticalGender.Feminine, "една")]
    [InlineData(1, GrammaticalGender.Neuter, "едно")]
    [InlineData(2, GrammaticalGender.Masculine, "два")]
    [InlineData(2, GrammaticalGender.Feminine, "две")]
    [InlineData(2, GrammaticalGender.Neuter, "две")]
    [InlineData(1000, GrammaticalGender.Neuter, "една хиляда")]
    public void ToWords_UsesBulgarianGenderedUnitForms(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToWords(gender, Bg));

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "нулев")]
    [InlineData(0, GrammaticalGender.Feminine, "нулева")]
    [InlineData(0, GrammaticalGender.Neuter, "нулево")]
    [InlineData(1, GrammaticalGender.Masculine, "първи")]
    [InlineData(1, GrammaticalGender.Feminine, "първа")]
    [InlineData(1, GrammaticalGender.Neuter, "първо")]
    [InlineData(20, GrammaticalGender.Masculine, "двадесети")]
    [InlineData(20, GrammaticalGender.Feminine, "двадесета")]
    [InlineData(20, GrammaticalGender.Neuter, "двадесето")]
    [InlineData(100, GrammaticalGender.Masculine, "стотен")]
    [InlineData(100, GrammaticalGender.Feminine, "стотна")]
    [InlineData(100, GrammaticalGender.Neuter, "стотно")]
    [InlineData(1000, GrammaticalGender.Masculine, "една хиляден")]
    [InlineData(1000, GrammaticalGender.Feminine, "една хилядна")]
    [InlineData(1000, GrammaticalGender.Neuter, "една хилядно")]
    public void ToOrdinalWords_UsesBulgarianGenderedOrdinalForms(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(gender, Bg));
}