using System;

namespace Humanizer.Localisation.DateToWords
{
    internal class EsDateToWordsConverter : DefaultDateToWordConverter
    {
        public override string Convert(DateTime date)
        {
            return date.Day.ToWords() + date.ToString(" 'de' MMMM 'de' ") + date.Year.ToWords();
        }
    }
}
