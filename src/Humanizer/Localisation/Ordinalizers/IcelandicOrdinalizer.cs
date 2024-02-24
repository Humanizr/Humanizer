namespace Humanizer;

class IcelandicOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + ".";
}