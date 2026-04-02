namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ca
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ca", "ahir", "d'aquí 2 dies", "2 dies", "res");
}
