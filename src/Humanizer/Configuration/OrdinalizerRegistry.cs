namespace Humanizer;

class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
{
    public OrdinalizerRegistry()
        : base(_ => new DefaultOrdinalizer())
    {
        Register("de", _ => new GermanOrdinalizer());
        Register("en", _ => new EnglishOrdinalizer());
        Register("es", c => new SpanishOrdinalizer(c));
        Register("fr", _ => new FrenchOrdinalizer());
        Register("is", _ => new IcelandicOrdinalizer());
        Register("it", _ => new ItalianOrdinalizer());
        Register("nl", _ => new DutchOrdinalizer());
        Register("pt", _ => new PortugueseOrdinalizer());
        Register("ro", _ => new RomanianOrdinalizer());
        Register("ru", _ => new RussianOrdinalizer());
        Register("tr", _ => new TurkishOrdinalizer());
        Register("uk", _ => new UkrainianOrdinalizer());
        Register("hy", _ => new ArmenianOrdinalizer());
        Register("az", _ => new AzerbaijaniOrdinalizer());
        Register("lb", _ => new LuxembourgishOrdinalizer());
        Register("hu", _ => new HungarianOrdinalizer());
        Register("ca", _ => new CatalanOrdinalizer());
    }
}