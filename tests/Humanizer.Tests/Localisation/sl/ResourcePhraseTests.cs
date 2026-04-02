namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sl
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("sl", "včeraj", "čez 2 dni", "2 dneva", "nič časa");
}
