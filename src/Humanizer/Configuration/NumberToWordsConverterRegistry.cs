using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry()
            : base(() => new DefaultNumberToWordsConverter(),
                new Localiser<INumberToWordsConverter>("en", () => new EnglishNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("ar", () => new ArabicNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("fa", () => new FarsiNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("es", () => new SpanishNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("pl", () => new PolishNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("pt-BR", () => new BrazilianPortugueseNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("ru", () => new RussianNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("fr", () => new FrenchNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("nl", () => new DutchNumberToWordsConverter()),
                new Localiser<INumberToWordsConverter>("he", () => new HebrewNumberToWordsConverter()))
        {
        }
    }
}