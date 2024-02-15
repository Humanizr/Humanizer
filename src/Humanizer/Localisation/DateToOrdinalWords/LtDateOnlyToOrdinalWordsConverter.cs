#if NET6_0_OR_GREATER

namespace Humanizer
{
    internal class LtDateOnlyToOrdinalWordsConverter : IDateOnlyToOrdinalWordConverter
    {
        public string Convert(DateOnly date)
        {
            return date.ToString("yyyy 'm.' MMMM d 'd.'");
        }

        public string Convert(DateOnly date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }
    }
}
#endif
