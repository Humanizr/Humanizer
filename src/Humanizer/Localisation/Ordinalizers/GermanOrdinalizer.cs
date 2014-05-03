namespace Humanizer.Localisation.Ordinalizers
{
    internal class GermanOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return numberString + ".";
        }
    }
}