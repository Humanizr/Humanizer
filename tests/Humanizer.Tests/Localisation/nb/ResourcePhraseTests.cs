namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nb
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("nb", "i går", "2 dager fra nå", "2 dager", "ingen tid");
}
