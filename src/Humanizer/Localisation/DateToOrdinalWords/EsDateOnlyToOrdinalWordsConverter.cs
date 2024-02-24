﻿#if NET6_0_OR_GREATER

namespace Humanizer;

class EsDateOnlyToOrdinalWordsConverter : DefaultDateOnlyToOrdinalWordConverter
{
    public override string Convert(DateOnly date)
    {
        var equivalentDateTime = date.ToDateTime(TimeOnly.MinValue);
        return Configurator.DateToOrdinalWordsConverter.Convert(equivalentDateTime);
    }
}

#endif
