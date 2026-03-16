#if NET6_0_OR_GREATER

namespace Humanizer;

class LvDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
{
    public override string Convert(DateOnly date) =>
        $"{date.Day}. {date:MMMM yyyy}";
}
#endif
