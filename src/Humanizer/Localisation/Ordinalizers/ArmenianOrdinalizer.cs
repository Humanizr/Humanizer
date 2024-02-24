namespace Humanizer;

class ArmenianOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString)
    {
        if (number is 1 or -1)
        {
            return numberString + "-ին";
        }

        return numberString + "-րդ";
    }
}