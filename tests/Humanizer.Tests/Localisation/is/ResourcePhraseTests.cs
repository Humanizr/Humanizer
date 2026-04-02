namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_is
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("is", "í gær", "eftir 2 daga", "2 dagar", "engin stund");
}
