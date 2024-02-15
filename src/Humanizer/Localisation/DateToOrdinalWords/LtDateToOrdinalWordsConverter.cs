namespace Humanizer
{
    internal class LtDateToOrdinalWordsConverter : IDateToOrdinalWordConverter
    {
        public string Convert(DateTime date)
        {
            return date.ToString("yyyy 'm.' MMMM d 'd.'");
        }

        public string Convert(DateTime date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }
    }
}
