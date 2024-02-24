#if NET6_0_OR_GREATER

namespace Humanizer;

class LtDateOnlyToOrdinalWordsConverter : IDateOnlyToOrdinalWordConverter
{
    public string Convert(DateOnly date) =>
        date.ToString("yyyy 'm.' MMMM d 'd.'");

    public string Convert(DateOnly date, GrammaticalCase grammaticalCase) =>
        Convert(date);
}
#endif
