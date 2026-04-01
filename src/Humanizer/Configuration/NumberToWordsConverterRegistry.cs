namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(_ => NumberToWordsProfileCatalog.Resolve("en", CultureInfo.InvariantCulture))
        => NumberToWordsConverterRegistryRegistrations.Register(this);
}
