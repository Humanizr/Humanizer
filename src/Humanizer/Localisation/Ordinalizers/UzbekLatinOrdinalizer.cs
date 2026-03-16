namespace Humanizer;

class UzbekLatinOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + "-chi";
}
