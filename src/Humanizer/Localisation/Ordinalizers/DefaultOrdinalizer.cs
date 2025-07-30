namespace Humanizer;

class DefaultOrdinalizer : IOrdinalizer
{
    public virtual string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString);

    public virtual string Convert(int number, string numberString) =>
        numberString;

    public virtual string Convert(int number, string numberString, WordForm wordForm) =>
        Convert(number, numberString, default, wordForm);

    public virtual string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm) =>
        Convert(number, numberString, gender);
}