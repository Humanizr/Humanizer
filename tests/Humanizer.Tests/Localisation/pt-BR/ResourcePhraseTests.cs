namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pt_BR
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("pt-BR", "ontem", "em 2 dias", "2 dias", "sem horário");
}
