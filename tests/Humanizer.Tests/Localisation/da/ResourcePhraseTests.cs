namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_da
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("da", "i går", "2 dage fra nu", "2 dage", "ingen tid");
}
