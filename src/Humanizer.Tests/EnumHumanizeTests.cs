using System.Diagnostics.CodeAnalysis;

public class EnumHumanizeTests
{
    [Fact]
    public void HonorsDescriptionAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithDescriptionAttribute, EnumUnderTest.MemberWithDescriptionAttribute.Humanize());

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

    [Fact]
    public void HandlesDisplayAttributeWithNoDescription() =>
        Assert.Equal(EnumTestsResources.MemberWithDisplayAttributeWithoutDescription, EnumUnderTest.MemberWithDisplayAttributeWithoutDescription.Humanize());

    [Fact]
    public void HonorsLocalizedDisplayAttribute() =>
        Assert.Equal(EnumTestsResources.MemberWithLocalizedDisplayAttribute, EnumUnderTest.MemberWithLocalizedDisplayAttribute.Humanize());

    [Fact]
    public void HumanizeCustomPropertyAttributeWithLocator()
    {
        Configurator.ResetUseEnumDescriptionPropertyLocator();
        Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
        try
        {
            Assert.Equal(EnumTestsResources.MemberWithCustomPropertyAttribute, EnumForCustomLocator.MemberWithCustomPropertyAttribute.Humanize());
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

    enum DummyEnum
    {
        First,
        Second
    }
    [Fact]
    public void Humanize_Works_For_Enum_Known_Only_At_Runtime()
    {
        Enum value = EnumUnderTest.MemberWithoutDescriptionAttribute;

        var result = value.Humanize();

        Assert.Equal(
            EnumTestsResources.MemberWithoutDescriptionAttributeSentence,
            result);
    }

}