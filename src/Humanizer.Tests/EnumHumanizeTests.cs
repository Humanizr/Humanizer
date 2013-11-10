using Xunit;

namespace Humanizer.Tests
{
    public class EnumHumanizeTests
    {
        [Fact] 
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumTestsResources.CustomDescription, EnumUnderTest.MemberWithDescriptionAttribute.Humanize());
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
    }
}