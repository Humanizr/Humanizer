public partial class LocalizedInflectionEngineTests
{
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
            var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        var bundle = CreateBundle(
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
        CreateBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.LowerTitleUpper,
            ["Latn"],
            [],
            [],
            [rule]);

    static InflectionBundle DualScriptRuleBundle(InflectionRule rule) =>
        CreateBundle(
            "zz",
            CardinalPluralRuleKind.Other,
            InflectionCasing.Exact,
            ["Latn", "Cyrl"],
            [],
            [],
            [rule]);

    static InflectionBundle ScriptRuleBundle(string script, string prefix) =>
        CreateBundle(
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