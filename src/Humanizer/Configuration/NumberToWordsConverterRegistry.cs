using Humanizer.Localisation;
using Humanizer.Localisation.Arabic;
using Humanizer.Localisation.BrazilianPortuguese;
using Humanizer.Localisation.Dutch;
using Humanizer.Localisation.English;
using Humanizer.Localisation.Farsi;
using Humanizer.Localisation.French;
using Humanizer.Localisation.German;
using Humanizer.Localisation.Hebrew;
using Humanizer.Localisation.Polish;
using Humanizer.Localisation.Russian;
using Humanizer.Localisation.Slovenian;
using Humanizer.Localisation.Spanish;

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