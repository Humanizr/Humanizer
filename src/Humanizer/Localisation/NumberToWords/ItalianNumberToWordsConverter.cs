namespace Humanizer;

class ItalianNumberToWordsConverter : GenderedNumberToWordsConverter
{
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;

        if (number < 0)
        {
            return "meno " + Convert(Math.Abs(number), gender);
        }

        var cruncher = new ItalianCardinalNumberCruncher(number, gender);

        return cruncher.Convert();
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        var cruncher = new ItalianOrdinalNumberCruncher(number, gender);

        return cruncher.Convert();
    }
}