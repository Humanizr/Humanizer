namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ms
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ms", "semalam", "2 hari dari sekarang", "2 hari", "tiada masa");
}
