namespace Humanizer.Tests.Localisation;

public class NumberWordOverloadTests
{
    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.AddAndCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberAddAndOutputs(string localeName, long number, string expected) =>
        Assert.Equal(expected, number.ToWords(culture: CultureInfo.GetCultureInfo(localeName), addAnd: false));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormOutputs(string localeName, long number, string expected) =>
        Assert.Equal(expected, number.ToWords(WordForm.Abbreviation, CultureInfo.GetCultureInfo(localeName)));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.GenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberGenderOutputs(string localeName, long number, string expected) =>
        Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine, CultureInfo.GetCultureInfo(localeName)));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormGenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormGenderOutputs(string localeName, long number, string expected) =>
        Assert.Equal(expected, number.ToWords(WordForm.Abbreviation, GrammaticalGender.Feminine, CultureInfo.GetCultureInfo(localeName)));
}