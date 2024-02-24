namespace Humanizer;

class TurkishOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + ".";
}