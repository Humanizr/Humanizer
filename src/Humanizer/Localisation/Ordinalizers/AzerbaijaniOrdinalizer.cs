namespace Humanizer;

class AzerbaijaniOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + ".";
}