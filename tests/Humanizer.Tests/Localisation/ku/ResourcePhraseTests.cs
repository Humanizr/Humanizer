namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ku
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ku", "دوێنێ", "2 ڕۆژی دیکە", "2 ڕۆژ", "ئێستا");
}
