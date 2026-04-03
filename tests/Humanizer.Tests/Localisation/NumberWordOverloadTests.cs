namespace Humanizer.Tests.Localisation;

public class NumberWordOverloadTests
{
    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.AddAndCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberAddAndOutputs(string localeName, LocaleNumberOverloadTheoryData.OverloadExpectation[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(culture: culture, addAnd: false));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormOutputs(string localeName, LocaleNumberOverloadTheoryData.OverloadExpectation[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(WordForm.Abbreviation, culture));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.GenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberGenderOutputs(string localeName, LocaleNumberOverloadTheoryData.OverloadExpectation[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(GrammaticalGender.Feminine, culture));

    [Theory]
    [MemberData(nameof(LocaleNumberOverloadTheoryData.WordFormGenderCases), MemberType = typeof(LocaleNumberOverloadTheoryData))]
    public void UsesExpectedLargeNumberWordFormGenderOutputs(string localeName, LocaleNumberOverloadTheoryData.OverloadExpectation[] expected) =>
        AssertMatches(localeName, expected, static (number, culture) => number.ToWords(WordForm.Abbreviation, GrammaticalGender.Feminine, culture));

    static void AssertMatches(
        string localeName,
        LocaleNumberOverloadTheoryData.OverloadExpectation[] expected,
        Func<long, CultureInfo, string> formatter)
    {
        Assert.Equal(LocaleNumberOverloadTheoryData.LargeNumbers.Length, expected.Length);

        var culture = CultureInfo.GetCultureInfo(localeName);

        for (var index = 0; index < LocaleNumberOverloadTheoryData.LargeNumbers.Length; index++)
        {
            var number = LocaleNumberOverloadTheoryData.LargeNumbers[index];
            var expectedValue = expected[index];

            if (expectedValue.ExpectsException)
            {
                var exception = Record.Exception(() => formatter(number, culture));

                Assert.NotNull(exception);

                if (!expectedValue.ExpectsAnyException)
                {
                    Assert.IsType(expectedValue.ExceptionType!, exception);
                }

                continue;
            }

            Assert.Equal(expectedValue.Value, formatter(number, culture));
        }
    }
}
