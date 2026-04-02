namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lb
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("lb", "gëschter", "iwwermuer", "2 Deeg", "Keng Zäit");
}
