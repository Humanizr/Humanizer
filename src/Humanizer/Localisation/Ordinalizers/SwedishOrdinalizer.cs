namespace Humanizer;

class SwedishOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString)
    {
        var lastTwoDigits = Math.Abs(number % 100);
        if (lastTwoDigits is >= 11 and <= 19)
        {
            return numberString + ":e";
        }

        var lastDigit = Math.Abs(number % 10);
        return numberString + (lastDigit is 1 or 2 ? ":a" : ":e");
    }
}
