namespace Humanizer.Tests.Localisation.mn;

[UseCulture("mn")]
public class MongolianNumberToWordsTests
{
    static readonly CultureInfo Mn = new("mn");

    [Theory]
    [InlineData(0, "тэг")]
    [InlineData(1, "нэг")]
    [InlineData(10, "арав")]
    [InlineData(11, "арван нэг")]
    [InlineData(20, "хорь")]
    [InlineData(21, "хорин нэг")]
    [InlineData(40, "дөч")]
    [InlineData(99, "ерэн ес")]
    [InlineData(100, "нэг зуу")]
    [InlineData(101, "нэг зуун нэг")]
    [InlineData(234, "хоёр зуун гучин дөрөв")]
    [InlineData(1000, "нэг мянга")]
    [InlineData(2026, "хоёр мянга хорин зургаа")]
    [InlineData(1000000, "нэг сая")]
    [InlineData(1000000000000, "нэг их наяд")]
    public void NumberToWords_ProducesExpectedMongolianOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Mn));
    }

    [Theory]
    [InlineData(-5, "хасах тав")]
    [InlineData(-1000, "хасах нэг мянга")]
    public void NumberToWords_UsesMongolianNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Mn));
    }

    [Theory]
    [InlineData(1, "нэгдүгээр")]
    [InlineData(2, "хоёрдугаар")]
    [InlineData(4, "дөрөвдүгээр")]
    [InlineData(21, "хорин нэгдүгээр")]
    [InlineData(99, "ерэн есдүгээр")]
    [InlineData(123, "нэг зуун хорин гурав дахь")]
    public void NumberToOrdinalWords_ProducesExpectedMongolianOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Mn));
    }

    [Theory]
    [InlineData("тэг", 0)]
    [InlineData("хорин нэг", 21)]
    [InlineData("хоёр зуун гучин дөрөв", 234)]
    [InlineData("хоёр мянга хорин зургаа", 2026)]
    [InlineData("хасах тав", -5)]
    [InlineData("нэг их наяд", 1000000000000)]
    [InlineData("дөрвөн их наяд гурван зуун хорин тав тэрбум арван сая долоон мянга арван найм", 4_325_010_007_018)]
    public void WordsToNumber_ParsesMongolianCardinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Mn));
        Assert.True(words.TryToNumber(out var parsed, Mn, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("нэгдүгээр", 1)]
    [InlineData("хорин нэгдүгээр", 21)]
    [InlineData("хасах хоёрдугаар", -2)]
    public void WordsToNumber_ParsesMongolianOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Mn));
    }
}