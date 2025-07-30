#if NET6_0_OR_GREATER
namespace Humanizer;

class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
{
    public DateOnlyToOrdinalWordsConverterRegistry() : base(new DefaultDateOnlyToOrdinalWordConverter())
    {
        Register("en-US", new UsDateOnlyToOrdinalWordsConverter());
        Register("fr", new FrDateOnlyToOrdinalWordsConverter());
        Register("es", new EsDateOnlyToOrdinalWordsConverter());
        Register("lt", new LtDateOnlyToOrdinalWordsConverter());
    }
}
#endif
