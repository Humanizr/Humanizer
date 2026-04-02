namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_es
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("es", "ayer", "en 2 días", "2 días", "nada");
}
