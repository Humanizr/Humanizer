namespace Humanizer;

class PatternDateToOrdinalWordsConverter(OrdinalDatePattern pattern) : DefaultDateToOrdinalWordConverter
{
    readonly OrdinalDatePattern pattern = pattern;

    public override string Convert(DateTime date) =>
        pattern.Format(date);
}
