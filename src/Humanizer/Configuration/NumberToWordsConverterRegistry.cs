using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry() : base(new DefaultNumberToWordsConverter())
        {
            Register<EnglishNumberToWordsConverter>("en");
            Register<ArabicNumberToWordsConverter>("ar");
            Register<FarsiNumberToWordsConverter>("fa");
            Register<SpanishNumberToWordsConverter>("es");
            Register<PolishNumberToWordsConverter>("pl");
            Register<BrazilianPortugueseNumberToWordsConverter>("pt-BR");
            Register<RussianNumberToWordsConverter>("ru");
            Register<FrenchNumberToWordsConverter>("fr");
            Register<DutchNumberToWordsConverter>("nl");
            Register<HebrewNumberToWordsConverter>("he");
            Register<SlovenianNumberToWordsConverter>("sl");
            Register<GermanNumberToWordsConverter>("de");
        }
    }
}