public class LocalizedInflectionEngineTests
{
    static readonly string[] LatinScript = ["Latn"];

    static readonly InflectionBundle Bundle = new(
        "zz",
        CardinalPluralRuleKind.EnglishLike,
        InflectionCasing.LowerTitleUpper,
        ["Latn"],
        ["news"],
        [
            Lexeme("zz.cat", "cat", "cats"),
            Lexeme("zz.person", "person", "people"),
            Lexeme("zz.attorney-general", "attorney general", "attorneys general"),
            Lexeme("zz.cafe", "café", "cafés")
        ],
        [
            new(
                "zz.forward.consonant-y",
                InflectionDirection.Forward,
                100,
                "y",
                "{stem}ies",
                reverseEnabled: true,
                requiresExistingLexeme: false)
        ]);

    static readonly InflectionBundle NoRuleBundle = new(
        "zz",
        CardinalPluralRuleKind.Other,
        InflectionCasing.LowerTitleUpper,
        ["Latn"],
        [],
        [],
        []);

    static readonly InflectionBundle ExactNumericSingletonBundle = new(
        "zz",
        CardinalPluralRuleKind.Other,
        InflectionCapability.DisplayByCategory,
        InflectionQuantitySelector.ExactNumericSingleton,
        InflectionCasing.Exact,
        ["Latn"],
        [],
        [
            new(
                "zz.unit",
                "unit",
                "units",
                ["unit"],
                ["units"],
                [
                    new(CardinalPluralCategory.One, "singleton", ["singleton"]),
                    new(CardinalPluralCategory.Other, "units", ["units"])
                ])
        ],
        []);

    [Theory]
    [InlineData((int)CardinalPluralRuleKind.French, 1_000_000, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.Portuguese, 1_000_000, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.CatalanItalian, 1_000_000, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.Spanish, 1_000_000, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.Slovenian, 2, (int)CardinalPluralCategory.Two)]
    [InlineData((int)CardinalPluralRuleKind.Maltese, 2, (int)CardinalPluralCategory.Two)]
    public void RepresentativeOperandsSelectEveryPreviouslyOmittedCategory(
        int rule,
        int quantity,
        int expected)
    {
        var category = CardinalPluralRules.Select(
            (CardinalPluralRuleKind)rule,
            quantity);

        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Theory]
    [InlineData("vi", "người")]
    [InlineData("yo", "ọmọ")]
    public void InvariantCapabilityTerminatesArbitraryNouns(
        string owner,
        string noun)
    {
        var bundle = new InflectionBundle(
            owner,
            CardinalPluralRuleKind.Other,
            InflectionCapability.Invariant,
            InflectionCasing.LowerTitleUpper,
            LatinScript,
            [],
            [],
            []);
        var input = new string(noun.ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: CardinalPluralCategory.Other);

        Assert.Equal(InflectionStatus.Invariant, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void KnownTargetFormIsInvariant()
    {
        var input = new string(['c', 'a', 't', 's']);

        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Invariant, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData("cat", "cats")]
    [InlineData("Cat", "Cats")]
    [InlineData("CAT", "CATS")]
    [InlineData("attorney general", "attorneys general")]
    [InlineData("cafe\u0301", "cafés")]
    public void ExactForwardUsesPreferredOutputAndCasing(string input, string expected)
    {
        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ExactLookupCannotBypassDeclaredScriptSafety()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [Lexeme("zz.cat", "кот", "коты")],
            []);
        var input = new string("кот".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ProductiveOutputIsDefensivelyScriptChecked()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.invalid-output",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: "{stem}ы",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false));
        var input = new string("city".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ReverseCandidateComparisonHandlesEqualUtf16LengthsWithDifferentScalarCounts()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.s",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}s",
            display: [],
            excludedSurfaces: [char.ConvertFromUtf32(0x10400)],
            reverseEnabled: true,
            requiresExistingLexeme: false));

        var result = bundle.Inflect(
            "abs",
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal("ab", result.Value);
    }

    [Fact]
    public void ProductiveForwardRuleCannotEscapeItsScriptScope()
    {
        var bundle = DualScriptRuleBundle(new InflectionRule(
            "zz.forward.nominal-latin-only",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "а",
            precedingNot: [],
            dictionaryPlural: "{stem}ы",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: true,
            requiresExistingLexeme: false,
            scripts: InflectionUnicodeScripts.Latn));
        var input = new string("машина".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProductiveReversePathsCannotEscapeTheirScriptScope(bool reverseEnabledForward)
    {
        var rule = reverseEnabledForward
            ? new InflectionRule(
                "zz.forward.nominal-latin-only",
                InflectionDirection.Forward,
                100,
                prefix: string.Empty,
                suffix: "а",
                precedingNot: [],
                dictionaryPlural: "{stem}ы",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: true,
                requiresExistingLexeme: false,
                scripts: InflectionUnicodeScripts.Latn)
            : new InflectionRule(
                "zz.reverse.nominal-latin-only",
                InflectionDirection.Reverse,
                100,
                prefix: string.Empty,
                suffix: "ы",
                precedingNot: [],
                dictionaryPlural: "{stem}а",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: false,
                requiresExistingLexeme: false,
                scripts: InflectionUnicodeScripts.Latn);
        var bundle = DualScriptRuleBundle(rule);
        var input = new string("машины".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ExactLookupPrecedesProductiveRuleScriptScope()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn", "Cyrl"],
            [],
            [Lexeme("zz.car", "машина", "машины")],
            [
                new(
                    "zz.forward.nominal-latin-only",
                    InflectionDirection.Forward,
                    100,
                    "y",
                    "{stem}ies",
                    reverseEnabled: true,
                    requiresExistingLexeme: false,
                    scripts: InflectionUnicodeScripts.Latn)
            ]);

        var result = bundle.Inflect(
            "машина",
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal("машины", result.Value);
    }

    [Fact]
    public void SharedHanScalarCanMatchMultipleFamilyScopesAndRemainAmbiguous()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Hani", "Jpan", "Kore"],
            [],
            [],
            [
                new(
                    "zz.forward.hani",
                    InflectionDirection.Forward,
                    100,
                    prefix: "中",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}甲",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false,
                    scripts: InflectionUnicodeScripts.Hani),
                new(
                    "zz.forward.jpan",
                    InflectionDirection.Forward,
                    100,
                    prefix: "中",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}乙",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false,
                    scripts: InflectionUnicodeScripts.Jpan)
            ]);
        var input = new string("中文".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void KanaNarrowsSharedOwnerToJapaneseRuleScope()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Hani", "Jpan", "Kore"],
            [],
            [],
            [
                new(
                    "zz.forward.hani",
                    InflectionDirection.Forward,
                    100,
                    prefix: "あ",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}{stem}",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false,
                    scripts: InflectionUnicodeScripts.Hani)
            ]);
        var input = new string("あい".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void OwnerScriptUnionDoesNotPermitMixedScriptProductiveInput()
    {
        var bundle = DualScriptRuleBundle(new InflectionRule(
            "zz.forward.nominal",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "а",
            precedingNot: [],
            dictionaryPlural: "{stem}ы",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false,
            scripts: InflectionUnicodeScripts.Cyrl));
        var input = new string("мaшина".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void WarmWrongRuleScriptNoMatchDoesNotAllocate()
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var bundle = DualScriptRuleBundle(new InflectionRule(
            "zz.forward.nominal-latin-only",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "а",
            precedingNot: [],
            dictionaryPlural: "{stem}ы",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: true,
            requiresExistingLexeme: false,
            scripts: InflectionUnicodeScripts.Latn));
        var input = new string("машина".ToCharArray());
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = bundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                category: null);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult result = default;
        for (var index = 0; index < iterations; index++)
        {
            result = bundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                category: null);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        GC.KeepAlive(result.Value);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
        Assert.Equal(0, allocated);
    }

    [Theory]
    [InlineData("city", (int)InflectionDirection.Forward, "cities")]
    [InlineData("cities", (int)InflectionDirection.Reverse, "city")]
    [InlineData("CITY", (int)InflectionDirection.Forward, "CITIES")]
    [InlineData("CITIES", (int)InflectionDirection.Reverse, "CITY")]
    public void ProductiveRulesAreBoundedAndRoundTrip(
        string input,
        int direction,
        string expected)
    {
        var result = Bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Reverse)]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Reverse)]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Reverse)]
    public void UnresolvedMixedCaseIsExactOnlyForEveryCasingMode(
        int casing,
        int direction)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            (InflectionCasing)casing,
            ["Latn"],
            [],
            [],
            [
                new(
                    "zz.forward.consonant-y",
                    InflectionDirection.Forward,
                    100,
                    "y",
                    "{stem}ies",
                    reverseEnabled: true,
                    requiresExistingLexeme: false)
            ]);
        var input = new string(
            ((InflectionDirection)direction == InflectionDirection.Forward
                ? "cITy"
                : "cITies").ToCharArray());

        var result = bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Forward, "cITy", "cITies")]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Reverse, "cITies", "cITy")]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Forward, "cITy", "cITies")]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Reverse, "cITies", "cITy")]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Forward, "cITy", "cITies")]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Reverse, "cITies", "cITy")]
    public void AuthoredMixedCaseExactFormsStillMatch(
        int casing,
        int direction,
        string input,
        string expected)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            (InflectionCasing)casing,
            ["Latn"],
            [],
            [Lexeme("zz.city", "cITy", "cITies")],
            []);

        var result = bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData((int)InflectionDirection.Forward, false, -1, "cat", (int)InflectionStatus.Exact, "cats")]
    [InlineData((int)InflectionDirection.Forward, false, -1, "cats", (int)InflectionStatus.Invariant, "cats")]
    [InlineData((int)InflectionDirection.Forward, false, -1, "solo", (int)InflectionStatus.Invariant, "solo")]
    [InlineData((int)InflectionDirection.Forward, true, -1, "solo", (int)InflectionStatus.Exact, "cats")]
    [InlineData((int)InflectionDirection.Forward, false, (int)CardinalPluralCategory.One, "cat", (int)InflectionStatus.Exact, "solo")]
    [InlineData((int)InflectionDirection.Forward, false, (int)CardinalPluralCategory.One, "cats", (int)InflectionStatus.Invariant, "cats")]
    [InlineData((int)InflectionDirection.Forward, false, (int)CardinalPluralCategory.One, "solo", (int)InflectionStatus.Invariant, "solo")]
    [InlineData((int)InflectionDirection.Forward, true, (int)CardinalPluralCategory.Other, "solo", (int)InflectionStatus.Exact, "cats")]
    [InlineData((int)InflectionDirection.Reverse, false, -1, "cat", (int)InflectionStatus.Invariant, "cat")]
    [InlineData((int)InflectionDirection.Reverse, false, -1, "cats", (int)InflectionStatus.Exact, "cat")]
    [InlineData((int)InflectionDirection.Reverse, false, -1, "solo", (int)InflectionStatus.Exact, "cat")]
    [InlineData((int)InflectionDirection.Reverse, true, -1, "solo", (int)InflectionStatus.Exact, "cat")]
    [InlineData((int)InflectionDirection.Reverse, false, (int)CardinalPluralCategory.One, "solo", (int)InflectionStatus.Exact, "cat")]
    public void ExactRoleDirectionFlagAndCategoryMatrix(
        int direction,
        bool allowProductive,
        int category,
        string value,
        int expectedStatus,
        string expectedValue)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.cat",
                    "cat",
                    "cats",
                    ["cat"],
                    ["cats"],
                    [
                        new(CardinalPluralCategory.One, "solo", ["solo"]),
                        new(CardinalPluralCategory.Other, "cats", ["cats"])
                    ])
            ],
            []);
        var input = new string(value.ToCharArray());

        var result = bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive,
            category < 0 ? null : (CardinalPluralCategory)category);

        Assert.Equal((InflectionStatus)expectedStatus, result.Status);
        Assert.Equal(expectedValue, result.Value);
        if (expectedValue == value)
        {
            Assert.Same(input, result.Value);
        }
    }

    public static TheoryData<decimal, string> ExactNumericSingletonDecimalCases =>
        new()
        {
            { 1m, "singleton" },
            { 1.0m, "singleton" },
            { 1.00m, "singleton" },
            { -1m, "singleton" },
            { -1.0m, "singleton" },
            { -1.00m, "singleton" },
            { 0m, "units" },
            { 2m, "units" },
            { -2m, "units" },
            { 0.5m, "units" },
            { -0.5m, "units" },
            { 1.0001m, "units" },
            { -1.0001m, "units" },
            { decimal.MinValue, "units" }
        };

    [Theory]
    [MemberData(nameof(ExactNumericSingletonDecimalCases))]
    public void ExactNumericSingletonUsesAbsoluteDecimalValueAcrossScales(
        decimal quantity,
        string expected)
    {
        var result = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void ExactNumericSingletonPreservesIntegralQuantityPaths()
    {
        var intOne = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: -1);
        var intMinimum = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: int.MinValue);
        var longOne = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: 1L);
        var longMinimum = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: long.MinValue);

        Assert.Equal("singleton", intOne.Value);
        Assert.Equal("units", intMinimum.Value);
        Assert.Equal("singleton", longOne.Value);
        Assert.Equal("units", longMinimum.Value);
    }

    public static TheoryData<double, string> ExactNumericSingletonDoubleCases =>
        new()
        {
            { 1d, "singleton" },
            { -1d, "singleton" },
            { 0d, "units" },
            { 2d, "units" },
            { -2d, "units" },
            { 0.5d, "units" },
            { -0.5d, "units" },
            { 1.0000000000000002d, "units" },
            { 0.9999999999999999d, "units" },
            { (double)decimal.MaxValue, "units" },
            { (double)decimal.MinValue, "units" },
            { double.MaxValue, "units" },
            { double.MinValue, "units" }
        };

    [Theory]
    [MemberData(nameof(ExactNumericSingletonDoubleCases))]
    public void ExactNumericSingletonPreservesFiniteDoubleIdentity(
        double quantity,
        string expected)
    {
        var result = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity);

        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void ExactNumericSingletonKeepsNonFiniteExactInput(double quantity)
    {
        var input = new string("unit".ToCharArray());
        var result = ExactNumericSingletonBundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            quantity);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    public static TheoryData<double, string> ExactDoubleCategoryCases =>
        new()
        {
            { BitConverter.Int64BitsToDouble(0x2A1A165700694830), "one" },
            { BitConverter.Int64BitsToDouble(0x0410000000000000), "few" },
            { BitConverter.Int64BitsToDouble(0x3E60000000000000), "other" },
            { double.Epsilon, "other" },
            { 1.0000000000000002d, "few" },
            { double.MaxValue, "other" }
        };

    [Theory]
    [MemberData(nameof(ExactDoubleCategoryCases))]
    public void FiniteDoubleUsesExactOperandsForBundleCategory(
        double quantity,
        string expected)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.SouthSlavic,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.unit",
                    "unit",
                    "other",
                    ["unit"],
                    ["other"],
                    [
                        new(CardinalPluralCategory.One, "one", ["one"]),
                        new(CardinalPluralCategory.Few, "few", ["few"]),
                        new(CardinalPluralCategory.Other, "other", ["other"])
                    ])
            ],
            []);

        var result = bundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void UnsupportedOperandsFailClosedWithExplicitCategory(double quantity)
    {
        var input = new string("unit".ToCharArray());
        var operands = CardinalPluralOperands.Create(quantity);

        var result = ExactNumericSingletonBundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            CardinalPluralCategory.Other,
            operands);

        Assert.False(operands.IsSupported);
        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ExactNumericSingletonDoesNotChangeUnoptedOrProductiveSelection()
    {
        var selectorDisabled = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.unit",
                    "unit",
                    "units",
                    ["unit"],
                    ["units"],
                    [
                        new(CardinalPluralCategory.One, "singleton", ["singleton"]),
                        new(CardinalPluralCategory.Other, "units", ["units"])
                    ])
            ],
            []);
        var unopted = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCapability.DisplayByCategory,
            InflectionQuantitySelector.ExactNumericSingleton,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.unit",
                    "unit",
                    "units",
                    ["unit"],
                    ["units"],
                    [new(CardinalPluralCategory.Other, "units", ["units"])])
            ],
            []);
        var productive = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCapability.DisplayByCategory,
            InflectionQuantitySelector.ExactNumericSingleton,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [],
            [
                new(
                    "zz.forward.y",
                    InflectionDirection.Forward,
                    100,
                    prefix: string.Empty,
                    suffix: "y",
                    precedingNot: [],
                    dictionaryPlural: "{stem}ies",
                    display:
                    [
                        new(CardinalPluralCategory.Other, "{stem}ies")
                    ],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);

        var selectorDisabledResult = selectorDisabled.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: 1m);
        var unoptedResult = unopted.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: 1m);
        var productiveResult = productive.Inflect(
            "city",
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: 1m);

        Assert.Equal("units", selectorDisabledResult.Value);
        Assert.Equal("units", unoptedResult.Value);
        Assert.Equal(InflectionStatus.Productive, productiveResult.Status);
        Assert.Equal("cities", productiveResult.Value);
    }

    [Fact]
    public void ExactNumericSingletonKeepsQuantityFreeAndReverseContracts()
    {
        var quantityFree = ExactNumericSingletonBundle.Inflect(
            "unit",
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);
        var reverse = ExactNumericSingletonBundle.Inflect(
            "singleton",
            InflectionDirection.Reverse,
            allowProductive: true,
            quantity: 1m);

        Assert.Equal("units", quantityFree.Value);
        Assert.Equal(InflectionStatus.Exact, reverse.Status);
        Assert.Equal("unit", reverse.Value);
    }

    [Fact]
    public void ExactNumericSingletonWaitsForUniqueLexemeRecognition()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCapability.DisplayByCategory,
            InflectionQuantitySelector.ExactNumericSingleton,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.one",
                    "shared",
                    "ones",
                    ["shared"],
                    ["ones"],
                    [
                        new(CardinalPluralCategory.One, "one", ["one"]),
                        new(CardinalPluralCategory.Other, "ones", ["ones"])
                    ]),
                new(
                    "zz.two",
                    "shared",
                    "twos",
                    ["shared"],
                    ["twos"],
                    [new(CardinalPluralCategory.Other, "twos", ["twos"])])
            ],
            []);
        var input = new string("shared".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            quantity: 1m);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void WarmExactNumericSingletonSelectionDoesNotAllocate()
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var input = new string("singleton".ToCharArray());
        var operands = CardinalPluralOperands.Create(1m);
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                CardinalPluralCategory.Other,
                operands);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult result = default;
        for (var index = 0; index < iterations; index++)
        {
            result = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                CardinalPluralCategory.Other,
                operands);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        GC.KeepAlive(result.Value);

        Assert.Equal(InflectionStatus.Invariant, result.Status);
        Assert.Same(input, result.Value);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void WarmExactNumericSingletonNumericOverloadsDoNotAllocate()
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var input = new string("unit".ToCharArray());
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1L);
            _ = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1.00m);
            _ = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1d);
        }

        var beforeLong = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult longResult = default;
        for (var index = 0; index < iterations; index++)
        {
            longResult = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1L);
        }

        var longAllocated = GC.GetAllocatedBytesForCurrentThread() - beforeLong;
        var beforeDecimal = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult decimalResult = default;
        for (var index = 0; index < iterations; index++)
        {
            decimalResult = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1.00m);
        }

        var decimalAllocated =
            GC.GetAllocatedBytesForCurrentThread() - beforeDecimal;
        var beforeDouble = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult doubleResult = default;
        for (var index = 0; index < iterations; index++)
        {
            doubleResult = ExactNumericSingletonBundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                quantity: 1d);
        }

        var doubleAllocated =
            GC.GetAllocatedBytesForCurrentThread() - beforeDouble;
        GC.KeepAlive(longResult.Value);
        GC.KeepAlive(decimalResult.Value);
        GC.KeepAlive(doubleResult.Value);

        Assert.Equal(InflectionStatus.Exact, longResult.Status);
        Assert.Equal(InflectionStatus.Exact, decimalResult.Status);
        Assert.Equal(InflectionStatus.Exact, doubleResult.Status);
        Assert.Equal(0, longAllocated);
        Assert.Equal(0, decimalAllocated);
        Assert.Equal(0, doubleAllocated);
    }

    [Fact]
    public void ReverseCollisionIsAnExplicitAmbiguousTerminal()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                Lexeme("zz.one", "one", "shared"),
                Lexeme("zz.two", "two", "shared")
            ],
            []);
        var input = new string("shared".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ProductiveReverseCollisionIsAnExplicitAmbiguousTerminal()
    {
        InflectionRule[] rules =
        [
            new(
                "zz.forward.y",
                InflectionDirection.Forward,
                100,
                "y",
                "{stem}ies",
                reverseEnabled: true,
                requiresExistingLexeme: false),
            new(
                "zz.forward.ie",
                InflectionDirection.Forward,
                90,
                "ie",
                "{stem}ies",
                reverseEnabled: true,
                requiresExistingLexeme: false)
        ];
        foreach (var orderedRules in new[] { rules, rules.Reverse().ToArray() })
        {
            var bundle = new InflectionBundle(
                "zz",
                CardinalPluralRuleKind.Other,
                InflectionCasing.LowerTitleUpper,
                ["Latn"],
                [],
                [],
                orderedRules);
            var input = new string("parties".ToCharArray());

            var result = bundle.Inflect(
                input,
                InflectionDirection.Reverse,
                allowProductive: true,
                category: null);

            Assert.Equal(InflectionStatus.Ambiguous, result.Status);
            Assert.Same(input, result.Value);
        }
    }

    [Fact]
    public void AcceptedPluralRetainsLexemeIdentityForCategoryProjection()
    {
        var input = new string("cats".ToCharArray());

        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            CardinalPluralCategory.One);

        Assert.Equal(InflectionStatus.Exact, result.Status);
        Assert.Equal("cat", result.Value);
    }

    [Fact]
    public void AcceptedTargetAlternativeRetainsExactInputReference()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.person",
                    "person",
                    "people",
                    ["person", "individual"],
                    ["people", "persons"],
                    [
                        new(CardinalPluralCategory.One, "person", ["person", "individual"]),
                        new(CardinalPluralCategory.Other, "people", ["people", "persons"])
                    ])
            ],
            []);
        (string Value, InflectionDirection Direction, bool AllowProductive, CardinalPluralCategory? Category)[] cases =
        [
            ("persons", InflectionDirection.Forward, false, null),
            ("persons", InflectionDirection.Forward, true, CardinalPluralCategory.Other),
            ("individual", InflectionDirection.Reverse, false, null)
        ];
        foreach (var testCase in cases)
        {
            var input = new string(testCase.Value.ToCharArray());
            var result = bundle.Inflect(
                input,
                testCase.Direction,
                testCase.AllowProductive,
                testCase.Category);

            Assert.Equal(InflectionStatus.Invariant, result.Status);
            Assert.Same(input, result.Value);
        }

        var sourceRole = bundle.Inflect(
            "persons",
            InflectionDirection.Forward,
            allowProductive: true,
            CardinalPluralCategory.One);
        Assert.Equal(InflectionStatus.Exact, sourceRole.Status);
        Assert.Equal("person", sourceRole.Value);
    }

    [Fact]
    public void CrossRoleExactCollisionIsAmbiguous()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                Lexeme("zz.axes", "axes", "axeses"),
                Lexeme("zz.axis", "axis", "axes")
            ],
            []);
        var input = new string("axes".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ComparerEquivalentExactSurfacesAreAmbiguous(bool reverseLexemes)
    {
        var sigma = Lexeme("zz.sigma", "σ", "σοι");
        var finalSigma = Lexeme("zz.final-sigma", "ς", "τελικά");
        var lexemes = reverseLexemes
            ? new[] { finalSigma, sigma }
            : [sigma, finalSigma];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Grek"],
            [],
            lexemes,
            []);
        var input = new string("σ".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData((int)InflectionDirection.Forward, "\uFF5A", (int)InflectionStatus.Exact)]
    [InlineData((int)InflectionDirection.Forward, "\U00010780", (int)InflectionStatus.Exact)]
    [InlineData((int)InflectionDirection.Reverse, "\uFF5A", (int)InflectionStatus.Invariant)]
    [InlineData((int)InflectionDirection.Reverse, "\U00010780", (int)InflectionStatus.Invariant)]
    public void GeneratedUnicodeOrderFindsEveryExactEntry(
        int direction,
        string input,
        int expected)
    {
        var lexemes = new[]
        {
            Lexeme("zz.fullwidth-z", "\uFF5A", "\uFF5As"),
            Lexeme("zz.supplementary-latin", "\U00010780", "\U00010780s")
        };
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            LatinScript,
            [],
            lexemes,
            []);

        var result = bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive: false,
            category: null);

        Assert.Equal((InflectionStatus)expected, result.Status);
    }

    [Theory]
    [InlineData("\u00DF", "Latn", (int)InflectionDirection.Forward)]
    [InlineData("\u1E9E", "Latn", (int)InflectionDirection.Forward)]
    [InlineData("\u00DF", "Latn", (int)InflectionDirection.Reverse)]
    [InlineData("\u1E9E", "Latn", (int)InflectionDirection.Reverse)]
    [InlineData("\u03B8", "Grek", (int)InflectionDirection.Forward)]
    [InlineData("\u03F4", "Grek", (int)InflectionDirection.Forward)]
    [InlineData("\u03B8", "Grek", (int)InflectionDirection.Reverse)]
    [InlineData("\u03F4", "Grek", (int)InflectionDirection.Reverse)]
    public void SimpleCaseEquivalentExactSurfacesFindAmbiguousEntry(
        string input,
        string script,
        int direction)
    {
        var first = script == "Latn" ? "\u00DF" : "\u03B8";
        var second = script == "Latn" ? "\u1E9E" : "\u03F4";
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            [script],
            [],
            [
                Lexeme("zz.first", first, first + "s"),
                Lexeme("zz.second", second, second + "s")
            ],
            []);

        var result = bundle.Inflect(
            input,
            (InflectionDirection)direction,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EqualPriorityPrefixRulesPreferTheLongestMatch(bool reverseRules)
    {
        var shortRule = new InflectionRule(
            "zz.forward.a-short",
            InflectionDirection.Forward,
            100,
            prefix: "a",
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}s",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var longRule = new InflectionRule(
            "zz.forward.z-long",
            InflectionDirection.Forward,
            100,
            prefix: "ab",
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}z",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var rules = reverseRules
            ? new[] { longRule, shortRule }
            : [shortRule, longRule];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [],
            rules);

        var result = bundle.Inflect(
            "abc",
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal("cz", result.Value);
    }

    [Theory]
    [InlineData("abcxc", "abc", "c", false, false, "xcprefix")]
    [InlineData("abcxc", "abc", "c", false, true, "xcprefix")]
    [InlineData("aabc", "a", "abc", false, false, "asuffix")]
    [InlineData("aabc", "a", "abc", false, true, "asuffix")]
    [InlineData("abc", "a", "c", false, false, null)]
    [InlineData("abc", "a", "c", false, true, null)]
    [InlineData("abc", "a", "c", true, false, null)]
    [InlineData("abc", "a", "c", true, true, null)]
    public void EqualPriorityCrossKindRulesUseLongestAffixThenRejectIncompatibleTie(
        string input,
        string prefix,
        string suffix,
        bool suffixIdFirst,
        bool reverseRules,
        string? expected)
    {
        var prefixRule = new InflectionRule(
            suffixIdFirst ? "zz.forward.z-prefix" : "zz.forward.a-prefix",
            InflectionDirection.Forward,
            100,
            prefix,
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}prefix",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var suffixRule = new InflectionRule(
            suffixIdFirst ? "zz.forward.a-suffix" : "zz.forward.z-suffix",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix,
            precedingNot: [],
            dictionaryPlural: "{stem}suffix",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var rules = reverseRules
            ? new[] { suffixRule, prefixRule }
            : [prefixRule, suffixRule];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [],
            rules);

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        if (expected is null)
        {
            Assert.Equal(InflectionStatus.Ambiguous, result.Status);
            Assert.Same(input, result.Value);
        }
        else
        {
            Assert.Equal(InflectionStatus.Productive, result.Status);
            Assert.Equal(expected, result.Value);
        }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void EqualRankIncompatibleRulesAreAmbiguousRegardlessOfIdOrOrder(
        bool renameIds,
        bool reverseRules)
    {
        var prefixRule = new InflectionRule(
            renameIds ? "zz.forward.z-prefix" : "zz.forward.a-prefix",
            InflectionDirection.Forward,
            100,
            prefix: "a",
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}P",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var suffixRule = new InflectionRule(
            renameIds ? "zz.forward.a-suffix" : "zz.forward.z-suffix",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "c",
            precedingNot: [],
            dictionaryPlural: "{stem}S",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var rules = reverseRules
            ? new[] { suffixRule, prefixRule }
            : [prefixRule, suffixRule];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [],
            rules);
        var input = new string("abc".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void EqualRankComparerEquivalentSameKindRulesAreAmbiguous(
        bool renameIds,
        bool reverseRules)
    {
        var sigmaRule = new InflectionRule(
            renameIds ? "zz.forward.z-sigma" : "zz.forward.a-sigma",
            InflectionDirection.Forward,
            100,
            prefix: "σ",
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}α",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var finalSigmaRule = new InflectionRule(
            renameIds ? "zz.forward.a-final-sigma" : "zz.forward.z-final-sigma",
            InflectionDirection.Forward,
            100,
            prefix: "ς",
            suffix: string.Empty,
            precedingNot: [],
            dictionaryPlural: "{stem}β",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false);
        var rules = reverseRules
            ? new[] { finalSigmaRule, sigmaRule }
            : [sigmaRule, finalSigmaRule];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Grek"],
            [],
            [],
            rules);
        var input = new string("ςα".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Ambiguous, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void NonOverlappingEqualLengthRulesRemainProductive()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [],
            [
                new(
                    "zz.forward.a",
                    InflectionDirection.Forward,
                    100,
                    prefix: "a",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}s",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false),
                new(
                    "zz.forward.b",
                    InflectionDirection.Forward,
                    100,
                    prefix: "b",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}z",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);

        var result = bundle.Inflect(
            "abc",
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal("bcs", result.Value);
    }

    [Fact]
    public void ReverseDirectionRuleExecutesDirectly()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.reverse.ies",
            InflectionDirection.Reverse,
            100,
            prefix: string.Empty,
            suffix: "ies",
            precedingNot: [],
            dictionaryPlural: "{stem}y",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false));

        var result = bundle.Inflect(
            "cities",
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal("city", result.Value);
    }

    [Fact]
    public void ReversedForwardRuleRechecksForwardGuards()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.guarded-y",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: ["a"],
            dictionaryPlural: "{stem}ies",
            display: [],
            excludedSurfaces: ["day"],
            reverseEnabled: true,
            requiresExistingLexeme: false));
        var input = new string("daies".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void ReversedForwardRuleHonorsLexemeExclusions()
    {
        ushort[] excludedLexemes = [0];
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [Lexeme("zz.city", "city", "urbanities")],
            [
                new(
                    "zz.forward.y",
                    InflectionDirection.Forward,
                    100,
                    prefix: string.Empty,
                    suffix: "y",
                    precedingNot: [],
                    dictionaryPlural: "{stem}ies",
                    display: [],
                    excludedSurfaces: [],
                    excludedLexemes,
                    reverseEnabled: true,
                    requiresExistingLexeme: false)
            ]);
        var input = new string("cities".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void MissingRuleDisplayCategoryFailsClosed()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.category",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: "{stem}ies",
            display:
            [
                new(CardinalPluralCategory.One, "{stem}y")
            ],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false));
        var input = new string("city".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            CardinalPluralCategory.Other);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void EmptyProjectedOutputFailsClosed()
    {
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.empty",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: string.Empty,
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false));
        var input = new string("City".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData(0xD800)]
    [InlineData(0xDC00)]
    public void IllFormedProjectedOutputFailsClosed(int outputCodeUnit)
    {
        var output = new string((char)outputCodeUnit, 1);
        var bundle = RuleBundle(new InflectionRule(
            "zz.forward.ill-formed",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: output,
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false));
        var input = new string("City".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void MissingLexemeDisplayCategoryFailsClosed()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn"],
            [],
            [
                new(
                    "zz.cat",
                    "cat",
                    "cats",
                    ["cat"],
                    ["cats"],
                    [new(CardinalPluralCategory.One, "cat", ["cat"])])
            ],
            []);
        var input = new string("cats".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            CardinalPluralCategory.Other);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData("cAt")]
    [InlineData("cat!")]
    [InlineData("cat dog")]
    [InlineData("cat2")]
    [InlineData("су")]
    [InlineData("news")]
    [InlineData("a\u02B9y")]
    [InlineData("a\u1DC0y")]
    public void SafetyAndSkipRejectionsAreTerminalUnsupported(string value)
    {
        var input = new string(value.ToCharArray());

        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void SafetyRejectionIsTerminalWhenProductiveRulesAreDisabled()
    {
        var input = new string("cat!".ToCharArray());

        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: false,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData("a\u02BCy", "a\u02BCies")]
    [InlineData("a\u034Fy", "a\u034Fies")]
    public void ExactScriptExtensionsAndInheritedMarksRemainEligible(
        string input,
        string expected)
    {
        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("Arab", "ا", "\u0640")]
    [InlineData("Jpan", "あ", "\u30FC")]
    public void ExactCommonScriptExtensionsRemainEligible(
        string script,
        string prefix,
        string extension)
    {
        var bundle = ScriptRuleBundle(script, prefix);

        var result = bundle.Inflect(
            prefix + extension,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal(extension + extension, result.Value);
    }

    [Fact]
    public void EligibleLexicalMissRemainsUnknown()
    {
        var input = new string("dog".ToCharArray());

        var result = Bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [InlineData("UNKNOWN", false, true, (int)InflectionDirection.Forward)]
    [InlineData("UNKNOWN", true, true, (int)InflectionDirection.Forward)]
    [InlineData("\u00C9L\u00C9PHANT", false, true, (int)InflectionDirection.Forward)]
    [InlineData("\u00C9L\u00C9PHANT", true, true, (int)InflectionDirection.Forward)]
    [InlineData("UNKNOWN", true, false, (int)InflectionDirection.Forward)]
    [InlineData("\u00C9L\u00C9PHANT", true, false, (int)InflectionDirection.Forward)]
    [InlineData("UNKNOWN", true, true, (int)InflectionDirection.Reverse)]
    [InlineData("\u00C9L\u00C9PHANT", true, true, (int)InflectionDirection.Reverse)]
    public void WarmUppercaseNoMatchDoesNotAllocate(
        string value,
        bool allowProductive,
        bool hasRules,
        int directionValue)
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var bundle = hasRules ? Bundle : NoRuleBundle;
        var direction = (InflectionDirection)directionValue;
        var input = new string(value.ToCharArray());
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = bundle.Inflect(
                input,
                direction,
                allowProductive,
                category: null);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult result = default;
        for (var index = 0; index < iterations; index++)
        {
            result = bundle.Inflect(
                input,
                direction,
                allowProductive,
                category: null);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        GC.KeepAlive(result.Value);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
        Assert.True(
            allocated == 0,
            $"{value}, productive={allowProductive}, rules={hasRules}, direction={direction}: " +
            $"{allocated / (double)iterations} B/op.");
    }

    [Theory]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.Exact, (int)InflectionDirection.Reverse)]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.LowerTitleUpper, (int)InflectionDirection.Reverse)]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Forward)]
    [InlineData((int)InflectionCasing.None, (int)InflectionDirection.Reverse)]
    public void WarmMixedCaseUnknownDoesNotAllocate(int casing, int direction)
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            (InflectionCasing)casing,
            ["Latn"],
            [],
            [],
            [
                new(
                    "zz.forward.consonant-y",
                    InflectionDirection.Forward,
                    100,
                    "y",
                    "{stem}ies",
                    reverseEnabled: true,
                    requiresExistingLexeme: false)
            ]);
        var input = new string(
            ((InflectionDirection)direction == InflectionDirection.Forward
                ? "cITy"
                : "cITies").ToCharArray());
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = bundle.Inflect(
                input,
                (InflectionDirection)direction,
                allowProductive: true,
                category: null);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult result = default;
        for (var index = 0; index < iterations; index++)
        {
            result = bundle.Inflect(
                input,
                (InflectionDirection)direction,
                allowProductive: true,
                category: null);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        GC.KeepAlive(result.Value);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void ReverseExistingLexemeRequirementUsesAcceptedSingularForms()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [Lexeme("zz.city", "city", "urbanities")],
            [
                new(
                    "zz.forward.consonant-y",
                    InflectionDirection.Forward,
                    100,
                    "y",
                    "{stem}ies",
                    reverseEnabled: true,
                    requiresExistingLexeme: true)
            ]);

        var found = bundle.Inflect(
            "cities",
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);
        var missing = bundle.Inflect(
            "babies",
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, found.Status);
        Assert.Equal("city", found.Value);
        Assert.Equal(InflectionStatus.Unknown, missing.Status);
        Assert.Equal("babies", missing.Value);
    }

    [Fact]
    public void DirectReverseExistingLexemeRequirementRejectsUnknownOutput()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [Lexeme("zz.city", "city", "urbanities")],
            [
                new(
                    "zz.reverse.consonant-y",
                    InflectionDirection.Reverse,
                    100,
                    prefix: string.Empty,
                    suffix: "ies",
                    precedingNot: [],
                    dictionaryPlural: "{stem}y",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: true)
            ]);
        var input = new string("parties".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void DirectReverseExcludedLexemeRejectsAcceptedOutput()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [Lexeme("zz.party", "party", "celebrations")],
            [
                new(
                    "zz.reverse.consonant-y",
                    InflectionDirection.Reverse,
                    100,
                    prefix: string.Empty,
                    suffix: "ies",
                    precedingNot: [],
                    dictionaryPlural: "{stem}y",
                    display: [],
                    excludedSurfaces: [],
                    excludedLexemes: new ushort[] { 0 },
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);
        var input = new string("parties".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
    }

    [Fact]
    public void DirectReverseRulesUseAuthoredRank()
    {
        InflectionRule[] rules =
        [
            new(
                "zz.reverse.consonant-y",
                InflectionDirection.Reverse,
                100,
                prefix: string.Empty,
                suffix: "ies",
                precedingNot: [],
                dictionaryPlural: "{stem}y",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: false,
                requiresExistingLexeme: false),
            new(
                "zz.reverse.fallback-s",
                InflectionDirection.Reverse,
                10,
                prefix: string.Empty,
                suffix: "s",
                precedingNot: [],
                dictionaryPlural: "{stem}",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: false,
                requiresExistingLexeme: false)
        ];
        foreach (var orderedRules in new[] { rules, rules.Reverse().ToArray() })
        {
            var bundle = new InflectionBundle(
                "zz",
                CardinalPluralRuleKind.Other,
                InflectionCasing.LowerTitleUpper,
                ["Latn"],
                [],
                [],
                orderedRules);

            var result = bundle.Inflect(
                "cities",
                InflectionDirection.Reverse,
                allowProductive: true,
                category: null);

            Assert.Equal(InflectionStatus.Productive, result.Status);
            Assert.Equal("city", result.Value);
        }
    }

    [Theory]
    [InlineData(false, (int)InflectionCountability.Count, (int)InflectionStatus.Unknown)]
    [InlineData(false, (int)InflectionCountability.Mass, (int)InflectionStatus.Productive)]
    [InlineData(true, (int)InflectionCountability.Count, (int)InflectionStatus.Unknown)]
    [InlineData(true, (int)InflectionCountability.Mass, (int)InflectionStatus.Productive)]
    public void ProductiveReversePathsHonorKnownLexemeCountability(
        bool reverseEnabledForward,
        int countabilities,
        int expectedStatus)
    {
        var rule = reverseEnabledForward
            ? new InflectionRule(
                "zz.forward.s",
                InflectionDirection.Forward,
                100,
                prefix: string.Empty,
                suffix: "s",
                precedingNot: [],
                dictionaryPlural: "{stem}ss",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: true,
                requiresExistingLexeme: true,
                countabilities: (InflectionCountability)countabilities)
            : new InflectionRule(
                "zz.reverse.s",
                InflectionDirection.Reverse,
                100,
                prefix: string.Empty,
                suffix: "s",
                precedingNot: [],
                dictionaryPlural: "{stem}",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: false,
                requiresExistingLexeme: true,
                countabilities: (InflectionCountability)countabilities);
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [Lexeme("zz.news", "news", "news", InflectionCountability.Mass)],
            [rule]);
        var input = new string("newss".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal((InflectionStatus)expectedStatus, result.Status);
        if (result.Status == InflectionStatus.Productive)
        {
            Assert.Equal("news", result.Value);
        }
        else
        {
            Assert.Same(input, result.Value);
        }
    }

    [Theory]
    [InlineData(false, (int)InflectionCountability.Count, (int)InflectionStatus.Unknown)]
    [InlineData(false, (int)InflectionCountability.All, (int)InflectionStatus.Productive)]
    [InlineData(true, (int)InflectionCountability.Count, (int)InflectionStatus.Unknown)]
    [InlineData(true, (int)InflectionCountability.All, (int)InflectionStatus.Productive)]
    public void ProductiveReversePathsFailClosedForUnknownCountability(
        bool reverseEnabledForward,
        int countabilities,
        int expectedStatus)
    {
        var rule = reverseEnabledForward
            ? new InflectionRule(
                "zz.forward.consonant-y",
                InflectionDirection.Forward,
                100,
                prefix: string.Empty,
                suffix: "y",
                precedingNot: [],
                dictionaryPlural: "{stem}ies",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: true,
                requiresExistingLexeme: false,
                countabilities: (InflectionCountability)countabilities)
            : new InflectionRule(
                "zz.reverse.consonant-y",
                InflectionDirection.Reverse,
                100,
                prefix: string.Empty,
                suffix: "ies",
                precedingNot: [],
                dictionaryPlural: "{stem}y",
                display: [],
                excludedSurfaces: [],
                reverseEnabled: false,
                requiresExistingLexeme: false,
                countabilities: (InflectionCountability)countabilities);
        var bundle = RuleBundle(rule);
        var input = new string("cities".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Reverse,
            allowProductive: true,
            category: null);

        Assert.Equal((InflectionStatus)expectedStatus, result.Status);
        if (result.Status == InflectionStatus.Productive)
        {
            Assert.Equal("city", result.Value);
        }
        else
        {
            Assert.Same(input, result.Value);
        }
    }

    [Theory]
    [InlineData((int)InflectionCountability.Count, (int)InflectionStatus.Unknown)]
    [InlineData((int)InflectionCountability.All, (int)InflectionStatus.Productive)]
    public void ProductiveForwardRulesFailClosedForUnknownCountability(
        int countabilities,
        int expectedStatus)
    {
        var bundle = RuleBundle(new(
            "zz.forward.consonant-y",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: "{stem}ies",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false,
            countabilities: (InflectionCountability)countabilities));
        var input = new string("city".ToCharArray());

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal((InflectionStatus)expectedStatus, result.Status);
        if (result.Status == InflectionStatus.Productive)
        {
            Assert.Equal("cities", result.Value);
        }
        else
        {
            Assert.Same(input, result.Value);
        }
    }

    [Fact]
    public void WarmUnknownCountabilityRuleNoMatchDoesNotAllocate()
    {
        const int warmupIterations = 10_000;
        const int iterations = 1000;
        var bundle = RuleBundle(new(
            "zz.forward.consonant-y",
            InflectionDirection.Forward,
            100,
            prefix: string.Empty,
            suffix: "y",
            precedingNot: [],
            dictionaryPlural: "{stem}ies",
            display: [],
            excludedSurfaces: [],
            reverseEnabled: false,
            requiresExistingLexeme: false,
            countabilities: InflectionCountability.Count));
        var input = new string("city".ToCharArray());
        for (var index = 0; index < warmupIterations; index++)
        {
            _ = bundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                category: null);
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        InflectionResult result = default;
        for (var index = 0; index < iterations; index++)
        {
            result = bundle.Inflect(
                input,
                InflectionDirection.Forward,
                allowProductive: true,
                category: null);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        GC.KeepAlive(result.Value);

        Assert.Equal(InflectionStatus.Unknown, result.Status);
        Assert.Same(input, result.Value);
        Assert.Equal(0, allocated);
    }

    [Fact]
    public void ForeignScriptCombiningMarkIsRejected()
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn", "Deva"],
            [],
            [],
            [
                new(
                    "zz.forward.prefix",
                    InflectionDirection.Forward,
                    100,
                    prefix: "a",
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}s",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);
        var input = new string(['a', '\u093E']);

        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, result.Status);
        Assert.Same(input, result.Value);
    }

    [Theory]
    [MemberData(nameof(DeclaredScriptSamples))]
    public void EveryDeclaredScriptSupportsARepresentativeScalar(
        string script,
        string prefix,
        string stem)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            [script],
            [],
            [],
            [
                new(
                    "zz.forward.script",
                    InflectionDirection.Forward,
                    100,
                    prefix,
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}{stem}",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);

        var result = bundle.Inflect(
            prefix + stem,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Productive, result.Status);
        Assert.Equal(stem + stem, result.Value);
    }

    [Fact]
    public void GeneratedRegistryPreservesExactCallerRegistration()
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new("en-MY"));

        Assert.Equal(
            "derived legacy 2",
            formatter.TimeSpanHumanize(TimeUnit.Second, 2));
    }

    [Theory]
    [InlineData(0xD800, -1)]
    [InlineData(0xDC00, -1)]
    [InlineData(0xD800, 'a')]
    [InlineData('a', 0xDC00)]
    public void IllFormedUtf16ReturnsTheOriginalReference(
        int firstCodeUnit,
        int secondCodeUnit)
    {
        var invalid = secondCodeUnit < 0
            ? new string((char)firstCodeUnit, 1)
            : new string([(char)firstCodeUnit, (char)secondCodeUnit]);

        var invalidResult = Bundle.Inflect(
            invalid,
            InflectionDirection.Forward,
            allowProductive: true,
            category: null);

        Assert.Equal(InflectionStatus.Unsupported, invalidResult.Status);
        Assert.Same(invalid, invalidResult.Value);
    }

    [Fact]
    public void GeneratedSupportUsesExactAcceptedNames()
    {
        Assert.True(Configurator.IsCultureSupported(new CultureInfo("fr-FR")));
        Assert.True(Configurator.IsCultureSupported(new CultureInfo("en-AL")));
        Assert.False(Configurator.IsCultureSupported(new CultureInfo("en-XX")));
    }

    [Fact]
    public void CallerCreatedRegistryKeepsCallerControlledParentFallback()
    {
        var fallback = new object();
        var french = new object();
        var registry = new LocaliserRegistry<object>(fallback);
        registry.Register("fr", french);

        Assert.Same(french, registry.ResolveForCulture(new CultureInfo("fr-FR")));
        Assert.Same(fallback, registry.ResolveForCulture(new CultureInfo("en-US")));
    }

    static InflectionLexeme Lexeme(
        string id,
        string singular,
        string plural,
        InflectionCountability countability = InflectionCountability.Count) =>
        new(
            id,
            singular,
            plural,
            [singular],
            [plural],
            [
                new(CardinalPluralCategory.One, singular, [singular]),
                new(CardinalPluralCategory.Other, plural, [plural])
            ],
            countability);

    static InflectionBundle RuleBundle(InflectionRule rule) =>
        new(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [],
            [rule]);

    static InflectionBundle DualScriptRuleBundle(InflectionRule rule) =>
        new(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn", "Cyrl"],
            [],
            [],
            [rule]);

    static InflectionBundle ScriptRuleBundle(string script, string prefix) =>
        new(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            [script],
            [],
            [],
            [
                new(
                    "zz.forward.script-extension",
                    InflectionDirection.Forward,
                    100,
                    prefix,
                    suffix: string.Empty,
                    precedingNot: [],
                    dictionaryPlural: "{stem}{stem}",
                    display: [],
                    excludedSurfaces: [],
                    reverseEnabled: false,
                    requiresExistingLexeme: false)
            ]);

    public static TheoryData<string, string, string> DeclaredScriptSamples =>
        new()
        {
            { "Arab", "ا", "ب" },
            { "Armn", "ա", "բ" },
            { "Beng", "ক", "খ" },
            { "Cyrl", "а", "б" },
            { "Deva", "क", "ख" },
            { "Ethi", "ሀ", "ሁ" },
            { "Geor", "ა", "ბ" },
            { "Grek", "α", "β" },
            { "Gujr", "ક", "ખ" },
            { "Guru", "ਕ", "ਖ" },
            { "Hani", "中", "文" },
            { "Hani", "𠀀", "𠀁" },
            { "Hebr", "א", "ב" },
            { "Jpan", "あ", "い" },
            { "Khmr", "ក", "ខ" },
            { "Knda", "ಕ", "ಖ" },
            { "Kore", "가", "나" },
            { "Laoo", "ກ", "ຂ" },
            { "Latn", "a", "b" },
            { "Mlym", "ക", "ഖ" },
            { "Mong", "ᠠ", "ᠡ" },
            { "Mymr", "က", "ခ" },
            { "Orya", "କ", "ଖ" },
            { "Sinh", "ක", "ඛ" },
            { "Taml", "க", "ங" },
            { "Telu", "క", "ఖ" },
            { "Thai", "ก", "ข" }
        };
}