namespace Humanizer;

class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
{
    public DateToOrdinalWordsConverterRegistry()
        : base(_ => new DefaultDateToOrdinalWordConverter())
        => DateToOrdinalWordsConverterRegistryRegistrations.Register(this);
}