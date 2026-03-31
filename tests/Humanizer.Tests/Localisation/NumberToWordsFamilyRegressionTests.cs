public class NumberToWordsFamilyRegressionTests
{
    [Theory]
    [InlineData(81, "fr-BE", "quatre-vingt-un")]
    [InlineData(80, "fr-CH", "octante")]
    [InlineData(30, "de-CH", "dreissig")]
    [InlineData(30, "de-LI", "dreissig")]
    [InlineData(100, "ms-MY", "seratus")]
    [InlineData(80, "id", "delapan puluh")]
    [InlineData(1000000000, "uz-Latn-UZ", "bir milliard")]
    [InlineData(1000000000, "uz-Cyrl-UZ", "бир миллиард")]
    [InlineData(2000000000, "sr-Latn", "dve milijarde")]
    [InlineData(2000000000, "sr", "две милијарде")]
    [InlineData(1000000000, "pt", "mil milhões")]
    [InlineData(1000000000, "pt-BR", "um bilhão")]
    [InlineData(1001, "en-US", "one thousand and one")]
    [InlineData(1001, "en-IN", "one thousand one")]
    [InlineData(1000, "cs-CZ", "jeden tisíc")]
    [InlineData(1000, "sk", "tisíc")]
    [InlineData(12345, "da", "tolv tusind tre hundrede og femogfyrre")]
    [InlineData(1234567890, "nl-NL", "een miljard tweehonderdvierendertig miljoen vijfhonderdzevenenzestigduizend achthonderdnegentig")]
    [InlineData(1234567890, "is", "einn milljarður tvö hundruð þrjátíu og fjórar milljónir fimm hundruð sextíu og sjö þúsund átta hundruð og níutíu")]
    [InlineData(1000010, "nb-NO", "en million og ti")]
    [InlineData(11213, "sv-SE", "elva tusen tvåhundratretton")]
    [InlineData(2000, "pl", "dwa tysiące")]
    [InlineData(2000, "lt", "du tūkstančiai")]
    [InlineData(751633617, "hy", "յոթ հարյուր հիսունմեկ միլիոն վեց հարյուր երեսուներեք հազար վեց հարյուր տասնյոթ")]
    [InlineData(1111111111, "th-TH", "หนึ่งพันหนึ่งร้อยสิบเอ็ดล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด")]
    [InlineData(1234567890, "fa", "یک میلیارد و دویست و سی و چهار میلیون و پانصد و شصت و هفت هزار و هشتصد و نود")]
    [InlineData(123456789, "ku", "سەد و بیست و سێ میلیۆن و چوار سەد و پەنجا و شەش هەزار و حەوت سەد و هەشتا و نۆ")]
    [InlineData(1234567890, "bn-BD", "একশ তেইশ কোটি পঁয়তাল্লিশ লক্ষ সাতষট্টি হাজার আটশ নব্বই")]
    public void FamilySharedConvertersPreserveCardinalOutputs(int number, string culture, string expected) =>
        Assert.Equal(expected, number.ToWords(new CultureInfo(culture)));

    [Theory]
    [InlineData(3501, "en-US", false, "three thousand five hundred one")]
    [InlineData(3501, "en-IN", false, "three thousand five hundred and one")]
    public void EnglishFamilyConvertersPreserveCultureSpecificAndBehavior(int number, string culture, bool addAnd, string expected) =>
        Assert.Equal(expected, number.ToWords(addAnd, new CultureInfo(culture)));

    [Theory]
    [InlineData(1000000000, "uz-Latn-UZ", "bir milliardinchi")]
    [InlineData(1000000000, "uz-Cyrl-UZ", "бир миллиардинчи")]
    [InlineData(1000000000, "pt", "milésimo milionésimo")]
    [InlineData(2000000, "pt", "segundo milionésimo")]
    [InlineData(1000000000, "pt-BR", "bilionésimo")]
    [InlineData(2000000, "pt-BR", "segundomilionésimo")]
    [InlineData(1021, "en-US", "thousand and twenty-first")]
    [InlineData(1021, "en-IN", "one thousand twenty one")]
    [InlineData(21, "cs-CZ", "21")]
    [InlineData(21, "sk", "21")]
    [InlineData(95, "nl-NL", "vijfennegentigste")]
    [InlineData(1021, "is", "eitt þúsund tuttugasti og fyrsti")]
    [InlineData(1000010, "nb-NO", "en million og tiende")]
    [InlineData(1000000, "sv-SE", "en miljonte")]
    [InlineData(21, "sv-SE", "tjugoförsta")]
    [InlineData(30, "nb-NO", "trettiende")]
    [InlineData(21, "pl", "21")]
    [InlineData(2000, "lt", "du tūkstantas")]
    [InlineData(121000, "hy", "հարյուր քսանմեկ հազարերորդ")]
    [InlineData(1333, "fa", "یک هزار و سیصد و سی و سوم")]
    [InlineData(1333, "ku", "هەزار و سێ سەد و سی و سێیەم")]
    [InlineData(100000, "bn-BD", "লক্ষ তম")]
    public void FamilySharedConvertersPreserveOrdinalOutputs(int number, string culture, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(new CultureInfo(culture)));

    [Theory]
    [InlineData(2, "tr", "çift")]
    [InlineData(3, "tr", "üçlü")]
    public void FamilySharedConvertersPreserveTupleOutputs(int number, string culture, string expected) =>
        Assert.Equal(expected, number.ToTuple(new CultureInfo(culture)));

    [Theory]
    [InlineData("seratus", "ms-MY", 100)]
    [InlineData("satu miliar", "id", 1000000000)]
    public void FamilySharedWordsToNumberConvertersPreserveParsing(string words, string culture, int expected) =>
        Assert.Equal(expected, words.ToNumber(new CultureInfo(culture)));
}
