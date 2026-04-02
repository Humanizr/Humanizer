namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Cyrl_UZ
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("uz-Cyrl-UZ", "кеча", "2 кундан сўнг", "2 кун", "вақт йўқ");
}
