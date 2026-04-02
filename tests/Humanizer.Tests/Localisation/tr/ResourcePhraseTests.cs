namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_tr
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("tr", "dün", "2 gün sonra", "2 gün", "zaman farkı yok");
}
