namespace Humanizer;

class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
{
    public DateToOrdinalWordsConverterRegistry()
        : base(new DefaultDateToOrdinalWordConverter())
    {
        Register("en-US", new UsDateToOrdinalWordsConverter());
        Register("fr", new FrDateToOrdinalWordsConverter());
        Register("es", new EsDateToOrdinalWordsConverter());
        Register("lt", new LtDateToOrdinalWordsConverter());
    }
}