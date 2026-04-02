namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hu
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("hu", "tegnap", "2 nap múlva", "2 nap", "nincs idő");
}
