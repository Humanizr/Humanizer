namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pl
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("pl", "wczoraj", "za 2 dni", "2 dni", "brak czasu");
}
