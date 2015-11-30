using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry() : base(new DefaultOrdinalizer())
        {
            Register("de", new GermanOrdinalizer());
            Register("en", new EnglishOrdinalizer());
            Register("es", new SpanishOrdinalizer());
            Register("it", new ItalianOrdinalizer());
            Register("nl", new DutchOrdinalizer());
            Register("pt", new PortugueseOrdinalizer());
            Register("ro", new RomanianOrdinalizer());
            Register("ru", new RussianOrdinalizer());
            Register("tr", new TurkishOrdinalizer());
            Register("uk", new UkrainianOrdinalizer());
        }
    }
}
