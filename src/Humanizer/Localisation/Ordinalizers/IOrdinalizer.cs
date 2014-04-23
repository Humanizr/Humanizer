using System;
namespace Humanizer.Localisation.Ordinalizers
{
    public interface IOrdinalizer
    {
        string Convert(int number, string numberString);
        string Convert(int number, string numberString, Humanizer.GrammaticalGender gender);
    }
}
