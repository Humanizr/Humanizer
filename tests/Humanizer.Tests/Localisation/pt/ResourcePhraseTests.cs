namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pt
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("pt", "ontem", "daqui a 2 dias", "2 dias", "sem horário");
}
