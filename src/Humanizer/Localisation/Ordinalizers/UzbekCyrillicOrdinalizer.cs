namespace Humanizer;

class UzbekCyrillicOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + "-чи";
}
