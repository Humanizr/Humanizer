namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hy
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("hy", "երեկ", "2 օրից", "2 օր", "ժամանակը բացակայում է");
}
