namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fa
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("fa", "دیروز", "2 روز بعد", "2 روز", "الآن");
}
