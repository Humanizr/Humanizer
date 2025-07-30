namespace Humanizer;

abstract class GenderedNumberToWordsConverter : INumberToWordsConverter
{
    readonly GrammaticalGender _defaultGender;

    protected GenderedNumberToWordsConverter(GrammaticalGender defaultGender = GrammaticalGender.Masculine) =>
        _defaultGender = defaultGender;

    /// <inheritdoc/>
    public string Convert(long number) =>
        Convert(number, _defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, WordForm wordForm) =>
        Convert(number, wordForm, _defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, bool addAnd) =>
        Convert(number, _defaultGender);

    /// <inheritdoc/>
    public string Convert(long number, bool addAnd, WordForm wordForm) =>
        Convert(number, wordForm, _defaultGender, addAnd);

    /// <inheritdoc/>
    public abstract string Convert(long number, GrammaticalGender gender, bool addAnd = true);

    /// <inheritdoc/>
    public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, gender, addAnd);

    /// <inheritdoc/>
    public string ConvertToOrdinal(int number) =>
        ConvertToOrdinal(number, _defaultGender);

    /// <inheritdoc/>
    public abstract string ConvertToOrdinal(int number, GrammaticalGender gender);

    /// <inheritdoc/>
    public string ConvertToOrdinal(int number, WordForm wordForm) =>
        ConvertToOrdinal(number, _defaultGender, wordForm);

    /// <inheritdoc/>
    public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
        ConvertToOrdinal(number, gender);

    /// <inheritdoc/>
    public virtual string ConvertToTuple(int number) =>
        Convert(number);
}