//The MIT License (MIT)

//Copyright (c) 2013-2014 Omar Khudeira (http://omar.io)

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

[UseCulture("en")]
public class ToStringTests
{
    [Fact]
    public void ReturnsLargestMetricSuffix() =>
        Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.5).ToString());

    [Fact]
    public void ReturnsDefaultNumberFormat()
    {
        Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.501).ToString());
        Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.5).ToString("KB"));
    }

    [Fact]
    public void ReturnsProvidedNumberFormat() =>
        Assert.Equal("10.1234 KB", ByteSize.FromKilobytes(10.1234).ToString("#.#### KB"));

    [Fact]
    public void ReturnsBits() =>
        Assert.Equal("10 b", ByteSize.FromBits(10).ToString("##.#### b"));

    [Fact]
    public void ReturnsBytes() =>
        Assert.Equal("10 B", ByteSize.FromBytes(10).ToString("##.#### B"));

    [Fact]
    public void ReturnsKilobytes() =>
        Assert.Equal("10 KB", ByteSize.FromKilobytes(10).ToString("##.#### KB"));

    [Fact]
    public void ReturnsMegabytes() =>
        Assert.Equal("10 MB", ByteSize.FromMegabytes(10).ToString("##.#### MB"));

    [Fact]
    public void ReturnsGigabytes() =>
        Assert.Equal("10 GB", ByteSize.FromGigabytes(10).ToString("##.#### GB"));

    [Fact]
    public void ReturnsTerabytes() =>
        Assert.Equal("10 TB", ByteSize.FromTerabytes(10).ToString("##.#### TB"));

    [Fact]
    public void ReturnsSelectedFormat() =>
        Assert.Equal("10.0 TB", ByteSize.FromTerabytes(10).ToString("0.0 TB"));

    [Fact]
    public void ReturnsLargestMetricPrefixLargerThanZero() =>
        Assert.Equal("512 KB", ByteSize.FromMegabytes(.5).ToString("#.#"));

    [Fact]
    public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues() =>
        Assert.Equal("-512 KB", ByteSize.FromMegabytes(-.5).ToString("#.#"));

    [Fact]
    public void ReturnsBytesViaGeneralFormat() =>
        Assert.Equal("10 B", $"{ByteSize.FromBytes(10)}");

    [Fact]
    public void LargestWholeNumberSymbolReturnsTerabytes() =>
        Assert.Equal("TB", ByteSize.FromTerabytes(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolReturnsGigabytes() =>
        Assert.Equal("GB", ByteSize.FromGigabytes(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolReturnsMegabytes() =>
        Assert.Equal("MB", ByteSize.FromMegabytes(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolReturnsKilobytes() =>
        Assert.Equal("KB", ByteSize.FromKilobytes(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolReturnsBytes() =>
        Assert.Equal("B", ByteSize.FromBytes(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolReturnsBits() =>
        Assert.Equal("b", ByteSize.FromBits(2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberFullWordReturnsTerabytes() =>
        Assert.Equal("terabytes", ByteSize.FromTerabytes(2).LargestWholeNumberFullWord);

    [Fact]
    public void LargestWholeNumberFullWordReturnsGigabytes() =>
        Assert.Equal("gigabytes", ByteSize.FromGigabytes(2).LargestWholeNumberFullWord);

    [Fact]
    public void LargestWholeNumberFullWordReturnsMegabytes() =>
        Assert.Equal("megabytes", ByteSize.FromMegabytes(2).LargestWholeNumberFullWord);

    [Fact]
    public void LargestWholeNumberFullWordReturnsKilobytes() =>
        Assert.Equal("kilobytes", ByteSize.FromKilobytes(2).LargestWholeNumberFullWord);

    [Fact]
    public void LargestWholeNumberFullWordReturnsBytes() =>
        Assert.Equal("bytes", ByteSize.FromBytes(2).LargestWholeNumberFullWord);

    [Fact]
    public void LargestWholeNumberFullWordReturnsBits() =>
        Assert.Equal("bits", ByteSize.FromBits(2).LargestWholeNumberFullWord);

    [Theory]
    [InlineData(2d * 1024 * 1024 * 1024 * 1024, 2d)]
    [InlineData(2d * 1024 * 1024 * 1024, 2d)]
    [InlineData(2d * 1024 * 1024, 2d)]
    [InlineData(2d * 1024, 2d)]
    [InlineData(2, 2d)]
    public void LargestWholeNumberValueReturnsTwoForEachTier(double bytes, double expected) =>
        Assert.Equal(expected, ByteSize.FromBytes(bytes).LargestWholeNumberValue);

    [Fact]
    public void LargestWholeNumberValueReturnsBitsWhenLessThanOneByte()
    {
        var result = ByteSize.FromBits(4);
        Assert.Equal(4, result.LargestWholeNumberValue);
    }

    [Fact]
    public void LargestWholeNumberSymbolNegativeTerabytes() =>
        Assert.Equal("TB", ByteSize.FromTerabytes(-2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolNegativeGigabytes() =>
        Assert.Equal("GB", ByteSize.FromGigabytes(-2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolNegativeMegabytes() =>
        Assert.Equal("MB", ByteSize.FromMegabytes(-2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolNegativeKilobytes() =>
        Assert.Equal("KB", ByteSize.FromKilobytes(-2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolNegativeBytes() =>
        Assert.Equal("B", ByteSize.FromBytes(-2).LargestWholeNumberSymbol);

    [Fact]
    public void LargestWholeNumberSymbolNegativeBits() =>
        Assert.Equal("b", ByteSize.FromBits(-2).LargestWholeNumberSymbol);

    [Fact]
    public void ToStringWithNullFormatUsesGeneral() =>
        Assert.Equal("10 KB", ByteSize.FromKilobytes(10).ToString((string?)null));

    [Fact]
    public void ToStringWithUnitOnlyFormat() =>
        Assert.Equal("10 KB", ByteSize.FromKilobytes(10).ToString("KB"));

    [Fact]
    public void ToStringWithHashFormat() =>
        Assert.Equal("10 KB", ByteSize.FromKilobytes(10).ToString("#.## KB"));

    [Fact]
    public void ToStringDefaultFallbackFormatsLargestWholeNumber()
    {
        // Format with no unit symbol or # or 0 triggers fallback formatting
        var result = ByteSize.FromKilobytes(10).ToString("N2");
        Assert.Contains("KB", result);
    }

    [Fact]
    public void ToFullWordsWithFormat()
    {
        var result = ByteSize.FromKilobytes(10).ToFullWords("KB");
        Assert.Contains("kilobytes", result);
    }

    [Fact]
    public void ToFullWordsWithNullFormat() =>
        Assert.Equal("10 kilobytes", ByteSize.FromKilobytes(10).ToFullWords());

    [Fact]
    public void ToStringWithNullProvider()
    {
        var result = ByteSize.FromKilobytes(10).ToString((IFormatProvider?)null);
        Assert.Contains("KB", result);
    }

    [Fact]
    public void ToStringIFormattableWithNullFormatAndProvider()
    {
        var size = ByteSize.FromKilobytes(10);
        var result = ((IFormattable)size).ToString(null, null);
        Assert.Contains("KB", result);
    }
}