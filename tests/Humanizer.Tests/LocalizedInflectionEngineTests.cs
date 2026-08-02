public partial class LocalizedInflectionEngineTests
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

    [Theory]
    [InlineData("\u1C8A", "\u1C8A\u1C8A")]
    [InlineData("\u1C89", "\u1C89\u1C89")]
    public void Unicode16ExactForwardCasingIsRuntimeIndependent(
        string input,
        string expected)
    {
        var bundle = new InflectionBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Cyrl"],
            [],
            [Lexeme("zz.unicode16", "\u1C8A", "\u1C8A\u1C8A")],
            []);

        var result = bundle.Inflect(
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

}