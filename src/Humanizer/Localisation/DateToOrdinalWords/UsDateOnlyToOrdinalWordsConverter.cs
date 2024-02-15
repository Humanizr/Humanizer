#if NET6_0_OR_GREATER

namespace Humanizer
{
    internal class UsDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
    {
        public override string Convert(DateOnly date)
        {
            return date.ToString("MMMM ") + date.Day.Ordinalize() + date.ToString(", yyyy");
        }
    }
}
#endif
