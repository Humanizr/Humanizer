namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sk
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("sk", "včera", "o 2 dni", "2 dni", "žiadny čas");
}
