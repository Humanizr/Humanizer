using System;
using Humanizer.Configuration;
using Xunit;

namespace Humanizer.Tests
{
    public class EnumHumanizeWithCustomDescriptionPropertyNamesTests : IDisposable
    {
        public EnumHumanizeWithCustomDescriptionPropertyNamesTests()
        {
            Configurator.EnumDescriptionPropertyNames[typeof (EnumUnderTest)] = "Info";
        }

        public void Dispose()
        {
            Configurator.EnumDescriptionPropertyNames.Remove(typeof (EnumUnderTest));
        }

        [Fact]
        public void HonorsCustomPropertyAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithCustomPropertyAttribute, EnumUnderTest.MemberWithCustomPropertyAttribute.Humanize());
        }

        [Fact]
        public void CanHumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize());
        }
    }
}