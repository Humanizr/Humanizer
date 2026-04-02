namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_de
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("de", "gestern", "in 2 Tagen", "2 Tage", "Keine Zeit");
}
