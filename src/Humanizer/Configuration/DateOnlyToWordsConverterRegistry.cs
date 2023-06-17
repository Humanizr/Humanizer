#if NET6_0_OR_GREATER
using Humanizer.Localisation.DateToWords;

namespace Humanizer.Configuration
{
    internal class DateOnlyToWordsConverterRegistry : LocaliserRegistry<IDateOnlyToWordConverter>
    {
        public DateOnlyToWordsConverterRegistry() : base(new DefaultDateOnlyToWordConverter())
        {
            Register("en-US", new UsDateOnlyToWordsConverter());
            Register("es", new EsDateOnlyToWordsConverter());
        }
    }
}
#endif
