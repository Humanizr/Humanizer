namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lt
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("lt", "vakar", "po 2 dienų", "2 dienos", "nėra laiko");
}
