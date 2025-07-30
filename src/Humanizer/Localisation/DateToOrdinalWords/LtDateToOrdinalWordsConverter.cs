namespace Humanizer;

class LtDateToOrdinalWordsConverter : IDateToOrdinalWordConverter
{
    public string Convert(DateTime date) =>
        date.ToString("yyyy 'm.' MMMM d 'd.'");

    public string Convert(DateTime date, GrammaticalCase grammaticalCase) =>
        Convert(date);
}