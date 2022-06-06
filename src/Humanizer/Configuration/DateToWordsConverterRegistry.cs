using Humanizer.Localisation.DateToWords;

namespace Humanizer.Configuration
{
    internal class DateToWordsConverterRegistry : LocaliserRegistry<IDateToWordConverter>
    {
        public DateToWordsConverterRegistry() : base(new DefaultDateToWordConverter())
        {
            Register("en-US", new UsDateToWordsConverter());
            Register("es", new EsDateToWordsConverter());
        }
    }
}
