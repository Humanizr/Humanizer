using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry()
            : base(() => new DefaultOrdinalizer(),
                new Localiser<IOrdinalizer>("en", () => new EnglishOrdinalizer()),
                new Localiser<IOrdinalizer>("es", () => new SpanishOrdinalizer()),
                new Localiser<IOrdinalizer>("pt-BR", () => new BrazilianPortugueseOrdinalizer()),
                new Localiser<IOrdinalizer>("ru", () => new RussianOrdinalizer()))
        {
        }
    }
}
