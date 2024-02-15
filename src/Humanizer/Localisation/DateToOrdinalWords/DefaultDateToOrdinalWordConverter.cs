namespace Humanizer
{
    internal class DefaultDateToOrdinalWordConverter : IDateToOrdinalWordConverter
    {
        public virtual string Convert(DateTime date)
        {
            return date.Day.Ordinalize() + date.ToString(" MMMM yyyy");
        }

        public virtual string Convert(DateTime date, GrammaticalCase grammaticalCase)
        {
            return Convert(date);
        }
    }
}
