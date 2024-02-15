namespace Humanizer.Localisation.Ordinalizers;

internal class LuxembourgishOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString)
    {
        return numberString + ".";
    }
}
