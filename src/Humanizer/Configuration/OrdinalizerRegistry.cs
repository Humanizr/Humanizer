using Humanizer.Localisation.Ordinalizers;

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
            Register<GermanOrdinalizer>("de");
        }
    }
}