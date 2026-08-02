namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliNumberToWordsTests
{
    static readonly CultureInfo So = new("so");

    [Theory]
    [InlineData(0, "eber")]
    [InlineData(5, "shan")]
    [InlineData(10, "toban")]
    [InlineData(11, "kow iyo toban")]
    [InlineData(19, "sagaal iyo toban")]
    [InlineData(21, "kow iyo labaatan")]
    [InlineData(99, "sagaal iyo sagaashan")]
    [InlineData(100, "boqol")]
    [InlineData(105, "boqol iyo shan")]
    [InlineData(120, "boqol iyo labaatan")]
    [InlineData(234, "laba boqol iyo afar iyo soddon")]
    [InlineData(1000, "kun")]
    [InlineData(2021, "laba kun kow iyo labaatan")]
    [InlineData(1234, "kun laba boqol iyo afar iyo soddon")]
    [InlineData(1000000, "milyan")]
    [InlineData(2000000, "laba milyan")]
    public void NumberToWords_ProducesExpectedSomaliOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(So));
    }

    [Theory]
    [InlineData(-5, "laga jaray shan")]
    [InlineData(-1000, "laga jaray kun")]
    public void NumberToWords_UsesSomaliNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(So));
    }

    [Theory]
    [InlineData(1, "koowaad")]
    [InlineData(2, "labaad")]
    [InlineData(3, "saddexaad")]
    [InlineData(21, "kow iyo labaatanaad")]
    [InlineData(100, "boqolaad")]
    [InlineData(-1, "laga jaray koowaad")]
    public void NumberToOrdinalWords_ProducesExpectedSomaliOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(So));
    }

    [Theory]
    [InlineData(10, "toban")]
    [InlineData(11, "kow iyo toban")]
    [InlineData(19, "sagaal iyo toban")]
    [InlineData(21, "kow iyo labaatan")]
    [InlineData(99, "sagaal iyo sagaashan")]
    [InlineData(105, "boqol iyo shan")]
    [InlineData(120, "boqol iyo labaatan")]
    [InlineData(1234, "kun laba boqol iyo afar iyo soddon")]
    [InlineData(2021, "laba kun kow iyo labaatan")]
    [InlineData(1000000, "milyan")]
    [InlineData(2000000, "laba milyan")]
    public void WordsToNumber_RoundTripsSomaliCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(So));
        Assert.True(words.TryToNumber(out var parsed, So, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("koowaad", 1)]
    [InlineData("kow iyo labaatanaad", 21)]
    [InlineData("laga jaray koowaad", -1)]
    public void WordsToNumber_ParsesSomaliOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(So));
    }

    [Fact]
    public void WordsToNumber_BoundsRecursiveInvertedTensParsing()
    {
        var legitimateWords = string.Concat(Enumerable.Repeat("kun", 64));
        Assert.Equal(64000, legitimateWords.ToNumber(So));
        Assert.Equal(20, "labaatan".ToNumber(So));
        Assert.Equal(2, "labaad".ToNumber(So));
        Assert.Equal(20.1m, "labaatan dhibic kow".ToDecimalNumber(So));
        Assert.Equal(21, "einundzwanzig".ToNumber(new("de")));

        var scaleWords = string.Concat(Enumerable.Repeat("kun", 256));
        AssertRejected(scaleWords);
        AssertRejected($"laga jaray {scaleWords}");
        AssertRejected("laba" + string.Concat(Enumerable.Repeat("aad", 256)));

        Assert.False($"{scaleWords} dhibic kow".TryToDecimalNumber(out var parsedDecimal, So, out var unrecognizedWord));
        Assert.Equal(0, parsedDecimal);
        Assert.NotNull(unrecognizedWord);

        void AssertRejected(string words)
        {
            var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();
            Assert.False(words.TryToNumber(out var parsed, So, out var unrecognizedWord));
            var allocated = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;

            Assert.Equal(0, parsed);
            Assert.NotNull(unrecognizedWord);
            Assert.True(allocated < 8_000_000, $"Inverted-tens rejection allocated {allocated:N0} bytes.");
            Assert.Throws<ArgumentException>(() => words.ToNumber(So));
        }
    }
}