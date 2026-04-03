namespace Humanizer.Tests.Localisation;

public class NumberWordOverloadTests
{
    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.AddAndCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberAddAndOutputs(string localeName, string[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(culture: culture, addAnd: false));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormOutputs(string localeName, string[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(WordForm.Abbreviation, culture));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.GenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberGenderOutputs(string localeName, string[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(GrammaticalGender.Feminine, culture));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormGenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormGenderOutputs(string localeName, string[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(WordForm.Abbreviation, GrammaticalGender.Feminine, culture));

    static void AssertMatches(string localeName, string[] expected, Func<long, CultureInfo, string> formatter)
    {
        Assert.Equal(LocaleNumberOverloadTheoryData.LargeNumbers.Length, expected.Length);

        var culture = CultureInfo.GetCultureInfo(localeName);

        for (var index = 0; index < LocaleNumberOverloadTheoryData.LargeNumbers.Length; index++)
        {
            var number = LocaleNumberOverloadTheoryData.LargeNumbers[index];
            var expectedValue = expected[index];

            if (expectedValue.StartsWith('!'))
            {
                var exception = Record.Exception(() => formatter(number, culture));

                Assert.NotNull(exception);
                Assert.Equal(expectedValue[1..], exception.GetType().Name);
                continue;
            }

            Assert.Equal(expectedValue, formatter(number, culture));
        }
    }
}
