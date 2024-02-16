namespace Humanizer
{
    internal class EsDateToOrdinalWordsConverter : DefaultDateToOrdinalWordConverter
    {
        public override string Convert(DateTime date) =>
            date.ToString("d 'de' MMMM 'de' yyyy");
    }
}
