namespace Humanizer.Tests.Localisation.el;

[UseCulture("el")]
public class GreekNumberToWordsTests
{
    static readonly CultureInfo El = new("el");

    [Theory]
    [InlineData(900, GrammaticalGender.Masculine, "εννιακόσια")]
    [InlineData(900, GrammaticalGender.Feminine, "εννιακόσιες")]
    [InlineData(900, GrammaticalGender.Neuter, "εννιακόσια")]
    [InlineData(1900, GrammaticalGender.Feminine, "χίλια εννιακόσιες")]
    public void ToWords_UsesRequestedGender(int number, GrammaticalGender gender, string expected) =>
        Assert.Equal(expected, number.ToWords(gender, El));

    [Fact]
    public void ToWords_UsesDefaultGender() =>
        Assert.Equal("εννιακόσια", 900.ToWords(El));

    [Fact]
    public void ToWords_WithWordForm_UsesRequestedGender() =>
        Assert.Equal("εννιακόσιες", 900.ToWords(WordForm.Normal, GrammaticalGender.Feminine, El));
}