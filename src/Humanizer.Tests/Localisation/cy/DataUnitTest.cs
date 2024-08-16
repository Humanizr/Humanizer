namespace cy;

[UseCulture("cy")]
public class DataUnitTests
{
    [Fact]
    public void BitShort() =>
        Assert.Equal("5 b", 5.Bits().ToString());

    [Fact]
    public void BitLong() =>
        Assert.Equal("5 b", 5.Bits().ToFullWords());

    [Fact]
    public void ByteShort() =>
    Assert.Equal("5 b", 5.Bits().ToString());

    [Fact]
    public void ByteLong() =>
        Assert.Equal("5 b", 5.Bits().ToFullWords());
}