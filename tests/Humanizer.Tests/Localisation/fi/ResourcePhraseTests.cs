namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fi
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("fi", "eilen", "2 päivän päästä", "2 days", "nyt");
}
