namespace Humanizer;

class AzerbaijaniOrdinalizer : DefaultOrdinalizer
{
    public override string Convert(int number, string numberString) =>
        numberString + SelectSuffix(number);

    static string SelectSuffix(int number)
    {
        var lastDigit = Math.Abs(number % 10);
        var lastTwoDigits = Math.Abs(number % 100);

        if (lastTwoDigits is 10 or 30 or 60 or 90 || lastDigit is 0 or 3 or 4)
        {
            return "-cü";
        }

        if (lastDigit is 1 or 2 or 5 or 7 or 8)
        {
            return "-ci";
        }

        return lastDigit is 6 ? "-cı" : "-cu";
    }
}
