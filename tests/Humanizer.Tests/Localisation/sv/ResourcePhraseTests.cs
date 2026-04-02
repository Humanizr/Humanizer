namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sv
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("sv", "igår", "om 2 dagar", "2 dagar", "ingen tid");
}
