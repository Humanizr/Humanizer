namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Bit, 1, "बिट")]
    [InlineData(DataUnit.Byte, 1, "बाइट")]
    [InlineData(DataUnit.Byte, 2, "बाइट")]
    [InlineData(DataUnit.Kilobyte, 2, "किलोबाइट")]
    [InlineData(DataUnit.Megabyte, 2, "मेगाबाइट")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ne"));
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