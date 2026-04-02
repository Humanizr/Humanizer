namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hr
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("hr", "jučer", "2 days from now", "2 dana", "bez podatka o vremenu");
}
