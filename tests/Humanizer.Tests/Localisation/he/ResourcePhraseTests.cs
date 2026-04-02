namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_he
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("he", "אתמול", "בעוד יומיים", "יומיים", "אין זמן");
}
