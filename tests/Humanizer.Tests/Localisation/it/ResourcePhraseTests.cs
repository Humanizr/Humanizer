namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_it
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("it", "ieri", "tra 2 giorni", "2 giorni", "0 secondi");
}
