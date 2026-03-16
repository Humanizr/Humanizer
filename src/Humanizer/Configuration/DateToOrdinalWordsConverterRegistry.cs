namespace Humanizer;

class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
{
    public DateToOrdinalWordsConverterRegistry()
        : base(_ => new DefaultDateToOrdinalWordConverter())
    {
        Register("en-US", _ => new UsDateToOrdinalWordsConverter());
        Register("fr", _ => new FrDateToOrdinalWordsConverter());
        Register("es", _ => new EsDateToOrdinalWordsConverter());
        Register("lt", _ => new LtDateToOrdinalWordsConverter());
        Register("ca", _ => new CaDateToOrdinalWordsConverter());
    }
}