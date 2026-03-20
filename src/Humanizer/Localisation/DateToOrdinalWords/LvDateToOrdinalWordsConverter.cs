namespace Humanizer;

class LvDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
    public override string Convert(DateTime date) =>
        $"{date.Day}. {date:MMMM yyyy}";
}
