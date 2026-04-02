namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Latn_UZ
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("uz-Latn-UZ", "kecha", "2 kundan so`ng", "2 kun", "vaqt yo`q");
}
