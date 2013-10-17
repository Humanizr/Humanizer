using System;
using Xunit;

namespace Humanizer.Tests
{
    public class DehumanizeToEnumTests
    {
        class DummyClass
        {
            
        }

        [Fact]
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.CustomDescription.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void ThrowsForNonEnums()
        {
            Assert.Throws<ArgumentException>(() => EnumTestsResources.CustomDescription.DehumanizeTo<DummyClass>());
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
                EnumTestsResources.MemberWithoutDescriptionAttributeTitle.DehumanizeTo<EnumUnderTest>());
        }

        [Fact]
        public void CanApplyLowerCaseCasingOnEnumHumanization()
        {
            Assert.Equal(
                EnumUnderTest.MemberWithoutDescriptionAttribute,
                EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase.DehumanizeTo<EnumUnderTest>());
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
