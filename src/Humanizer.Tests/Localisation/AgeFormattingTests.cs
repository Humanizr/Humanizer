using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class AgeFormattingTests
{
    [Theory]
    [InlineData("ru", "{0} лет")]
    [InlineData("es", "{0} años")]
    [InlineData("fr", "{0} ans")]
    [InlineData("de", "{0} alt")]
    [InlineData("bg", "{0} годишен")]
    [InlineData("pt", "{0} anos")]
    [InlineData("pt-BR", "{0} anos")]
    [InlineData("it", "{0} anni")]
    [InlineData("hu", "{0} éves")]
    [InlineData("zh-CN", "{0} 岁")]
    [InlineData("zh-Hans", "{0} 岁")]
    [InlineData("zh-Hant", "{0} 歲")]
    [InlineData("ja", "{0} 歳")]
    [InlineData("ko-KR", "{0} 세")]
    [InlineData("ar", "{0} سنة")]
    [InlineData("he", "{0} שנים")]
    [InlineData("pl", "{0} lat")]
    [InlineData("nl", "{0} jaar")]
    [InlineData("sv", "{0} år")]
    [InlineData("da", "{0} år")]
    [InlineData("nb", "{0} år")]
    [InlineData("fi-FI", "{0} vuotta")]
    [InlineData("cs", "{0} let")]
    [InlineData("sk", "{0} rokov")]
    [InlineData("sl", "{0} let")]
    [InlineData("hr", "{0} godina")]
    [InlineData("sr", "{0} година")]
    [InlineData("sr-Latn", "{0} godina")]
    [InlineData("uk", "{0} років")]
    [InlineData("tr", "{0} yaşında")]
    [InlineData("el", "{0} ετών")]
    [InlineData("ro", "{0} ani")]
    public void TimeSpanHumanizeAgeShouldBeTranslated(string cultureName, string expected)
    {
        var culture = new CultureInfo(cultureName);
        var actual = Resources.GetResource("TimeSpanHumanize_Age", culture);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("es", "hace dos días")]
    [InlineData("es", "en dos días")]
    [InlineData("fr", "il y a deux jours")]
    [InlineData("fr", "dans deux jours")]
    [InlineData("de", "vor zwei Tagen")]
    [InlineData("de", "in zwei Tagen")]
    [InlineData("bg", "преди два дена")]
    [InlineData("bg", "след два дена")]
    public void TwoDaysTranslationsShouldExist(string cultureName, string expectedPattern)
    {
        var culture = new CultureInfo(cultureName);
        
        // Test that we have some version of "two days" translations
        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", culture);
        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", culture);
        
        Assert.NotNull(twoDaysAgo);
        Assert.NotNull(twoDaysFromNow);
        Assert.NotEmpty(twoDaysAgo);
        Assert.NotEmpty(twoDaysFromNow);
        
        // At least one should match our expected pattern (depending on which key it is)
        Assert.True(twoDaysAgo == expectedPattern || twoDaysFromNow == expectedPattern, 
            $"Expected '{expectedPattern}' in either '{twoDaysAgo}' or '{twoDaysFromNow}'");
    }
}