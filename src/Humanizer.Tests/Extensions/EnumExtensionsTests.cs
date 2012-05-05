using System.ComponentModel;
using Xunit;
using Humanize.Extensions;

namespace Humanizer.Tests.Extensions
{
    public enum EnumUnderTest
    {
        [Description(EnumExtensionsTests.CustomDescription)]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute
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
    }
}