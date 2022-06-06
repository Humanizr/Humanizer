#if NET6_0_OR_GREATER

using System;
using Humanizer.Configuration;

namespace Humanizer.Localisation.DateToWords
{
    internal class EsDateOnlyToWordsConverter : DefaultDateOnlyToWordConverter
    {
        public override string Convert(DateOnly date)
        {
            var equivalentDateTime = date.ToDateTime(TimeOnly.MinValue);
            return Configurator.DateToWordsConverter.Convert(equivalentDateTime);
        }
    }
}

#endif
