namespace Humanizer.Localisation.NumberToWords
{
    internal class GermanSwissLiechtensteinNumberToWordsConverter : GermanNumberToWordsConverterBase
    {
        protected override string GetTens(long tens)
        {
            if (tens == 3)
            {
                return "dreissig";
            }

            return base.GetTens(tens);
        }
    }
}
