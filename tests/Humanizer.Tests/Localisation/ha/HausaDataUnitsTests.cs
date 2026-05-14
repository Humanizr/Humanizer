namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Byte, "baiti")]
    [InlineData(DataUnit.Kilobyte, "kilobaiti")]
    [InlineData(DataUnit.Megabyte, "megabaiti")]
    public void DataUnitHumanize_UsesHausaNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanize_UsesSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ha"));
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: true));
    }
}