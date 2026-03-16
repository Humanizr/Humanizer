#if NET6_0_OR_GREATER

namespace Humanizer;

class LongDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
{
    public override string Convert(DateOnly date) =>
        date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
}
#endif
