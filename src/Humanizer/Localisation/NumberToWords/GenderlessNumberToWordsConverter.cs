namespace Humanizer;

abstract class GenderlessNumberToWordsConverter : INumberToWordsConverter
{
    /// <inheritdoc/>
    public abstract string Convert(long number);

    public string Convert(long number, WordForm wordForm) =>
        Convert(number);

    public virtual string Convert(long number, bool addAnd) =>
        Convert(number);

    public string Convert(long number, bool addAnd, WordForm wordForm) =>
        Convert(number, wordForm);

    public virtual string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number);

    public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, addAnd, wordForm);

    public abstract string ConvertToOrdinal(int number);

    public string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        ConvertToOrdinal(number);

    public virtual string ConvertToOrdinal(int number, WordForm wordForm) =>
        ConvertToOrdinal(number);

    public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, wordForm);

    public virtual string ConvertToTuple(int number) =>
        Convert(number);
}