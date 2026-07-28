namespace Humanizer.Tests.Localisation.en_GB;

[UseCulture("en-GB")]
public class EnglishUnitedKingdomCollectionHumanizeTests
{
    [Fact]
    public void HumanizeUsesBritishEnglishPunctuation()
    {
        var collection = new[] { "one", "two", "three" };

        Assert.Multiple(
            () => Assert.Equal("one, two and three", collection.Humanize()),
            () => Assert.Equal("one, two or three", collection.Humanize("or")));
    }
}