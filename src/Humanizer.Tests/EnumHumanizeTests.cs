﻿namespace Humanizer.Tests;

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
    public void DehumanizeThrowsForEnumNoMatch()
    {
        Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<DummyEnum>());
        Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(DummyEnum)));
    }

    [Fact]
    public void DehumanizeCanReturnNullForEnumNoMatch() =>
        Assert.Null(EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(DummyEnum), OnNoMatch.ReturnsNull));

    [Fact]
    public void DehumanizeDescriptionAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    public void DehumanizeDescriptionAttributeSubclasses()
    {
        const string calculatedDescription = "Overridden " + EnumTestsResources.MemberWithDescriptionAttributeSubclass;
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    public void DehumanizeAnyAttributeWithDescriptionStringProperty()
    {
        Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    public void DehumanizeMembersWithoutDescriptionAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Theory]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute)]
    public void DehumanizeIsCaseInsensitive(string input, EnumUnderTest expectedEnum)
    {
        Assert.Equal(expectedEnum, input.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(expectedEnum, input.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    public void DehumanizeAllCapitalMembersAreReturnedAsIs()
    {
        Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
    public void DehumanizeDisplayAttribute()
    {
        Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo<EnumUnderTest>());
        Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo(typeof(EnumUnderTest)));
    }

    [Fact]
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
}