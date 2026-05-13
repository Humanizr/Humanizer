namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Bit, 1, "ਬਿਟ")]
    [InlineData(DataUnit.Byte, 1, "ਬਾਈਟ")]
    [InlineData(DataUnit.Byte, 2, "ਬਾਈਟ")]
    [InlineData(DataUnit.Kilobyte, 2, "ਕਿਲੋਬਾਈਟ")]
    [InlineData(DataUnit.Megabyte, 2, "ਮੇਗਾਬਾਈਟ")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("pa"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(1024, "1 KB")]
    [InlineData(1048576, "1 MB")]
    public void ByteSize_SymbolForms(long bytes, string expected)
    {
        Assert.Equal(expected, bytes.Bytes().Humanize());
    }
}