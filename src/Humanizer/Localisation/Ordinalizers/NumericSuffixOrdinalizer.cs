namespace Humanizer;

class NumericSuffixOrdinalizer(string suffix) : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + suffix;
}
