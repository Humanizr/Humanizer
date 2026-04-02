namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_az
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("az", "dünən", "2 gün sonra", "2 gün", "zaman fərqi yoxdur");
}
