namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_CN
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("zh-CN", "昨天", "2 天后", "2 天", "没有时间");
}
