#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.DateToWords
{
    internal class DefaultDateOnlyToWordConverter : IDateOnlyToWordConverter
    {

        public virtual string Convert(DateOnly date)
        {
            return "the " + date.Day.ToOrdinalWords() + date.ToString(" 'of' MMMM ") + date.Year.ToWords();
        }

        public virtual string Convert(DateOnly date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }

    }
}

#endif
