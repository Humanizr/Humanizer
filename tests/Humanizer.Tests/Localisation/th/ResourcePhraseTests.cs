namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_th
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("th", "เมื่อวานนี้", "2 วันจากนี้", "2 วัน", "ไม่มีเวลา");
}
