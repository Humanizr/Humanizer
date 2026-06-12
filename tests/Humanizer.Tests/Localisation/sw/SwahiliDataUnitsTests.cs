namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Byte, "baiti")]
    [InlineData(DataUnit.Kilobyte, "kilobaiti")]
    [InlineData(DataUnit.Megabyte, "megabaiti")]
    public void DataUnitHumanize_UsesSwahiliNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanize_UsesSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("sw"));
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: true));
    }
}