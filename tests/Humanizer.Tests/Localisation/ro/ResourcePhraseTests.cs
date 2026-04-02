namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ro
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ro", "ieri", "peste 2 zile", "2 zile", "0 secunde");
}
