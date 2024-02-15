﻿namespace Humanizer
{
    internal class EsDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
    {
        public override string Convert(DateTime date)
        {
            return date.ToString("d 'de' MMMM 'de' yyyy");
        }
    }
}
