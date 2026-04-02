namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_vi
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("vi", "hôm qua", "2 ngày nữa", "2 ngày", "không giờ");
}
