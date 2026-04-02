namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ja
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ja", "昨日", "2 日後", "2 日", "0 秒");
}
