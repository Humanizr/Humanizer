namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bg
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("bg", "вчера", "след 2 дена", "2 дена", "няма време");
}
