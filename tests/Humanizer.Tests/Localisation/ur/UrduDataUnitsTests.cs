namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduDataUnitsTests
{
    [Theory]
    [InlineData(DataUnit.Bit, 1, "بٹ")]
    [InlineData(DataUnit.Byte, 1, "بائٹ")]
    [InlineData(DataUnit.Byte, 2, "بائٹ")]
    [InlineData(DataUnit.Kilobyte, 2, "کلوبائٹ")]
    [InlineData(DataUnit.Megabyte, 2, "میگابائٹ")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ur"));
        var result = formatter.DataUnitHumanize(unit, count, toSymbol: false);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1024, "1 KB")]
    [InlineData(1048576, "1 MB")]
    public void ByteSize_SymbolForms(long bytes, string expected)
    {
        var result = bytes.Bytes().Humanize();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}