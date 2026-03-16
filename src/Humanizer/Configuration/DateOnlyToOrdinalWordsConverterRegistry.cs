#if NET6_0_OR_GREATER
namespace Humanizer;

class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
{
    public DateOnlyToOrdinalWordsConverterRegistry() : base(_ => new DefaultDateOnlyToOrdinalWordConverter())
    {
        Register("en-US", _ => new UsDateOnlyToOrdinalWordsConverter());
        Register("fr", _ => new FrDateOnlyToOrdinalWordsConverter());
        Register("es", _ => new EsDateOnlyToOrdinalWordsConverter());
        Register("lt", _ => new LtDateOnlyToOrdinalWordsConverter());
        Register("ca", _ => new CaDateOnlyToOrdinalWordsConverter());
    }
}
#endif
