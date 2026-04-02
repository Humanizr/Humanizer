namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fil
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("fil", "kahapon", "2 araw mula ngayon", "2 araw", "walang oras");
}
