namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(_ => NumberToWordsProfileCatalog.Resolve("english", CultureInfo.InvariantCulture))
        => NumberToWordsConverterRegistryRegistrations.Register(this);
}
