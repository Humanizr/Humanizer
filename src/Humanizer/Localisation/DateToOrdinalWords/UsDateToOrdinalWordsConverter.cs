namespace Humanizer;

class UsDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
    public override string Convert(DateTime date) =>
        date.ToString("MMMM ") + date.Day.Ordinalize() + date.ToString(", yyyy");
}