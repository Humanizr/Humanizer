using System.ComponentModel;
using Xunit;

namespace Humanizer.Tests.Extensions
{
    public enum EnumUnderTest
    {
        [Description(EnumExtensionsTests.CustomDescription)]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute,
        ALLCAPITALS
    }

    public class EnumExtensionsTests
    {
        public const string CustomDescription = "Some Description";

        [Fact] 
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(CustomDescription, EnumUnderTest.MemberWithDescriptionAttribute.Humanize());
        }

        [Fact] 
        public void CanHumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal("Member without description attribute", EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize());
        }

        [Fact] 
        public void CanApplyTitleCasingOnEnumHumanization()
        {
            Assert.Equal(
                "Member Without Description Attribute", 
                EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.Title));
        }

        [Fact] 
        public void CanApplyLowerCaseCasingOnEnumHumanization()
        {
            Assert.Equal(
                "member without description attribute", 
                EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.LowerCase));
        }

        [Fact] 
        public void AllCapitalMembersAreReturnedAsIs()
        {
            Assert.Equal(EnumUnderTest.ALLCAPITALS.ToString(), EnumUnderTest.ALLCAPITALS.Humanize());
        }
    }
}