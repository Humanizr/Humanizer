#if NET6_0_OR_GREATER
using Humanizer.Localisation.DateToOrdinalWords;

namespace Humanizer.Configuration
{
    internal class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
    {
        public DateOnlyToOrdinalWordsConverterRegistry() : base(new DefaultDateOnlyToOrdinalWordConverter())
        {
            Register("en-US", new UsDateOnlyToOrdinalWordsConverter());
            Register("fr", new FrDateOnlyToOrdinalWordsConverter());
            Register("es", new EsDateOnlyToOrdinalWordsConverter());
        }
    }
}
#endif
