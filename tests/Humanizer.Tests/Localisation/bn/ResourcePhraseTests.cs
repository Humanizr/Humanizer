namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bn
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("bn", "গতকাল", "2 দিন পর", "2 দিন", "শূন্য সময়");
}
