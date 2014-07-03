using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry() : base(new DefaultOrdinalizer())
        {
            Register("en", new EnglishOrdinalizer());
            Register("es", new SpanishOrdinalizer());
            Register("ru", new RussianOrdinalizer());
            Register("pt-BR", new BrazilianPortugueseOrdinalizer());
            Register("de", new GermanOrdinalizer());
            Register("tr", new TurkishOrdinalizer());
        }
    }
}