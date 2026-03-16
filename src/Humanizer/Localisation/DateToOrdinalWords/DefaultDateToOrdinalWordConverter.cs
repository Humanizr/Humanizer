namespace Humanizer;

class DefaultDateToOrdinalWordConverter : IDateToOrdinalWordConverter
{
    public virtual string Convert(DateTime date) =>
        date.Day.Ordinalize() + date.ToString(" MMMM yyyy");

    public virtual string Convert(DateTime date, GrammaticalCase grammaticalCase) =>
        Convert(date);
}