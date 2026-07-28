using System.Diagnostics.CodeAnalysis;

public class EnumHumanizeTests
{
    [Fact]
    public void HonorsDescriptionAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithDescriptionAttribute, EnumUnderTest.MemberWithDescriptionAttribute.Humanize());

    [Theory]
    [InlineData(LetterCasing.Title)]
    [InlineData(LetterCasing.AllCaps)]
    [InlineData(LetterCasing.LowerCase)]
    [InlineData(LetterCasing.Sentence)]
    public void CasingPreservesDescriptionAttribute(LetterCasing casing) =>
        Assert.Equal(EnumTestsResources.MemberWithDescriptionAttribute, EnumUnderTest.MemberWithDescriptionAttribute.Humanize(casing));

    [Fact]
    public void InvalidCasingStillThrowsForDescriptionAttribute() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => EnumUnderTest.MemberWithDescriptionAttribute.Humanize((LetterCasing)42));

    [Fact]
    public void HonorsDescriptionAttributeSubclasses() =>
        Assert.Equal("Overridden " + EnumTestsResources.MemberWithDescriptionAttributeSubclass, EnumUnderTest.MemberWithDescriptionAttributeSubclass.Humanize());

    [Fact]
    public void HonorsAnyAttributeWithDescriptionStringProperty() =>
        Assert.Equal(EnumTestsResources.MemberWithCustomDescriptionAttribute, EnumUnderTest.MemberWithCustomDescriptionAttribute.Humanize());

    [Fact]
    public void OnlyStringDescriptionsApply() =>
        Assert.Equal(EnumTestsResources.MemberWithImposterDescriptionAttribute, EnumUnderTest.MemberWithImposterDescriptionAttribute.Humanize());

    [Fact]
    public void CanHumanizeMembersWithoutDescriptionAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize());

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void CanHumanizeWhenEnumTypeIsKnownAtRuntimeOnly()
    {
        Enum value = EnumUnderTest.MemberWithoutDescriptionAttribute;

        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, value.Humanize());
        Assert.Equal(
            EnumTestsResources.MemberWithoutDescriptionAttributeTitle,
            value.Humanize(LetterCasing.Title));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void RuntimeEnumCasingPreservesDescriptionAttribute()
    {
        Enum value = EnumUnderTest.MemberWithDescriptionAttribute;

        Assert.Equal(EnumTestsResources.MemberWithDescriptionAttribute, value.Humanize(LetterCasing.Title));
        Assert.Throws<ArgumentOutOfRangeException>(() => value.Humanize((LetterCasing)42));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void RuntimeEnumHumanizationPreservesInnerExceptionStack()
    {
        Enum value = (EnumUnderTest)int.MaxValue;

        var exception = Assert.Throws<KeyNotFoundException>(() => value.Humanize());

        Assert.Contains("Humanize[T]", exception.StackTrace);
    }

    [Fact]
    public void CanApplyTitleCasingOnEnumHumanization() =>
        Assert.Equal(
            EnumTestsResources.MemberWithoutDescriptionAttributeTitle,
            EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.Title));

    [Fact]
    public void CanApplyLowerCaseCasingOnEnumHumanization() =>
        Assert.Equal(
            EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase,
            EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.LowerCase));

    [Fact]
    public void AllCapitalMembersAreReturnedAsIs() =>
        Assert.Equal(EnumUnderTest.ALLCAPITALS.ToString(), EnumUnderTest.ALLCAPITALS.Humanize());

    [Fact]
    public void HonorsDisplayAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithDisplayAttribute, EnumUnderTest.MemberWithDisplayAttribute.Humanize());

    [Theory]
    [InlineData(LetterCasing.Title)]
    [InlineData(LetterCasing.AllCaps)]
    [InlineData(LetterCasing.LowerCase)]
    [InlineData(LetterCasing.Sentence)]
    public void CasingPreservesDisplayAttribute(LetterCasing casing) =>
        Assert.Equal(EnumTestsResources.MemberWithDisplayAttribute, EnumUnderTest.MemberWithDisplayAttribute.Humanize(casing));

    [Fact]
    public void HandlesDisplayAttributeWithNoDescription() =>
        Assert.Equal(EnumTestsResources.MemberWithDisplayAttributeWithoutDescription, EnumUnderTest.MemberWithDisplayAttributeWithoutDescription.Humanize());

    [Fact]
    public void HonorsLocalizedDisplayAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithLocalizedDisplayAttribute, EnumUnderTest.MemberWithLocalizedDisplayAttribute.Humanize());

    [Theory]
    [InlineData(EnumHumanizeSource.Default, "Localized display description")]
    [InlineData(EnumHumanizeSource.EnumName, "Raw enum name")]
    [InlineData(EnumHumanizeSource.DisplayName, "Localized display name")]
    [InlineData(EnumHumanizeSource.DisplayDescription, "Localized display description")]
    [InlineData(EnumHumanizeSource.DisplayShortName, "Localized short name")]
    public void CanSelectHumanizeSource(EnumHumanizeSource source, string expected) =>
        Assert.Equal(expected, EnumHumanizeSourcesUnderTest.RawEnumName.Humanize(LetterCasing.Sentence, source));

    [Fact]
    public void SelectedMetadataPreservesCasingWhileEnumNameHonorsIt()
    {
        var value = EnumHumanizeSourcesUnderTest.RawEnumName;

        Assert.Equal("Localized display name", value.Humanize(LetterCasing.AllCaps, EnumHumanizeSource.DisplayName));
        Assert.Equal("RAW ENUM NAME", value.Humanize(LetterCasing.AllCaps, EnumHumanizeSource.EnumName));
    }

    [Theory]
    [InlineData(EnumHumanizeSource.DisplayName)]
    [InlineData(EnumHumanizeSource.DisplayDescription)]
    [InlineData(EnumHumanizeSource.DisplayShortName)]
    public void MissingSelectedMetadataFallsBackToEnumName(EnumHumanizeSource source) =>
        Assert.Equal(
            "No display metadata",
            EnumHumanizeSourcesUnderTest.NoDisplayMetadata.Humanize(LetterCasing.Sentence, source));

    [Fact]
    public void DisplayShortNameUsesDisplayNameFallback() =>
        Assert.Equal(
            "Display name only",
            EnumHumanizeSourcesUnderTest.DisplayNameOnly.Humanize(
                LetterCasing.Sentence,
                EnumHumanizeSource.DisplayShortName));

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void RuntimeEnumCanSelectHumanizeSource()
    {
        Enum value = EnumHumanizeSourcesUnderTest.RawEnumName;

        Assert.Equal(
            "Localized short name",
            value.Humanize(LetterCasing.Sentence, EnumHumanizeSource.DisplayShortName));
        Assert.Equal("RAW ENUM NAME", value.Humanize(LetterCasing.AllCaps, EnumHumanizeSource.EnumName));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void LegacyZeroCasingCallsRemainUnambiguous()
    {
        var value = EnumUnderTest.MemberWithoutDescriptionAttribute;
        Enum runtimeValue = value;

        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, value.Humanize(default));
        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, value.Humanize(0));
        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, runtimeValue.Humanize(default));
        Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, runtimeValue.Humanize(0));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void InvalidHumanizeSourceThrows()
    {
        var source = (EnumHumanizeSource)42;
        Enum runtimeValue = EnumHumanizeSourcesUnderTest.RawEnumName;

        Assert.Throws<ArgumentOutOfRangeException>(() => EnumHumanizeSourcesUnderTest.RawEnumName.Humanize(LetterCasing.Title, source));
        Assert.Throws<ArgumentOutOfRangeException>(() => runtimeValue.Humanize(LetterCasing.Title, source));
    }

    [Fact]
    public void HumanizeCustomPropertyAttributeWithLocator()
    {
        Configurator.ResetUseEnumDescriptionPropertyLocator();
        Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
        try
        {
            Assert.Equal(EnumTestsResources.MemberWithCustomPropertyAttribute, EnumForCustomLocator.MemberWithCustomPropertyAttribute.Humanize());
            Assert.Equal(
                "Member with custom property attribute",
                EnumForCustomLocator.MemberWithCustomPropertyAttribute.Humanize(
                    LetterCasing.Sentence,
                    EnumHumanizeSource.DisplayDescription));
        }
        finally
        {
            Configurator.ResetUseEnumDescriptionPropertyLocator();
        }
    }

    [Fact]
    public void HumanizeMembersWithoutDescriptionAttributeWithLocator()
    {
        Configurator.ResetUseEnumDescriptionPropertyLocator();
        Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
        try
        {
            Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumForCustomLocator.MemberWithoutDescriptionAttribute.Humanize());
        }
        finally
        {
            Configurator.ResetUseEnumDescriptionPropertyLocator();
        }
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeThrowsForEnumNoMatch()
    {
        Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<DummyEnum>());
        Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(DummyEnum)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeCanReturnNullForEnumNoMatch() =>
        Assert.Null(EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<DummyEnum>(OnNoMatch.ReturnsNull));

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeDescriptionAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeDescriptionAttributeSubclasses()
    {
        const string calculatedDescription = "Overridden " + EnumTestsResources.MemberWithDescriptionAttributeSubclass;
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeAnyAttributeWithDescriptionStringProperty()
    {
        Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeMembersWithoutDescriptionAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Theory]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData(nameof(EnumUnderTest.MemberWithoutDescriptionAttribute), EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData("memberwithoutdescriptionattribute", EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeIsCaseInsensitive(string input, EnumUnderTest expectedEnum)
    {
        Assert.Equal(expectedEnum, input.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(expectedEnum, input.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeAllCapitalMembersAreReturnedAsIs()
    {
        Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeDisplayAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeLocalizedDisplayAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithLocalizedDisplayAttribute, EnumTestsResources.MemberWithLocalizedDisplayAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithLocalizedDisplayAttribute, EnumTestsResources.MemberWithLocalizedDisplayAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Theory]
    [InlineData(nameof(EnumAliasesUnderTest.RawEnumName))]
    [InlineData("Raw enum name")]
    [InlineData("Display name")]
    [InlineData("Display description")]
    [InlineData("Display short name")]
    [InlineData("display SHORT name")]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeRecognizesEnumAliases(string input)
    {
        Assert.Equal(EnumAliasesUnderTest.RawEnumName, input.DehumanizeTo<EnumAliasesUnderTest>());
        Assert.Equal(EnumAliasesUnderTest.RawEnumName, input.DehumanizeTo(typeof(EnumAliasesUnderTest)));
        Assert.Equal("Display description", EnumAliasesUnderTest.RawEnumName.Humanize());
    }

    [Fact]
    public void DehumanizeDoesNotTrimAliases() =>
        Assert.Null(" Display name ".DehumanizeTo<EnumAliasesUnderTest>(OnNoMatch.ReturnsNull));

    [Fact]
    public void DehumanizeRecognizesShortNameWithoutChangingHumanizeSource()
    {
        Assert.Equal(EnumShortNameAliasUnderTest.RawEnumName, "Display short name".DehumanizeTo<EnumShortNameAliasUnderTest>());
        Assert.Equal("Raw enum name", EnumShortNameAliasUnderTest.RawEnumName.Humanize());
    }

    [Fact]
    public void DehumanizeRecognizesConfiguredDescriptionAsCanonicalAlias()
    {
        Configurator.ResetUseEnumDescriptionPropertyLocator();
        Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
        try
        {
            Assert.Equal(EnumCustomAliasUnderTest.RawEnumName, "Configured description".DehumanizeTo<EnumCustomAliasUnderTest>());
            Assert.Equal("Configured description", EnumCustomAliasUnderTest.RawEnumName.Humanize());
        }
        finally
        {
            Configurator.ResetUseEnumDescriptionPropertyLocator();
        }
    }

    [Theory]
    [InlineData(nameof(BitFieldEnumUnderTest.DARK_GRAY))]
    [InlineData("dark gray")]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizePreservesFlagsAliases(string input)
    {
        Assert.Equal(BitFieldEnumUnderTest.DARK_GRAY, input.DehumanizeTo<BitFieldEnumUnderTest>());
        Assert.Equal(BitFieldEnumUnderTest.DARK_GRAY, input.DehumanizeTo(typeof(BitFieldEnumUnderTest)));
    }

    [Theory]
    [InlineData(nameof(EnumAliasCollisionUnderTest.RawName), (int)EnumAliasCollisionUnderTest.MetadataName)]
    [InlineData("Supplemental collision", (int)EnumAliasCollisionUnderTest.LaterValue)]
    [RequiresDynamicCode("The native code for the target enumeration might not be available at runtime.")]
    [RequiresUnreferencedCode("The native code for the target enumeration might not be available at runtime.")]
    public void DehumanizeAliasCollisionsUseExistingCachePrecedence(string input, int expectedValue)
    {
        var expected = (EnumAliasCollisionUnderTest)expectedValue;
        Assert.Equal(expected, input.DehumanizeTo<EnumAliasCollisionUnderTest>());
        Assert.Equal(expected, input.DehumanizeTo(typeof(EnumAliasCollisionUnderTest)));
    }

    [Theory]
    [InlineData(nameof(EnumDuplicateValueAliasesUnderTest.FirstName))]
    [InlineData(nameof(EnumDuplicateValueAliasesUnderTest.SecondName))]
    [InlineData("First display name")]
    [InlineData("Second display name")]
    public void DehumanizeRecognizesAliasesForDuplicateEnumValues(string input) =>
        Assert.Equal(EnumDuplicateValueAliasesUnderTest.FirstName, input.DehumanizeTo<EnumDuplicateValueAliasesUnderTest>());

    enum DummyEnum
    {
        First,
        Second
    }

    enum EnumAliasesUnderTest
    {
        [System.ComponentModel.Description("Configured description")]
        [System.ComponentModel.DataAnnotations.Display(
            Name = "Display name",
            Description = "Display description",
            ShortName = "Display short name")]
        RawEnumName
    }

    enum EnumHumanizeSourcesUnderTest
    {
        [System.ComponentModel.DataAnnotations.Display(
            Name = nameof(EnumHumanizeSourceResources.Name),
            Description = nameof(EnumHumanizeSourceResources.Description),
            ShortName = nameof(EnumHumanizeSourceResources.ShortName),
            ResourceType = typeof(EnumHumanizeSourceResources))]
        RawEnumName,
        NoDisplayMetadata,
        [System.ComponentModel.DataAnnotations.Display(Name = "Display name only")]
        DisplayNameOnly
    }

    public static class EnumHumanizeSourceResources
    {
        public static string Name => "Localized display name";
        public static string Description => "Localized display description";
        public static string ShortName => "Localized short name";
    }

    enum EnumShortNameAliasUnderTest
    {
        [System.ComponentModel.DataAnnotations.Display(ShortName = "Display short name")]
        RawEnumName
    }

    enum EnumCustomAliasUnderTest
    {
        [CustomProperty("Configured description")]
        RawEnumName
    }

    enum EnumAliasCollisionUnderTest
    {
        RawName = 0,
        [System.ComponentModel.Description(nameof(RawName))]
        MetadataName = 1,
        [System.ComponentModel.DataAnnotations.Display(Name = "Supplemental collision", Description = "Later canonical")]
        LaterValue = 3,
        [System.ComponentModel.DataAnnotations.Display(Name = "Supplemental collision", Description = "Earlier canonical")]
        EarlierValue = 2
    }

#pragma warning disable CA1069 // Duplicate values verify aliases for every declared enum name.
    enum EnumDuplicateValueAliasesUnderTest
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "First display name")]
        FirstName = 0,
        [System.ComponentModel.DataAnnotations.Display(Name = "Second display name")]
        SecondName = 0
    }
#pragma warning restore CA1069
}