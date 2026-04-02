namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_el
{
    [Fact]
    public void UsesExpectedResourceBackedPhrases() =>
        LocaleResourcePhraseAssertions.Verify("el", "χθες", "2 ημέρες από τώρα", "2 μέρες", "μηδέν χρόνος");
}
