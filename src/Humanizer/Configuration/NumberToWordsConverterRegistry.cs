using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry()
            : base((culture) => new DefaultNumberToWordsConverter(culture))
        {
            Register("af", new AfrikaansNumberToWordsConverter());
            Register("en", new EnglishNumberToWordsConverter());
            Register("ar", new ArabicNumberToWordsConverter());
            Register("fa", new FarsiNumberToWordsConverter());
            Register("eo", new EsperantoNumberToWordsConverter());
            Register("es", new SpanishNumberToWordsConverter ());
            Register("pl", (culture) => new PolishNumberToWordsConverter(culture));
            Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            Register("ro", new RomanianNumberToWordsConverter());
            Register("ru", new RussianNumberToWordsConverter());
            Register("fi", new FinnishNumberToWordsConverter());
            Register("fr-BE", new FrenchBelgianNumberToWordsConverter());
            Register("fr-CH", new FrenchSwissNumberToWordsConverter());
            Register("fr", new FrenchNumberToWordsConverter());
            Register("nl", new DutchNumberToWordsConverter());
            Register("he", (culture) => new HebrewNumberToWordsConverter(culture));
            Register("sl", (culture) => new SlovenianNumberToWordsConverter(culture));
            Register("de", new GermanNumberToWordsConverter());
            Register("bn-BD", new BanglaNumberToWordsConverter());
            Register("tr", new TurkishNumberToWordConverter());
            Register("it", new ItalianNumberToWordsConverter());
            Register("uk", new UkrainianNumberToWordsConverter());
            Register("uz-Latn-UZ", new UzbekLatnNumberToWordConverter());
            Register("uz-Cyrl-UZ", new UzbekCyrlNumberToWordConverter());
            Register("sr", (culture) => new SerbianCyrlNumberToWordsConverter(culture));
            Register("sr-Latn", (culture) => new SerbianNumberToWordsConverter(culture));
            Register("nb", new NorwegianBokmalNumberToWordsConverter());
        }
    }
}
