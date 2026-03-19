namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToNumberTests_US
{
    [Theory]
    [InlineData("zero", 0)]
    [InlineData("one", 1)]
    [InlineData("minus five", -5)]
    [InlineData("eleven", 11)]
    [InlineData("ninety five", 95)]
    [InlineData("hundred five", 105)]
    [InlineData("one hundred ninety six", 196)]
    [InlineData("minus one hundred and five", -105)]
    [InlineData("seventeenth", 17)]
    [InlineData("thirtieth", 30)]
    [InlineData("twenty-seventh", 27)]
    [InlineData("thirty-first", 31)]
    [InlineData("minus twenty-first", -21)]
    [InlineData("two thousand twenty three", 2023)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567)]
    [InlineData("one hundred and third", 103)]
    [InlineData("two hundred and first", 201)]
    [InlineData("five thousand and ninth", 5009)]
    [InlineData("17th", 17)]
    [InlineData("31st", 31)]
    [InlineData("100th", 100)]
    [InlineData("203rd", 203)]
    [InlineData("minus 21st", -21)]
    [InlineData("negative five", -5)]
    [InlineData("negative one hundred and five", -105)]
    [InlineData("negative twenty-first", -21)]
    public void ToNumber_US(string words, int expectedNumber) => Assert.Equal(expectedNumber, words.ToNumber(CultureInfo.CurrentCulture));

    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("one", 1, null)]
    [InlineData("minus five", -5, null)]
    [InlineData("eleven", 11, null)]
    [InlineData("ninety five", 95, null)]
    [InlineData("hundred five", 105, null)]
    [InlineData("one hundred ninety six", 196, null)]
    [InlineData("minus one hundred and five", -105, null)]
    [InlineData("seventeenth", 17, null)]
    [InlineData("thirtieth", 30, null)]
    [InlineData("twenty-seventh", 27, null)]
    [InlineData("thirty-first", 31, null)]
    [InlineData("minus twenty-first", -21, null)]
    [InlineData("two thousand twenty three", 2023, null)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567, null)]
    [InlineData("one hundred and third", 103, null)]
    [InlineData("two hundred and first", 201, null)]
    [InlineData("five thousand and ninth", 5009, null)]
    [InlineData("17th", 17, null)]
    [InlineData("31st", 31, null)]
    [InlineData("100th", 100, null)]
    [InlineData("203rd", 203, null)]
    [InlineData("minus 21st", -21, null)]
    [InlineData("negative five", -5, null)]
    [InlineData("negative one hundred and five", -105, null)]
    [InlineData("negative twenty-first", -21, null)]
    public void TryToNumber_ValidInput_US(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("twenty nine hello", 0, "hello")]
    [InlineData("mister three", 0, "mister")]
    [InlineData("tenn", 0, "tenn")]
    [InlineData("twenty sveen", 0, "sveen")]
    [InlineData("minus fift five", 0, "fift")]
    [InlineData("sixty two j", 0, "j")]
    [InlineData("two hundred , ninetyy sevennn", 0, "ninetyy")]
    [InlineData("invalidinput", 0, "invalidinput")]
    [InlineData("30rmd", 0, "30rmd")]
    [InlineData("negative energy", 0, "energy")]
    public void TryToNumber_InvalidInput_US(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

}

[UseCulture("en-GB")]
public class WordsToNumberTests_GB
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("one", 1, null)]
    [InlineData("minus five", -5, null)]
    [InlineData("eleven", 11, null)]
    [InlineData("ninety five", 95, null)]
    [InlineData("hundred five", 105, null)]
    [InlineData("one hundred ninety six", 196, null)]
    [InlineData("minus one hundred and five", -105, null)]
    [InlineData("seventeenth", 17, null)]
    [InlineData("thirtieth", 30, null)]
    [InlineData("twenty-seventh", 27, null)]
    [InlineData("thirty-first", 31, null)]
    [InlineData("minus twenty-first", -21, null)]
    [InlineData("two thousand twenty three", 2023, null)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567, null)]
    [InlineData("one hundred and third", 103, null)]
    [InlineData("two hundred and first", 201, null)]
    [InlineData("five thousand and ninth", 5009, null)]
    [InlineData("17th", 17, null)]
    [InlineData("31st", 31, null)]
    [InlineData("100th", 100, null)]
    [InlineData("203rd", 203, null)]
    [InlineData("minus 21st", -21, null)]
    [InlineData("negative five", -5, null)]
    [InlineData("negative one hundred and five", -105, null)]
    [InlineData("negative twenty-first", -21, null)]
    public void TryToNumber_ValidInput_GB(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("twenty nine hello", 0, "hello")]
    [InlineData("mister three", 0, "mister")]
    [InlineData("tenn", 0, "tenn")]
    [InlineData("twenty sveen", 0, "sveen")]
    [InlineData("minus fift five", 0, "fift")]
    [InlineData("sixty two j", 0, "j")]
    [InlineData("two hundred , ninetyy sevennn", 0, "ninetyy")]
    [InlineData("invalidinput", 0, "invalidinput")]
    [InlineData("30rmd", 0, "30rmd")]
    [InlineData("negative energy", 0, "energy")]
    public void TryToNumber_InvalidInput_GB(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

}

[UseCulture("de-DE")]
public class WordsToNumberTests_German
{
    [Theory]
    [InlineData("null", 0, null)]
    [InlineData("minus fünf", -5, null)]
    [InlineData("einundzwanzig", 21, null)]
    [InlineData("zweiunddreißig", 32, null)]
    [InlineData("einhundertneunundvierzig", 149, null)]
    [InlineData("zweitausendachtundzwanzig", 2028, null)]
    [InlineData("dreißigste", 30, null)]
    [InlineData("einundzwanzigste", 21, null)]
    public void TryToNumber_ValidInput_German(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("zwanzig foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_German(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("de-CH")]
    [InlineData("de-LI")]
    public void ResolveForSwissVariants(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        Assert.Equal(21, "einundzwanzig".ToNumber(culture));
    }
}

[UseCulture("ca")]
public class WordsToNumberTests_Catalan
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("menys cinc", -5, null)]
    [InlineData("vint-i-u", 21, null)]
    [InlineData("trenta-una", 31, null)]
    [InlineData("dos-cents", 200, null)]
    [InlineData("dues-centes", 200, null)]
    [InlineData("cent vint-i-una", 121, null)]
    [InlineData("dos mil vint-i-u", 2021, null)]
    [InlineData("un milió una", 1000001, null)]
    [InlineData("primer", 1, null)]
    [InlineData("segona", 2, null)]
    [InlineData("21è", 21, null)]
    [InlineData("22a", 22, null)]
    public void TryToNumber_ValidInput_Catalan(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("vint foo", 0, "foo")]
    [InlineData("menys xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Catalan(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("fr-FR")]
public class WordsToNumberTests_French
{
    [Theory]
    [InlineData("zéro", 0, null)]
    [InlineData("moins cinq", -5, null)]
    [InlineData("vingt et un", 21, null)]
    [InlineData("trente-et-un", 31, null)]
    [InlineData("soixante et onze", 71, null)]
    [InlineData("quatre-vingt-un", 81, null)]
    [InlineData("quatre-vingt-dix-neuf", 99, null)]
    [InlineData("cent vingt-deux", 122, null)]
    [InlineData("mille deux cent trente-quatre", 1234, null)]
    [InlineData("premier", 1, null)]
    [InlineData("première", 1, null)]
    public void TryToNumber_ValidInput_French(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("vingt foo", 0, "foo")]
    [InlineData("moins xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_French(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("it")]
public class WordsToNumberTests_Italian
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("meno cinque", -5, null)]
    [InlineData("uno", 1, null)]
    [InlineData("ventuno", 21, null)]
    [InlineData("venti uno", 21, null)]
    [InlineData("venti-otto", 28, null)]
    [InlineData("trentotto", 38, null)]
    [InlineData("centoventidue", 122, null)]
    [InlineData("duemilatrecentoquaranta", 2340, null)]
    [InlineData("un milione duecentomila", 1200000, null)]
    [InlineData("quattromilacinquecentosessantasette", 4567, null)]
    [InlineData("venti e uno", 21, null)]
    [InlineData("trenta ed otto", 38, null)]
    [InlineData("ventitré", 23, null)]
    [InlineData("cento e uno", 101, null)]
    public void TryToNumber_ValidInput_Italian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("primo", 1, null)]
    [InlineData("prima", 1, null)]
    [InlineData("secondo", 2, null)]
    [InlineData("seconda", 2, null)]
    [InlineData("ventunesimo", 21, null)]
    [InlineData("ventunesima", 21, null)]
    [InlineData("centounesimo", 101, null)]
    [InlineData("centounesima", 101, null)]
    public void TryToNumber_ValidOrdinalInput_Italian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("venti foo", 0, "foo")]
    [InlineData("meno xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Italian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("it")]
public class WordsToNumberTests_ItalianOrdinalAbbreviations
{
    [Theory]
    [InlineData("1º", 1, null)]
    [InlineData("2ª", 2, null)]
    [InlineData("21º", 21, null)]
    public void TryToNumber_OrdinalAbbreviations(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("3z", 0, "3z")]
    public void TryToNumber_InvalidOrdinalAbbreviations(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("pt-PT")]
public class WordsToNumberTests_Portuguese
{
    [Theory]
    [InlineData("zero", 0)]
    [InlineData("menos cinco", -5)]
    [InlineData("vinte e um", 21)]
    [InlineData("cento e cinco", 105)]
    [InlineData("duzentos e uma", 201)]
    [InlineData("mil e vinte", 1020)]
    [InlineData("um milhão e dois", 1000002)]
    [InlineData("primeiro", 1)]
    [InlineData("primeira", 1)]
    [InlineData("vigésimo primeiro", 21)]
    [InlineData("1º", 1)]
    [InlineData("1ª", 1)]
    public void ToNumber_Portuguese(string words, int expectedNumber) =>
        Assert.Equal(expectedNumber, words.ToNumber(CultureInfo.CurrentCulture));

    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("menos cinco", -5, null)]
    [InlineData("vinte e um", 21, null)]
    [InlineData("cento e cinco", 105, null)]
    [InlineData("duzentos e uma", 201, null)]
    [InlineData("mil e vinte", 1020, null)]
    [InlineData("um milhão e dois", 1000002, null)]
    [InlineData("primeiro", 1, null)]
    [InlineData("primeira", 1, null)]
    [InlineData("vigésimo primeiro", 21, null)]
    [InlineData("1º", 1, null)]
    [InlineData("1ª", 1, null)]
    public void TryToNumber_ValidInput_Portuguese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("vinte foo", 0, "foo")]
    [InlineData("menos xyz", 0, "xyz")]
    [InlineData("treze e direfente", 0, "direfente")]
    [InlineData("123abc", 0, "123abc")]
    public void TryToNumber_InvalidInput_Portuguese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("es-ES")]
public class WordsToNumberTests_Spanish
{
    [Theory]
    [InlineData("cero", 0, null)]
    [InlineData("uno", 1, null)]
    [InlineData("un", 1, null)]
    [InlineData("veintiuno", 21, null)]
    [InlineData("veintiún", 21, null)]
    [InlineData("veintiuna", 21, null)]
    [InlineData("veintidós", 22, null)]
    [InlineData("treinta y cuatro", 34, null)]
    [InlineData("ciento uno", 101, null)]
    [InlineData("quinientos cuarenta y dos", 542, null)]
    [InlineData("mil doscientos treinta y cuatro", 1234, null)]
    [InlineData("dos millones trescientos mil", 2300000, null)]
    [InlineData("menos cinco", -5, null)]
    public void TryToNumber_ValidInput_Spanish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("primero", 1, null)]
    [InlineData("primera", 1, null)]
    [InlineData("segundo", 2, null)]
    [InlineData("vigésimo primero", 21, null)]
    [InlineData("vigésima primera", 21, null)]
    public void TryToNumber_ValidOrdinalInput_Spanish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("1.º", 1, null)]
    [InlineData("1.ª", 1, null)]
    [InlineData("21.º", 21, null)]
    public void TryToNumber_OrdinalAbbreviations_Spanish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("veinte foo", 0, "foo")]
    [InlineData("menos dos mil xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Spanish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("nl-NL")]
public class WordsToNumberTests_Dutch
{
    [Theory]
    [InlineData("nul", 0, null)]
    [InlineData("min tien", -10, null)]
    [InlineData("eenentwintig", 21, null)]
    [InlineData("honderdacht", 108, null)]
    [InlineData("duizend honderdelf", 1111, null)]
    [InlineData("een miljoen", 1000000, null)]
    [InlineData("eerste", 1, null)]
    [InlineData("eenentwintigste", 21, null)]
    public void TryToNumber_ValidInput_Dutch(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("twintig foo", 0, "foo")]
    [InlineData("min xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Dutch(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("sv-SE")]
public class WordsToNumberTests_Swedish
{
    [Theory]
    [InlineData("noll", 0, null)]
    [InlineData("minus tio", -10, null)]
    [InlineData("tjugoett", 21, null)]
    [InlineData("hundraåtta", 108, null)]
    [InlineData("ett tusen", 1000, null)]
    [InlineData("ett tusen hundraelva", 1111, null)]
    [InlineData("en miljon", 1000000, null)]
    [InlineData("första", 1, null)]
    [InlineData("tjugoförsta", 21, null)]
    public void TryToNumber_ValidInput_Swedish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("tjugo foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Swedish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("is-IS")]
public class WordsToNumberTests_Icelandic
{
    [Theory]
    [InlineData("núll", 0, null)]
    [InlineData("mínus fimm", -5, null)]
    [InlineData("tuttugu og eitt", 21, null)]
    [InlineData("tuttugu og tvö", 22, null)]
    [InlineData("fjörutíu og fimm", 45, null)]
    [InlineData("tvö hundruð sextíu", 260, null)]
    [InlineData("eitt hundrað og þrjú", 103, null)]
    [InlineData("þúsund tvö hundruð þrjátíu og fjögur", 1234, null)]
    [InlineData("milljón tvö þúsund", 1002000, null)]
    [InlineData("tveir milljónir", 2000000, null)]
    [InlineData("milljarður", 1000000000, null)]
    public void TryToNumber_ValidInput_Icelandic(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("tuttugu foo", 0, "foo")]
    [InlineData("hundrað og xxx", 0, "xxx")]
    [InlineData("tvö þúsund abc", 0, "abc")]
    [InlineData("milljón og foo", 0, "foo")]
    [InlineData("fimmz", 0, "fimmz")]
    public void TryToNumber_InvalidInput_Icelandic(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(21)]
    [InlineData(31)]
    [InlineData(100)]
    public void TryToNumber_OrdinalRoundTrip_Icelandic(int number)
    {
        var converter = new IcelandicNumberToWordsConverter();
        var ordinal = converter.ConvertToOrdinal(number);
        Assert.True(ordinal.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Null(unrecognizedWord);
        Assert.Equal(number, parsedNumber);
    }
}

[UseCulture("da")]
public class WordsToNumberTests_Danish
{
    [Theory]
    [InlineData("nul", 0, null)]
    [InlineData("minus femoghalvtreds", -55, null)]
    [InlineData("enogtyve", 21, null)]
    [InlineData("nioghalvfems", 99, null)]
    [InlineData("et hundrede og en", 101, null)]
    [InlineData("to hundrede og toogtres", 262, null)]
    [InlineData("et tusind og en", 1001, null)]
    [InlineData("to millioner", 2_000_000, null)]
    public void TryToNumber_ValidInput_Danish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("enogtyve foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Danish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("bg")]
public class WordsToNumberTests_Bulgarian
{
    [Theory]
    [InlineData("нула", 0, null)]
    [InlineData("сто и единадесет", 111, null)]
    [InlineData("петстотин петдесет и пет", 555, null)]
    [InlineData("четири хиляди двеста и тринадесет", 4213, null)]
    [InlineData("четири милиона двеста и десет", 4_000_210, null)]
    public void TryToNumber_ValidInput_Bulgarian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("петдесет foo", 0, "foo")]
    public void TryToNumber_InvalidInput_Bulgarian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("fi-FI")]
public class WordsToNumberTests_Finnish
{
    [Theory]
    [InlineData("nolla", 0, null)]
    [InlineData("satayksi", 101, null)]
    [InlineData("tuhat kaksi", 1002, null)]
    [InlineData("yksitoista", 11, null)]
    [InlineData("miinus viisitoista", -15, null)]
    public void TryToNumber_ValidInput_Finnish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("af")]
public class WordsToNumberTests_Afrikaans
{
    [Theory]
    [InlineData("een", 1, null)]
    [InlineData("een honderd twee en twintig", 122, null)]
    [InlineData("drie duisend vyf honderd en een", 3501, null)]
    [InlineData("een miljard een honderd en elf miljoen een honderd en elf duisend een honderd en elf", 1111111111, null)]
    public void TryToNumber_ValidInput_Afrikaans(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("cs-CZ")]
public class WordsToNumberTests_Czech
{
    [Theory]
    [InlineData("nula", 0, null)]
    [InlineData("sto dvanáct", 112, null)]
    [InlineData("jeden tisíc", 1000, null)]
    [InlineData("dvě miliardy sto čtyřicet sedm milionů čtyři sta osmdesát tři tisíc šest set čtyřicet sedm", 2147483647, null)]
    [InlineData("mínus jedna miliarda pět set jedna milionů jeden tisíc osm set devadesát dva", -1501001892, null)]
    public void TryToNumber_ValidInput_Czech(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("he")]
public class WordsToNumberTests_Hebrew
{
    [Theory]
    [InlineData("אפס", 0, null)]
    [InlineData("מינוס שתיים", -2, null)]
    [InlineData("מאה ואחת", 101, null)]
    [InlineData("מאתיים ארבעים ואחת", 241, null)]
    [InlineData("שבע מאות עשרים וחמש", 725, null)]
    [InlineData("אלף ואחת עשרה", 1011, null)]
    [InlineData("שבעת אלפים עשרים ואחת", 7021, null)]
    [InlineData("עשרים ושמונה אלף מאה עשרים ושלוש", 28123, null)]
    [InlineData("מיליון ואחת", 1_000_001, null)]
    [InlineData("מיליארד ואחד", 1_000_000_001, null)]
    [InlineData("שני מיליון ארבע מאות ושמונה", 2_000_408, null)]
    public void TryToNumber_ValidInput_Hebrew(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("עשרים ושמונה hello", 0, "hello")]
    [InlineData("מינוס xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Hebrew(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("hu-HU")]
public class WordsToNumberTests_Hungarian
{
    [Theory]
    [InlineData("nulla", 0, null)]
    [InlineData("huszonegy", 21, null)]
    [InlineData("száztizenegy", 111, null)]
    [InlineData("kétezer-egy", 2001, null)]
    [InlineData("egymilliárd-kétszázharmincnégymillió-ötszázhatvanhétezer-nyolcszázkilencven", 1234567890, null)]
    [InlineData("mínusz tizennyolc", -18, null)]
    public void TryToNumber_ValidInput_Hungarian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("lt")]
public class WordsToNumberTests_Lithuanian
{
    [Theory]
    [InlineData("minus vienas", -1, null)]
    [InlineData("dvidešimt vienas", 21, null)]
    [InlineData("šimtas penkiasdešimt vienas", 151, null)]
    [InlineData("du tūkstančiai trys šimtai keturiasdešimt penki", 2345, null)]
    [InlineData("šeši milijonai", 6000000, null)]
    public void TryToNumber_ValidInput_Lithuanian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("lv-LV")]
public class WordsToNumberTests_Latvian
{
    [Theory]
    [InlineData("divdesmit viens", 21, null)]
    [InlineData("simtu piecdesmit viens", 151, null)]
    [InlineData("divi tūkstoši trīs simti četrdesmit pieci", 2345, null)]
    [InlineData("seši miljoni", 6000000, null)]
    public void TryToNumber_ValidInput_Latvian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("mt")]
public class WordsToNumberTests_Maltese
{
    [Theory]
    [InlineData("wieħed", 1, null)]
    [InlineData("mija u tlieta u għoxrin", 123, null)]
    [InlineData("tlett elef u ħames mija u wieħed", 3501, null)]
    [InlineData("miljun u mitejn u erbgħa u tletin elf u ħames mija u sebgħa u sittin", 1234567, null)]
    [InlineData("għoxrin inqas minn żero", -20, null)]
    public void TryToNumber_ValidInput_Maltese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("hr-HR")]
public class WordsToNumberTests_Croatian
{
    [Theory]
    [InlineData("nula", 0, null)]
    [InlineData("tri tisuće petsto jedan", 3501, null)]
    [InlineData("dvjesto dvadeset dvije tisuće dvjesto dvadeset dva", 222222, null)]
    [InlineData("minus sedam tisuća petsto šesnaest", -7516, null)]
    public void TryToNumber_ValidInput_Croatian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("ru-RU")]
public class WordsToNumberTests_Russian
{
    [Theory]
    [InlineData("ноль", 0, null)]
    [InlineData("минус сто двадцать три", -123, null)]
    [InlineData("одна тысяча сто двенадцать", 1112, null)]
    [InlineData("сто двадцать одна тысяча триста четырнадцать", 121314, null)]
    [InlineData("два миллиона сто тридцать две тысячи четыреста пятнадцать", 2132415, null)]
    [InlineData("первый", 1, null)]
    [InlineData("третья", 3, null)]
    [InlineData("десятое", 10, null)]
    [InlineData("двадцать вторая", 22, null)]
    public void TryToNumber_ValidInput_Russian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("двадцать foo", 0, "foo")]
    [InlineData("минус xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Russian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("ro-RO")]
public class WordsToNumberTests_Romanian
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("minus unu", -1, null)]
    [InlineData("o sută douăzeci și unu", 121, null)]
    [InlineData("douăzeci și una de mii", 21000, null)]
    [InlineData("două milioane", 2_000_000, null)]
    public void TryToNumber_ValidInput_Romanian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("douăzeci foo", 0, "foo")]
    public void TryToNumber_InvalidInput_Romanian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("pl")]
public class WordsToNumberTests_Polish
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("minus miliard pięćset jeden milionów tysiąc osiemset dziewięćdziesiąt dwa", -1501001892, null)]
    [InlineData("sto dwanaście", 112, null)]
    [InlineData("dwadzieścia dwa tysiące", 22000, null)]
    [InlineData("dwa miliardy sto czterdzieści siedem milionów czterysta osiemdziesiąt trzy tysiące sześćset czterdzieści siedem", 2147483647, null)]
    public void TryToNumber_ValidInput_Polish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("sl-SI")]
public class WordsToNumberTests_Slovenian
{
    [Theory]
    [InlineData("nič", 0, null)]
    [InlineData("sto dvaindvajset", 122, null)]
    [InlineData("tri tisoč petsto ena", 3501, null)]
    [InlineData("dva milijona sto dvaintrideset tisoč štiristo petnajst", 2132415, null)]
    [InlineData("minus sedemsto enainpetdeset milijonov šeststo triintrideset tisoč šeststo devetnajst", -751633619, null)]
    public void TryToNumber_ValidInput_Slovenian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("sr")]
public class WordsToNumberTests_SerbianCyrillic
{
    [Theory]
    [InlineData("нула", 0, null)]
    [InlineData("четрдесет три", 43, null)]
    [InlineData("петсто четрдесет седам", 547, null)]
    [InlineData("десет милиона", 10000000, null)]
    public void TryToNumber_ValidInput_SerbianCyrillic(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("sr-Latn")]
public class WordsToNumberTests_SerbianLatin
{
    [Theory]
    [InlineData("nula", 0, null)]
    [InlineData("četrdeset tri", 43, null)]
    [InlineData("petsto četrdeset sedam", 547, null)]
    [InlineData("deset miliona", 10000000, null)]
    public void TryToNumber_ValidInput_SerbianLatin(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("sk")]
public class WordsToNumberTests_Slovak
{
    [Theory]
    [InlineData("nula", 0, null)]
    [InlineData("mínus deväťdesiat deväť", -99, null)]
    [InlineData("sto jeden", 101, null)]
    [InlineData("dve miliardy sto štyridsať sedem miliónov štyristo osemdesiat tri tisíc šesťsto štyridsať sedem", 2147483647, null)]
    public void TryToNumber_ValidInput_Slovak(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("sto foo", 0, "foo")]
    public void TryToNumber_InvalidInput_Slovak(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("uk-UA")]
public class WordsToNumberTests_Ukrainian
{
    [Theory]
    [InlineData("нуль", 0, null)]
    [InlineData("мінус сто двадцять три", -123, null)]
    [InlineData("одна тисяча сто дванадцять", 1112, null)]
    [InlineData("сто двадцять одна тисяча триста чотирнадцять", 121314, null)]
    [InlineData("два мільйона сто тридцять дві тисячі чотириста п'ятнадцять", 2132415, null)]
    [InlineData("перший", 1, null)]
    [InlineData("третя", 3, null)]
    [InlineData("десяте", 10, null)]
    public void TryToNumber_ValidInput_Ukrainian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("nb-NO")]
public class WordsToNumberTests_Norwegian
{
    [Theory]
    [InlineData("null", 0, null)]
    [InlineData("minus ti", -10, null)]
    [InlineData("nittiåtte", 98, null)]
    [InlineData("hundreogelleve", 111, null)]
    [InlineData("tusenogen", 1001, null)]
    [InlineData("ettusenethundreogtolv", 1112, null)]
    [InlineData("en million", 1000000, null)]
    [InlineData("første", 1, null)]
    [InlineData("tjuende", 20, null)]
    public void TryToNumber_ValidInput_Norwegian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("tjue foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Norwegian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("tr-TR")]
public class WordsToNumberTests_Turkish
{
    [Theory]
    [InlineData("sıfır", 0, null)]
    [InlineData("eksi beş", -5, null)]
    [InlineData("yirmi iki", 22, null)]
    [InlineData("yüz on iki", 112, null)]
    [InlineData("üç bin beş yüz bir", 3501, null)]
    [InlineData("bir milyon bir", 1000001, null)]
    [InlineData("birinci", 1, null)]
    [InlineData("yüz yirminci", 120, null)]
    public void TryToNumber_ValidInput_Turkish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("yirmi foo", 0, "foo")]
    [InlineData("eksi xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Turkish(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("az")]
public class WordsToNumberTests_Azerbaijani
{
    [Theory]
    [InlineData("sıfır", 0, null)]
    [InlineData("mənfi beş", -5, null)]
    [InlineData("iyirmi iki", 22, null)]
    [InlineData("yüz on iki", 112, null)]
    [InlineData("üç min beş yüz bir", 3501, null)]
    [InlineData("bir milyon bir", 1000001, null)]
    [InlineData("birinci", 1, null)]
    [InlineData("yüz iyirminci", 120, null)]
    public void TryToNumber_ValidInput_Azerbaijani(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("iyirmi foo", 0, "foo")]
    [InlineData("mənfi xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Azerbaijani(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("zh-CN")]
public class WordsToNumberTests_Chinese
{
    [Theory]
    [InlineData("零", 0, null)]
    [InlineData("负 五", -5, null)]
    [InlineData("十三", 13, null)]
    [InlineData("一百", 100, null)]
    [InlineData("五百零七", 507, null)]
    [InlineData("二万零八十九", 20089, null)]
    [InlineData("第 一", 1, null)]
    [InlineData("第 三万一千二百三十四", 31234, null)]
    public void TryToNumber_ValidInput_Chinese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("二十foo", 0, "二")]
    [InlineData("负 xyz", 0, "x")]
    public void TryToNumber_InvalidInput_Chinese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("vi-VN")]
public class WordsToNumberTests_Vietnamese
{
    [Theory]
    [InlineData("không", 0, null)]
    [InlineData("trừ một", -1, null)]
    [InlineData("mười một", 11, null)]
    [InlineData("hai mươi mốt", 21, null)]
    [InlineData("một trăm linh năm", 105, null)]
    [InlineData("một nghìn", 1000, null)]
    [InlineData("một triệu", 1000000, null)]
    [InlineData("thứ nhất", 1, null)]
    [InlineData("thứ hai mươi mốt", 21, null)]
    public void TryToNumber_ValidInput_Vietnamese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("hai mươi foo", 0, "foo")]
    [InlineData("trừ xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Vietnamese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("ja-JP")]
public class WordsToNumberTests_Japanese
{
    [Theory]
    [InlineData("〇", 0, null)]
    [InlineData("十一", 11, null)]
    [InlineData("百二十二", 122, null)]
    [InlineData("三千五百一", 3501, null)]
    [InlineData("一億", 100000000, null)]
    [InlineData("一番目", 1, null)]
    [InlineData("百十二番目", 112, null)]
    public void TryToNumber_ValidInput_Japanese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("二十foo", 0, "二")]
    [InlineData("百xyz", 0, "百")]
    public void TryToNumber_InvalidInput_Japanese(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("ko-KR")]
public class WordsToNumberTests_Korean
{
    [Theory]
    [InlineData("영", 0, null)]
    [InlineData("십일", 11, null)]
    [InlineData("백이십이", 122, null)]
    [InlineData("삼천오백일", 3501, null)]
    [InlineData("일억", 100000000, null)]
    [InlineData("첫번째", 1, null)]
    [InlineData("백십이번째", 112, null)]
    public void TryToNumber_ValidInput_Korean(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("이십foo", 0, "이")]
    [InlineData("백xyz", 0, "백")]
    public void TryToNumber_InvalidInput_Korean(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("fil-PH")]
public class WordsToNumberTests_Filipino
{
    [Theory]
    [InlineData("sero", 0, null)]
    [InlineData("minus tatlo", -3, null)]
    [InlineData("labing-isa", 11, null)]
    [InlineData("dalawampu't isa", 21, null)]
    [InlineData("isang daan at lima", 105, null)]
    [InlineData("isang libo", 1000, null)]
    [InlineData("dalawang milyon", 2000000, null)]
    public void TryToNumber_ValidInput_Filipino(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("dalawampu foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Filipino(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

}

[UseCulture("id-ID")]
public class WordsToNumberTests_Indonesian
{
    [Theory]
    [InlineData("nol", 0, null)]
    [InlineData("minus satu", -1, null)]
    [InlineData("sebelas", 11, null)]
    [InlineData("dua puluh satu", 21, null)]
    [InlineData("seratus lima", 105, null)]
    [InlineData("seribu", 1000, null)]
    [InlineData("satu juta", 1000000, null)]
    public void TryToNumber_ValidInput_Indonesian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("dua puluh foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Indonesian(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

[UseCulture("ms-MY")]
public class WordsToNumberTests_Malay
{
    [Theory]
    [InlineData("kosong", 0, null)]
    [InlineData("minus satu", -1, null)]
    [InlineData("sebelas", 11, null)]
    [InlineData("dua puluh satu", 21, null)]
    [InlineData("seratus lima", 105, null)]
    [InlineData("seribu", 1000, null)]
    [InlineData("satu juta", 1000000, null)]
    public void TryToNumber_ValidInput_Malay(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("dua puluh foo", 0, "foo")]
    [InlineData("minus xyz", 0, "xyz")]
    public void TryToNumber_InvalidInput_Malay(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }
}

public class WordsToNumberTests_NonEnglish
{
    [Theory]
    [InlineData("ar", "عشرون")]
    public void ThrowsForNonEnglishWords(string cultureName, string word)
    {
        var culture = new CultureInfo(cultureName);
        var ex = Assert.Throws<NotSupportedException>(() =>
            word.ToNumber(culture));

        Assert.Contains($"'{culture.TwoLetterISOLanguageName}'", ex.Message);
    }
}
