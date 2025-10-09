namespace Humanizer;

abstract class GenderedNumberToWordsConverter(GrammaticalGender defaultGender = GrammaticalGender.Masculine) : INumberToWordsConverter
{
    readonly GrammaticalGender defaultGender = defaultGender;

    /// <inheritdoc/>
    public string Convert(long number) =>
        Convert(number, defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, WordForm wordForm) =>
        Convert(number, wordForm, defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, bool addAnd) =>
        Convert(number, defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, bool addAnd, WordForm wordForm) =>
        Convert(number, wordForm, defaultGender, addAnd);

    /// <inheritdoc/>
    public abstract string Convert(long number, GrammaticalGender gender, bool addAnd = true);

    /// <inheritdoc/>
    public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, gender, addAnd);

    /// <inheritdoc/>
    public string ConvertToOrdinal(int number) =>
        ConvertToOrdinal(number, defaultGender);

    /// <inheritdoc/>
    public abstract string ConvertToOrdinal(int number, GrammaticalGender gender);

    /// <inheritdoc/>
    public string ConvertToOrdinal(int number, WordForm wordForm) =>
        ConvertToOrdinal(number, defaultGender, wordForm);

    /// <inheritdoc/>
    public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, gender);

    /// <inheritdoc/>
    public virtual string ConvertToTuple(int number) =>
        Convert(number);
}