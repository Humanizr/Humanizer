namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_mt
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("mt", "il-bieraħ", "pitgħada", "jumejn", "xejn");
}
