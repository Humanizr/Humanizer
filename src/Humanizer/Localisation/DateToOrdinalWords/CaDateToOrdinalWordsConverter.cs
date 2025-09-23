namespace Humanizer;

class CaDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
{
    public override string Convert(DateTime date) =>
        date.ToString("d MMMM 'de' yyyy");
}