using System;

using Humanizer.Localisation.DateToWords;

namespace Humanizer.Localisation.DateToWords
{
    internal class UsDateToWordsConverter : DefaultDateToWordConverter
    {
        public override string Convert(DateTime date)
        {
            return date.ToString("MMMM ") + date.Day.ToOrdinalWords() + ", " + date.Year.ToWords();
        }
    }
}
