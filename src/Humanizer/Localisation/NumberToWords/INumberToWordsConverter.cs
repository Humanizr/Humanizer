using System;
namespace Humanizer.Localisation.NumberToWords
{
    public interface INumberToWordsConverter
    {
        string Convert(int number);
        string Convert(int number, Humanizer.GrammaticalGender gender);
        string ConvertToOrdinal(int number);
        string ConvertToOrdinal(int number, Humanizer.GrammaticalGender gender);
    }
}
