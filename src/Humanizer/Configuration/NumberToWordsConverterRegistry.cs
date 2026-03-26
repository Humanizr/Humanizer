namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(_ => new EnglishNumberToWordsConverter())
        => NumberToWordsConverterRegistryRegistrations.Register(this);
}
