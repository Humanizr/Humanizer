namespace Humanizer.Tests;

public class WordsToNumberSharedLexiconRegressionTests
{
    [Theory]
    [InlineData("en-US", "one hundred and third", 103)]
    [InlineData("en-GB", "five thousand and ninth", 5009)]
    [InlineData("de-DE", "einundzwanzigste", 21)]
    [InlineData("da-DK", "et hundrede og en", 101)]
    [InlineData("nl-NL", "honderdacht", 108)]
    [InlineData("lb-LU", "dräidausendfënnefhonnerteen", 3501)]
    [InlineData("sv-SE", "tjugoförsta", 21)]
    [InlineData("nb-NO", "tusenogen", 1001)]
    [InlineData("is-IS", "eitt hundrað og þrjú", 103)]
    [InlineData("pt-PT", "um mil milhões e dois", 1_000_000_002)]
    [InlineData("es-ES", "vigésima primera", 21)]
    [InlineData("ro-RO", "douazeci si una de mii", 21_000)]
    [InlineData("fa", "یک\u200cمیلیون و دویست و سی و چهار هزار و پانصد و شصت و هفت", 1_234_567)]
    [InlineData("ku", "نێگەتیڤ نۆ سەد و نەوەد و نۆ", -999)]
    [InlineData("mt", "għoxrin inqas minn żero", -20)]
    [InlineData("sk", "minus devatdesiat devat", -99)]
    [InlineData("vi", "thứ hai mươi mốt", 21)]
    public void SharedLexiconConvertersPreserveExistingLocaleBehavior(string cultureName, string words, int expected)
    {
        var culture = new CultureInfo(cultureName);

        Assert.True(words.TryToNumber(out var parsed, culture, out var unrecognizedWord));
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, parsed);
    }
}
