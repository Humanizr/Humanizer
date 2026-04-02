namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ru
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("ru", "вчера", "через 2 дня", "2 дня", "нет времени");
}
