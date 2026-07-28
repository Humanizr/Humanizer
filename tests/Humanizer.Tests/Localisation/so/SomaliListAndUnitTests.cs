namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliListAndUnitTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesIyo()
    {
        Assert.Equal("1 iyo 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndIyo()
    {
        Assert.Equal("1, 2 iyo 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(DataUnit.Byte, "bayt")]
    [InlineData(DataUnit.Kilobyte, "kilobayt")]
    [InlineData(DataUnit.Megabyte, "megabayt")]
    public void DataUnitHumanize_UsesSomaliNames(DataUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, 2, toSymbol: false));
    }

    [Fact]
    public void DataUnitHumanize_UsesSymbols()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal("KB", formatter.DataUnitHumanize(DataUnit.Kilobyte, 2, toSymbol: true));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "ilb")]
    [InlineData(TimeUnit.Minute, "daq")]
    [InlineData(TimeUnit.Hour, "saac")]
    [InlineData(TimeUnit.Day, "maal")]
    public void TimeUnitHumanize_UsesSomaliLabels(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("so"));
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }
}