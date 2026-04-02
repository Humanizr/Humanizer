namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_af
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("af", "gister", "oor 2 dae", "2 dae", "geen tyd");
}
