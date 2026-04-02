namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fr
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("fr", "hier", "après-demain", "2 jours", "temps nul");
}
