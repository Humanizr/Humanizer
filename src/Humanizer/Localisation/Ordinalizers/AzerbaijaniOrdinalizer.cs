namespace Humanizer.Localisation.Ordinalizers
{
    internal class AzerbaijaniOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return numberString + ".";
        }
    }
}
