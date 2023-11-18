#if NET6_0_OR_GREATER
using System;

namespace Humanizer.Localisation.DateToOrdinalWords
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
