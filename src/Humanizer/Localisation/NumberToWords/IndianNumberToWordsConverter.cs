namespace Humanizer;

class IndianNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] Tillnineteen =
    [
        "one", "two", "three", "four", "five", "six", "seven", "eight",
        "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
        "seventeen", "eighteen", "nineteen"
    ];

    static readonly string[] Tens =
    [
        "twenty", "thirty", "forty", "fifty", "sixty", "seventy",
        "eighty", "ninety"
    ];

    public override string Convert(long number) =>
        NumberToText(number)
            .Trim();

    public override string ConvertToOrdinal(int number)
    {
        var result = NumberToText(number)
            .Trim();
        return result;
    }

    static string NumberToText(long number)
    {
        if (number < 0)
        {
            return "(Negative) " + NumberToText(-number);
        }

        if (number == 0)
        {
            return "";
        }

        if (number <= 19)
        {
            return Tillnineteen[number - 1] + " ";
        }

        if (number <= 99)
        {
            return Tens[number / 10 - 2] + " " + NumberToText(number % 10);
        }

        if (number <= 199)
        {
            return ("one hundred " + (number % 100 > 0 ? "and " : "") + NumberToText(number % 100)).Trim();
        }

        if (number <= 999)
        {
            return NumberToText(number / 100) + "hundred " + (number % 100 > 0 ? "and " : "") + NumberToText(number % 100);
        }

        if (number <= 1999)
        {
            return "one thousand " + NumberToText(number % 1000);
        }

        if (number <= 99999)
        {
            return NumberToText(number / 1000) + "thousand " + NumberToText(number % 1000);
        }

        if (number <= 199999)
        {
            return ("one lakh " + NumberToText(number % 100000)).Trim();
        }

        if (number <= 9999999)
        {
            return NumberToText(number / 100000) + "lakh " + NumberToText(number % 100000);
        }

        if (number <= 19999999)
        {
            return "one crore " + NumberToText(number % 10000000);
        }

        return NumberToText(number / 10000000)
            .Trim() + " crore " + NumberToText(number % 10000000);
    }
}