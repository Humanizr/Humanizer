#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.DateToOrdinalWords
{
    internal class DefaultDateOnlyToOrdinalWordConverter : IDateOnlyToOrdinalWordConverter
    {

        public virtual string Convert(DateOnly date)
        {
            return date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
        }

        public virtual string Convert(DateOnly date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }

    }
}

#endif
