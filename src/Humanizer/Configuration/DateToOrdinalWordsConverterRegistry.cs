using Humanizer.Localisation.DateToOrdinalWords;
namespace Humanizer.Configuration
{
    internal class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
    {
        public DateToOrdinalWordsConverterRegistry() : base(new DefaultDateToOrdinalWordConverter())
        {
            Register("en-UK", new DefaultDateToOrdinalWordConverter());
            Register("de", new DefaultDateToOrdinalWordConverter());
            Register("en-US", new UsDateToOrdinalWordsConverter());
        }
    }
}
