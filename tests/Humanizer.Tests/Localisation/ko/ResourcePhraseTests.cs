namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ko
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ko", "어제", "2일 후", "2일", "방금");
}
