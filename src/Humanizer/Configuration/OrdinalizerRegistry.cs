using Humanizer.Localisation;
using Humanizer.Localisation.BrazilianPortuguese;
using Humanizer.Localisation.English;
using Humanizer.Localisation.Russian;
using Humanizer.Localisation.Spanish;

namespace Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry() : base(new DefaultOrdinalizer())
        {
            Register<EnglishOrdinalizer>("en");
            Register<SpanishOrdinalizer>("es");
            Register<RussianOrdinalizer>("ru");
            Register<BrazilianPortugueseOrdinalizer>("pt-BR");
        }
    }
}