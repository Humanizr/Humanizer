namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_id
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("id", "kemarin", "2 hari dari sekarang", "2 hari", "waktu kosong");
}
