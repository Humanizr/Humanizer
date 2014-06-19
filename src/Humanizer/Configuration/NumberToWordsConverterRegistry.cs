using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry() : base(new DefaultNumberToWordsConverter())
        {
            Register("en", new EnglishNumberToWordsConverter());
            Register("ar", new ArabicNumberToWordsConverter());
            Register("fa", new FarsiNumberToWordsConverter());
            Register("es", new SpanishNumberToWordsConverter());
            Register("pl", new PolishNumberToWordsConverter());
            Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            Register("ru", new RussianNumberToWordsConverter());
            Register("fr", new FrenchNumberToWordsConverter());
            Register("nl", new DutchNumberToWordsConverter());
            Register("he", new HebrewNumberToWordsConverter());
            Register("sl", new SlovenianNumberToWordsConverter());
            Register("de", new GermanNumberToWordsConverter());
            Register("bn-BD", new BanglaNumberToWordsConverter());
        }
    }
}
