namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lv
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("lv", "vakardien", "pēc 2 dienām", "2 dienas", "bez laika");
}
