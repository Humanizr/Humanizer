#if NET6_0_OR_GREATER

namespace Humanizer;

class DefaultDateOnlyToOrdinalWordConverter : IDateOnlyToOrdinalWordConverter
{
    public virtual string Convert(DateOnly date) =>
        date.Day.Ordinalize() + date.ToString(" MMMM yyyy");

    public virtual string Convert(DateOnly date, GrammaticalCase grammaticalCase) =>
        Convert(date);
}

#endif
