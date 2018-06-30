using Xunit;

namespace Humanizer.Tests
{
    public class EnumHumanizeTests
    {
#if !NETFX_CORE
        [Fact]
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithDescriptionAttribute, EnumUnderTest.MemberWithDescriptionAttribute.Humanize());
        }


        [Fact]
        public void HonorsDescriptionAttributeSubclasses()
        {
            Assert.Equal("Overridden " + EnumTestsResources.MemberWithDescriptionAttributeSubclass, EnumUnderTest.MemberWithDescriptionAttributeSubclass.Humanize());
        }
#endif
        [Fact]
        public void HonorsAnyAttributeWithDescriptionStringProperty()
        {
            Assert.Equal(EnumTestsResources.MemberWithCustomDescriptionAttribute, EnumUnderTest.MemberWithCustomDescriptionAttribute.Humanize());
        }

        [Fact]
        public void OnlyStringDescriptionsApply()
        {
            Assert.Equal(EnumTestsResources.MemberWithImposterDescriptionAttribute, EnumUnderTest.MemberWithImposterDescriptionAttribute.Humanize());
        }

        [Fact]
        public void CanHumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize());
        }

        [Fact]
        public void CanApplyTitleCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumTestsResources.MemberWithoutDescriptionAttributeTitle,
                EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.Title));
        }

        [Fact]
        public void CanApplyLowerCaseCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase,
                EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.LowerCase));
        }

        [Fact]
        public void AllCapitalMembersAreReturnedAsIs()
        {
            Assert.Equal(EnumUnderTest.ALLCAPITALS.ToString(), EnumUnderTest.ALLCAPITALS.Humanize());
        }

        [Fact]
        public void HonorsDisplayAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithDisplayAttribute, EnumUnderTest.MemberWithDisplayAttribute.Humanize());
        }
        [Fact]
        public void HandlesDisplayAttributeWithNoDescription()
        {
            Assert.Equal(EnumTestsResources.MemberWithDisplayAttributeWithoutDescription, EnumUnderTest.MemberWithDisplayAttributeWithoutDescription.Humanize());
        }

        [Fact]
        public void HonorsLocalizedDisplayAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithLocalizedDisplayAttribute, EnumUnderTest.MemberWithLocalizedDisplayAttribute.Humanize());
        }

    }
}
