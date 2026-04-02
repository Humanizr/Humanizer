namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("sr", "јуче", "за 2 дана", "2 дана", "без протеклог времена");
}
