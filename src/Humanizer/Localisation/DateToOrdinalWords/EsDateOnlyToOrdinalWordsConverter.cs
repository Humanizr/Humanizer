#if NET6_0_OR_GREATER

using System;

using Humanizer.Configuration;

namespace Humanizer.Localisation.DateToOrdinalWords
{
    internal class EsDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
    {
        public override string Convert(DateOnly date)
        {
            var equivalentDateTime = date.ToDateTime(TimeOnly.MinValue);
            return Configurator.DateToOrdinalWordsConverter.Convert(equivalentDateTime);
        }
    }
}

#endif
