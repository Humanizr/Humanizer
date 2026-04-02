namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ar
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ar", "أمس", "في غضون يومين من الآن", "يومين", "حالاً");
}
