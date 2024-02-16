namespace Humanizer;

class LuxembourgishOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + ".";
}
