namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboNumberFormattingTests
{
    [Fact]
    public void Ordinalize_UsesAuthoredNumberFormatting()
    {
        Assert.Equal("nke -1", (-1).Ordinalize());
    }
}