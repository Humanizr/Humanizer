namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr_Latn
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("sr-Latn", "juče", "za 2 dana", "2 dana", "bez proteklog vremena");
}
