namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_Hans
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("zh-Hans", "昨天", "2 天后", "2 天", "没有时间");
}
