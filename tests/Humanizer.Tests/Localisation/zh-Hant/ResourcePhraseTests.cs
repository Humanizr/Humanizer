namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_Hant
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("zh-Hant", "昨天", "2 天後", "2 天", "沒有時間");
}
