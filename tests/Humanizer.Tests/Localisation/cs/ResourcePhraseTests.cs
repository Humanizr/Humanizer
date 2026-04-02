namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_cs
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("cs", "včera", "za 2 dny", "2 dny", "není čas");
}
