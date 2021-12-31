#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.DateToOrdinalWords
{
    internal class EsDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
    {
        public override string Convert(DateOnly date)
        {
            return date.ToString("d 'de' MMMM 'de' yyyy");
        }
    }
}

#endif
