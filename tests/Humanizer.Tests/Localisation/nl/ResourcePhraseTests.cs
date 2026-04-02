namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nl
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("nl", "gisteren", "over 2 dagen", "2 dagen", "geen tijd");
}
