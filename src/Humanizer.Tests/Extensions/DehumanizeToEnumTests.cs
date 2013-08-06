using Xunit;

namespace Humanizer.Tests.Extensions
{
    public class DehumanizeToEnumTests
    {
        [Fact]
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.CustomDescription.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void CanHumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void CanApplyTitleCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumUnderTest.MemberWithoutDescriptionAttribute,
                EnumTestsResources.MemberWithoutDescriptionAttributeTitle.DehumanizeTo<EnumUnderTest>(LetterCasing.Title));
        }

        [Fact]
        public void CanApplyLowerCaseCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumUnderTest.MemberWithoutDescriptionAttribute,
                EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase.DehumanizeTo<EnumUnderTest>(LetterCasing.LowerCase));
        }

        [Fact]
        public void AllCapitalMembersAreReturnedAsIs()
        {
            Assert.Equal(
                EnumUnderTest.ALLCAPITALS,
                EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo<EnumUnderTest>());
        }
    }
}
