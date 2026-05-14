namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Byte, "baita")]
    [InlineData(DataUnit.Kilobyte, "kilobaita")]
    [InlineData(DataUnit.Megabyte, "megabaita")]
    public void DataUnitHumanize_UsesIgboNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanize_UsesSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ig"));
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: true));
    }
}