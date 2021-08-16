namespace Humanizer.Localisation.Ordinalizers
{
    internal class IcelandicOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return numberString + ".";
        }
    }
}
