using Humanizer.Tests.Localisation;

public class LocalizedInflectionTests
{
    public static IEnumerable<object?[]> ShippedLocaleRows =>
        LocaleCoverageData.ShippedLocales.Select(static locale => new object?[] { locale });

    public static TheoryData<string, decimal, int> CardinalRuleCases => new()
    {
        { "id", 1m, (int)CardinalPluralCategory.Other },
        { "am", 0m, (int)CardinalPluralCategory.One },
        { "hy", 0m, (int)CardinalPluralCategory.One },
        { "en", 1m, (int)CardinalPluralCategory.One },
        { "en", 1.0m, (int)CardinalPluralCategory.Other },
        { "si", 0.1m, (int)CardinalPluralCategory.One },
        { "pa", 0m, (int)CardinalPluralCategory.One },
        { "af", 1.0m, (int)CardinalPluralCategory.One },
        { "da", 0.1m, (int)CardinalPluralCategory.One },
        { "is", 21m, (int)CardinalPluralCategory.One },
        { "mk", 1.1m, (int)CardinalPluralCategory.One },
        { "fil", 4m, (int)CardinalPluralCategory.Other },
        { "lv", 0m, (int)CardinalPluralCategory.Zero },
        { "he", 2m, (int)CardinalPluralCategory.Two },
        { "ro", 0m, (int)CardinalPluralCategory.Few },
        { "hr", 3m, (int)CardinalPluralCategory.Few },
        { "fr", 1_000_000m, (int)CardinalPluralCategory.Many },
        { "pt", 0m, (int)CardinalPluralCategory.One },
        { "it", 1m, (int)CardinalPluralCategory.One },
        { "es", 1m, (int)CardinalPluralCategory.One },
        { "sl", 2m, (int)CardinalPluralCategory.Two },
        { "cs", 1.2m, (int)CardinalPluralCategory.Many },
        { "pl", 5m, (int)CardinalPluralCategory.Many },
        { "be", 2m, (int)CardinalPluralCategory.Few },
        { "lt", 1.2m, (int)CardinalPluralCategory.Many },
        { "ru", 22m, (int)CardinalPluralCategory.Few },
        { "mt", 11m, (int)CardinalPluralCategory.Many },
        { "ga", 7m, (int)CardinalPluralCategory.Many },
        { "ar", 0m, (int)CardinalPluralCategory.Zero },
        { "cy", 6m, (int)CardinalPluralCategory.Many }
    };

    public static TheoryData<string, decimal, int> ReviewedDecimalRuleCases => new()
    {
        { "pa", 0.0m, (int)CardinalPluralCategory.One },
        { "pa", 1.0m, (int)CardinalPluralCategory.One },
        { "pa", 0.5m, (int)CardinalPluralCategory.Other },
        { "lv", 11.0m, (int)CardinalPluralCategory.Zero },
        { "lv", 19.0m, (int)CardinalPluralCategory.Zero },
        { "lv", 11.5m, (int)CardinalPluralCategory.Other },
        { "lv", 11.11m, (int)CardinalPluralCategory.Zero },
        { "be", 2.0m, (int)CardinalPluralCategory.Few },
        { "be", 4.0m, (int)CardinalPluralCategory.Few },
        { "be", 2.5m, (int)CardinalPluralCategory.Other },
        { "be", 5.0m, (int)CardinalPluralCategory.Many },
        { "be", 9.0m, (int)CardinalPluralCategory.Many },
        { "be", 11.0m, (int)CardinalPluralCategory.Many },
        { "be", 14.0m, (int)CardinalPluralCategory.Many },
        { "be", 5.5m, (int)CardinalPluralCategory.Other },
        { "be", 12.5m, (int)CardinalPluralCategory.Other },
        { "be", 22.0m, (int)CardinalPluralCategory.Few },
        { "lt", 2.0m, (int)CardinalPluralCategory.Few },
        { "lt", 9.0m, (int)CardinalPluralCategory.Few },
        { "lt", 12.0m, (int)CardinalPluralCategory.Other },
        { "lt", 19.0m, (int)CardinalPluralCategory.Other },
        { "lt", 22.0m, (int)CardinalPluralCategory.Few },
        { "lt", 2.5m, (int)CardinalPluralCategory.Many },
        { "lt", 12.5m, (int)CardinalPluralCategory.Many },
        { "lt", 22.5m, (int)CardinalPluralCategory.Many },
        { "mt", 3.0m, (int)CardinalPluralCategory.Few },
        { "mt", 10.0m, (int)CardinalPluralCategory.Few },
        { "mt", 3.5m, (int)CardinalPluralCategory.Other },
        { "mt", 11.0m, (int)CardinalPluralCategory.Many },
        { "mt", 19.0m, (int)CardinalPluralCategory.Many },
        { "mt", 11.5m, (int)CardinalPluralCategory.Other },
        { "ga", 3.0m, (int)CardinalPluralCategory.Few },
        { "ga", 6.0m, (int)CardinalPluralCategory.Few },
        { "ga", 3.5m, (int)CardinalPluralCategory.Other },
        { "ga", 7.0m, (int)CardinalPluralCategory.Many },
        { "ga", 10.0m, (int)CardinalPluralCategory.Many },
        { "ga", 7.5m, (int)CardinalPluralCategory.Other },
        { "ar", 3.0m, (int)CardinalPluralCategory.Few },
        { "ar", 10.0m, (int)CardinalPluralCategory.Few },
        { "ar", 3.5m, (int)CardinalPluralCategory.Other },
        { "ar", 11.0m, (int)CardinalPluralCategory.Many },
        { "ar", 99.0m, (int)CardinalPluralCategory.Many },
        { "ar", 11.5m, (int)CardinalPluralCategory.Other },
        { "ro", 2.5m, (int)CardinalPluralCategory.Few },
        { "es", 1.0m, (int)CardinalPluralCategory.One },
        { "es", 1.00m, (int)CardinalPluralCategory.One },
        { "es", 1.1m, (int)CardinalPluralCategory.Other }
    };

    public static TheoryData<decimal, decimal, decimal, int, int, decimal, decimal> CardinalOperandCases => new()
    {
        { 1m, 1m, 1m, 0, 0, 0m, 0m },
        { 1.0m, 1.0m, 1m, 1, 0, 0m, 0m },
        { 1.00m, 1.00m, 1m, 2, 0, 0m, 0m },
        { 1.30m, 1.30m, 1m, 2, 1, 30m, 3m },
        { 1.03m, 1.03m, 1m, 2, 2, 3m, 3m },
        { 1.230m, 1.230m, 1m, 3, 2, 230m, 23m },
        { -1.230m, 1.230m, 1m, 3, 2, 230m, 23m },
        { -0.0000000000000000000000000010m, 0.0000000000000000000000000010m, 0m, 28, 27, 10m, 1m }
    };

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void PluralizationFormsWorkForEveryShippedLocale(string locale)
    {
        var forms = PluralizationForms.Invariant("token");

        var success = forms.TryPluralize(1.0m, new CultureInfo(locale), out var result);

        Assert.True(success);
        Assert.Equal("token", result);
    }

    [Theory]
    [MemberData(nameof(CardinalRuleCases))]
    public void SelectsCLDRCardinalCategories(
        string locale,
        decimal quantity,
        int expected)
    {
        var success = LocalizedInflectionCatalog.TrySelectCategory(
            new CultureInfo(locale),
            quantity,
            out var category);

        Assert.True(success);
        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Theory]
    [MemberData(nameof(ReviewedDecimalRuleCases))]
    public void AppliesReviewedCLDRDecimalRuleSemantics(
        string locale,
        decimal quantity,
        int expected)
    {
        var success = LocalizedInflectionCatalog.TrySelectCategory(
            new CultureInfo(locale),
            quantity,
            out var category);

        Assert.True(success);
        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Theory]
    [MemberData(nameof(CardinalOperandCases))]
    public void PreservesCLDRDecimalOperands(
        decimal quantity,
        decimal expectedN,
        decimal expectedI,
        int expectedV,
        int expectedW,
        decimal expectedF,
        decimal expectedT)
    {
        var operands = CardinalPluralOperands.Create(quantity);

        Assert.Equal(expectedN, operands.N);
        Assert.Equal(expectedI, operands.I);
        Assert.Equal(expectedV, operands.V);
        Assert.Equal(expectedW, operands.W);
        Assert.Equal(expectedF, operands.F);
        Assert.Equal(expectedT, operands.T);
    }

    [Theory]
    [InlineData("-79228162514264337593543950335")]
    [InlineData("79228162514264337593543950335")]
    public void SupportsTheFullDecimalIntegerRange(string quantity)
    {
        var operands = CardinalPluralOperands.Create(
            decimal.Parse(quantity, CultureInfo.InvariantCulture));

        Assert.Equal(decimal.MaxValue, operands.N);
        Assert.Equal(decimal.MaxValue, operands.I);
        Assert.Equal(0, operands.V);
        Assert.Equal(0, operands.W);
        Assert.Equal(0m, operands.F);
        Assert.Equal(0m, operands.T);
    }

    [Fact]
    public void DecimalScaleSuppliesVisibleFractionOperands()
    {
        var forms = new PluralizationForms(
            "item",
            "other",
            one: "one");
        var culture = new CultureInfo("en");

        Assert.True(forms.TryPluralize(1m, culture, out var integer));
        Assert.True(forms.TryPluralize(1.0m, culture, out var visibleFraction));
        Assert.Equal("one", integer);
        Assert.Equal("other", visibleFraction);
    }

    [Theory]
    [InlineData("en", "1")]
    [InlineData("ar", "0")]
    [InlineData("ar", "2")]
    [InlineData("ar", "3")]
    [InlineData("ar", "11")]
    public void MissingCallerFormsReturnFalse(
        string locale,
        string quantity)
    {
        var forms = new PluralizationForms("item", "items");

        var success = forms.TryPluralize(
            decimal.Parse(quantity, CultureInfo.InvariantCulture),
            new CultureInfo(locale),
            out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Theory]
    [InlineData("ar", "0", "items")]
    [InlineData("ar", "1", "item")]
    [InlineData("ar", "2", "items")]
    [InlineData("ar", "3", "items")]
    [InlineData("ar", "11", "items")]
    public void ExplicitEqualCallerFormsRemainAvailable(
        string locale,
        string quantity,
        string expected)
    {
        var forms = new PluralizationForms(
            "item",
            "items",
            zero: "items",
            one: "item",
            two: "items",
            few: "items",
            many: "items");

        var success = forms.TryPluralize(
            decimal.Parse(quantity, CultureInfo.InvariantCulture),
            new CultureInfo(locale),
            out var result);

        Assert.True(success);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("pt", "0", 2)]
    [InlineData("pt", "0.5", 2)]
    [InlineData("pt-PT", "0", 0)]
    [InlineData("pt-PT", "0.5", 0)]
    [InlineData("pt-PT", "1.0", 0)]
    [InlineData("pt-PT", "1", 2)]
    [InlineData("pt-PT", "1000000", 5)]
    public void EuropeanPortugueseUsesItsRegionalCLDRCardinalRule(
        string locale,
        string quantity,
        int expected)
    {
        var success = LocalizedInflectionCatalog.TrySelectCategory(
            new CultureInfo(locale),
            decimal.Parse(quantity, CultureInfo.InvariantCulture),
            out var category);

        Assert.True(success);
        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Theory]
    [InlineData("child")]
    [InlineData("children")]
    [InlineData("zero children")]
    [InlineData("one child")]
    [InlineData("two children")]
    [InlineData("few children")]
    [InlineData("many children")]
    public void TrySingularizeAcceptsEveryAuthoredForm(string form)
    {
        var forms = new PluralizationForms(
            "child",
            "children",
            zero: "zero children",
            one: "one child",
            two: "two children",
            few: "few children",
            many: "many children");

        Assert.True(forms.TrySingularize(form, out var singular));
        Assert.Equal("child", singular);
    }

    [Fact]
    public void TrySingularizeUsesNfcOrdinalCaseSensitiveMatching()
    {
        var forms = new PluralizationForms("café", "cafés");

        Assert.True(forms.TrySingularize("cafe\u0301s", out var singular));
        Assert.Equal("café", singular);
        Assert.False(forms.TrySingularize("CAFÉS", out singular));
        Assert.Null(singular);
        Assert.False(forms.TrySingularize("coffee", out singular));
        Assert.Null(singular);
    }

    [Fact]
    public void UnsupportedCultureDoesNotFallBackToEnglish()
    {
        var culture = new CultureInfo("eo");
        var forms = PluralizationForms.Invariant("token");

        Assert.False(forms.TryPluralize(1m, culture, out var authoredResult));
        Assert.Null(authoredResult);
    }

    [Fact]
    public void InvariantCultureIsUnsupported()
    {
        var forms = PluralizationForms.Invariant("token");

        Assert.False(forms.TryPluralize(1m, CultureInfo.InvariantCulture, out var authoredResult));
        Assert.Null(authoredResult);
    }

    [Fact]
    [UseCulture("en")]
    public void ExistingEnglishInflectionAndQuantityContractsAreUnchanged()
    {
        Assert.Equal("people", "person".Pluralize());
        Assert.Equal("person", "people".Singularize());
        Assert.Equal("2 people", "person".ToQuantity(2));
    }

    [Fact]
    public void ConstructorRejectsMissingRequiredForms()
    {
        Assert.Throws<ArgumentNullException>(() => new PluralizationForms(null!, "items"));
        Assert.Throws<ArgumentException>(() => new PluralizationForms("item", " "));
        Assert.Throws<ArgumentException>(() => new PluralizationForms("item", "items", one: ""));
    }

    [Fact]
    public void PublicOperationsRejectNullInputs()
    {
        string text = null!;

        Assert.Throws<ArgumentNullException>(() => PluralizationForms.Invariant(null!));
        Assert.Throws<ArgumentNullException>(() => PluralizationForms.Invariant("item").TryPluralize(1m, null!, out _));
        Assert.Throws<ArgumentNullException>(() => PluralizationForms.Invariant("item").TrySingularize(text, out _));
    }

    [Fact]
    public void TryGetFormRejectsUnknownCategory()
    {
        var forms = PluralizationForms.Invariant("item");

        Assert.Throws<ArgumentOutOfRangeException>(
            () => forms.TryGetForm((CardinalPluralCategory)99, out _));
    }
}