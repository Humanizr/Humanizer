using System;

namespace Humanizer.Localisation.DateToWords
{
    internal class DefaultDateToWordConverter : IDateToWordConverter
    {

        public virtual string Convert(DateTime date)
        {
            return "the " + date.Day.ToOrdinalWords() + date.ToString(" 'of' MMMM ") + date.Year.ToWords();
        }

        public virtual string Convert(DateTime date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }

    }
}
