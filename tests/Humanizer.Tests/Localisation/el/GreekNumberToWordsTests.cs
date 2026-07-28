namespace Humanizer.Tests.Localisation.el;

public class GreekNumberToWordsTests
{
    [Fact]
    public void ToWords_UsesFeminineHundreds() =>
        Assert.Equal("εννιακόσιες", 900.ToWords(GrammaticalGender.Feminine, new CultureInfo("el")));
}