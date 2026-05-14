namespace Humanizer.Tests.Localisation.kn;

[UseCulture("kn")]
public class KannadaNumberToWordsTests
{
    static readonly CultureInfo Kn = new("kn");

    [Theory]
    [InlineData(0, "ಸೊನ್ನೆ")]
    [InlineData(5, "ಐದು")]
    [InlineData(21, "ಇಪ್ಪತ್ತೊಂದು")]
    [InlineData(99, "ತೊಂಬತ್ತೊಂಬತ್ತು")]
    [InlineData(100, "ನೂರು")]
    [InlineData(101, "ನೂರ ಒಂದು")]
    [InlineData(200, "ಇನ್ನೂರು")]
    [InlineData(201, "ಇನ್ನೂರ ಒಂದು")]
    [InlineData(1000, "ಸಾವಿರ")]
    [InlineData(1001, "ಸಾವಿರದ ಒಂದು")]
    [InlineData(2000, "ಎರಡು ಸಾವಿರ")]
    [InlineData(2001, "ಎರಡು ಸಾವಿರದ ಒಂದು")]
    [InlineData(100000, "ಒಂದು ಲಕ್ಷ")]
    [InlineData(200000, "ಎರಡು ಲಕ್ಷ")]
    [InlineData(200001, "ಎರಡು ಲಕ್ಷದ ಒಂದು")]
    [InlineData(10000000, "ಒಂದು ಕೋಟಿ")]
    [InlineData(20000000, "ಎರಡು ಕೋಟಿ")]
    [InlineData(20000001, "ಎರಡು ಕೋಟಿಯ ಒಂದು")]
    [InlineData(12_345_678, "ಒಂದು ಕೋಟಿಯ ಇಪ್ಪತ್ತಮೂರು ಲಕ್ಷದ ನಲವತ್ತೈದು ಸಾವಿರದ ಆರುನೂರ ಎಪ್ಪತ್ತೆಂಟು")]
    [InlineData(1_001_000_001, "ಒಂದು ಅರಬ್ ಹತ್ತು ಲಕ್ಷದ ಒಂದು")]
    [InlineData(4_325_010_007_018, "ನಲವತ್ತಮೂರು ಖರಬ್ ಇಪ್ಪತ್ತೈದು ಅರಬ್ ಒಂದು ಕೋಟಿಯ ಏಳು ಸಾವಿರದ ಹದಿನೆಂಟು")]
    public void NumberToWords_ProducesExpectedKannadaOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Kn));
    }

    [Theory]
    [InlineData(-5, "ಋಣ ಐದು")]
    [InlineData(-100000, "ಋಣ ಒಂದು ಲಕ್ಷ")]
    public void NumberToWords_UsesKannadaNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Kn));
    }

    [Theory]
    [InlineData(1, "ಒಂದನೇ")]
    [InlineData(2, "ಎರಡನೇ")]
    [InlineData(21, "ಇಪ್ಪತ್ತೊಂದನೇ")]
    [InlineData(101, "ನೂರ ಒಂದನೇ")]
    [InlineData(200, "ಇನ್ನೂರನೇ")]
    [InlineData(1000, "ಸಾವಿರನೇ")]
    [InlineData(1001, "ಸಾವಿರದ ಒಂದನೇ")]
    [InlineData(1234, "ಸಾವಿರದ ಇನ್ನೂರ ಮೂವತ್ತನಾಲ್ಕನೇ")]
    [InlineData(2000, "ಎರಡು ಸಾವಿರನೇ")]
    [InlineData(200000, "ಎರಡು ಲಕ್ಷನೇ")]
    [InlineData(20000000, "ಎರಡು ಕೋಟಿಯನೇ")]
    public void NumberToOrdinalWords_ProducesExpectedKannadaOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Kn));
    }

    [Theory]
    [InlineData(21, "ಇಪ್ಪತ್ತೊಂದು")]
    [InlineData(101, "ನೂರ ಒಂದು")]
    [InlineData(2001, "ಎರಡು ಸಾವಿರದ ಒಂದು")]
    [InlineData(200001, "ಎರಡು ಲಕ್ಷದ ಒಂದು")]
    [InlineData(20000001, "ಎರಡು ಕೋಟಿಯ ಒಂದು")]
    [InlineData(12_345_678, "ಒಂದು ಕೋಟಿಯ ಇಪ್ಪತ್ತಮೂರು ಲಕ್ಷದ ನಲವತ್ತೈದು ಸಾವಿರದ ಆರುನೂರ ಎಪ್ಪತ್ತೆಂಟು")]
    [InlineData(1_001_000_001, "ಒಂದು ಅರಬ್ ಹತ್ತು ಲಕ್ಷದ ಒಂದು")]
    [InlineData(4_325_010_007_018, "ನಲವತ್ತಮೂರು ಖರಬ್ ಇಪ್ಪತ್ತೈದು ಅರಬ್ ಒಂದು ಕೋಟಿಯ ಏಳು ಸಾವಿರದ ಹದಿನೆಂಟು")]
    public void WordsToNumber_RoundTripsKannadaCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Kn));
        Assert.True(words.TryToNumber(out var parsed, Kn, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ಇಪ್ಪತ್ತೊಂದನೇ", 21)]
    [InlineData("ನೂರ ಒಂದನೇ", 101)]
    [InlineData("ಇನ್ನೂರನೇ", 200)]
    [InlineData("ಸಾವಿರದ ಒಂದನೇ", 1001)]
    [InlineData("ಸಾವಿರದ ಇನ್ನೂರ ಮೂವತ್ತನಾಲ್ಕನೇ", 1234)]
    [InlineData("ಎರಡು ಸಾವಿರನೇ", 2000)]
    [InlineData("ಎರಡು ಲಕ್ಷನೇ", 200000)]
    [InlineData("ಎರಡು ಕೋಟಿಯನೇ", 20000000)]
    [InlineData("ಋಣ ಐದನೇ", -5)]
    public void WordsToNumber_ParsesKannadaOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Kn));
    }

    [Fact]
    public void WordsToNumber_RoundTripsKannadaLongMinValue()
    {
        var words = long.MinValue.ToWords(Kn);

        Assert.Equal(long.MinValue, words.ToNumber(Kn));
        Assert.True(words.TryToNumber(out var parsed, Kn, out var unrecognizedWord));
        Assert.Equal(long.MinValue, parsed);
        Assert.Null(unrecognizedWord);
    }
}