using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry() : base((culture) => new DefaultNumberToWordsConverter(culture))
        {
            Register("en", new EnglishNumberToWordsConverter());
            Register("ar", new ArabicNumberToWordsConverter());
            Register("fa", new FarsiNumberToWordsConverter());
            Register("es", new SpanishNumberToWordsConverter());
            Register("pl", (culture) => new PolishNumberToWordsConverter(culture));
            Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            Register("ru", new RussianNumberToWordsConverter());
            Register("fr", new FrenchNumberToWordsConverter());
            Register("nl", new DutchNumberToWordsConverter());
            Register("he", (culture) => new HebrewNumberToWordsConverter(culture));
            Register("sl", (culture) => new SlovenianNumberToWordsConverter(culture));
            Register("de", new GermanNumberToWordsConverter());
            Register("bn-BD", new BanglaNumberToWordsConverter());
            Register("tr", new TurkishNumberToWordConverter());
            Register("it", new ItalianNumberToWordsConverter());
            Register("uz-Latn-UZ", new UzbekLatnNumberToWordConverter());
            Register("uz-Cyrl-UZ", new UzbekCyrlNumberToWordConverter());
        }
    }
}
