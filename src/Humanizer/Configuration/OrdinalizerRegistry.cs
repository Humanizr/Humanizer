using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry()
        {
            RegisterDefault<DefaultOrdinalizer>();
            Register<EnglishOrdinalizer>("en");
            Register<SpanishOrdinalizer>("es");
            Register<RussianOrdinalizer>("ru");
            Register<BrazilianPortugueseOrdinalizer>("pt-BR");
        }
    }
}