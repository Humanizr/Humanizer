#if NET6_0_OR_GREATER
namespace Humanizer;

class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
{
    public DateOnlyToOrdinalWordsConverterRegistry() : base(_ => new DefaultDateOnlyToOrdinalWordConverter()) =>
        DateOnlyToOrdinalWordsConverterRegistryRegistrations.Register(this);
}
#endif