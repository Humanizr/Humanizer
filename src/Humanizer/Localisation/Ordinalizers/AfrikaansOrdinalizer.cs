namespace Humanizer;

class AfrikaansOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString)
    {
        var suffix = number switch
        {
            1 => "ste",
            2 => "de",
            3 => "de",
            8 => "ste",
            _ when Math.Abs(number) >= 20 => "ste",
            _ => "de"
        };

        return numberString + suffix;
    }
}
