namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uk
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("uk", "вчора", "через 2 дні", "2 дні", "без часу");
}
