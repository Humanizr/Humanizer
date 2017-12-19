using System;

namespace Humanizer.Localisation.DateToOrdinalWords
{
    internal class UsDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
    {
        public override string Convert(DateTime date)
        {
            return date.ToString("MMMM ") + date.Day.Ordinalize() + date.ToString(", yyyy");
        }
    }
}