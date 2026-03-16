namespace Humanizer;

class LongDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
    public override string Convert(DateTime date) =>
        date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
}
