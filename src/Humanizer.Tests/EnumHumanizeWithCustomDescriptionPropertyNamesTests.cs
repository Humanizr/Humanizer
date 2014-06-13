using System;
using Humanizer.Configuration;
using Xunit;

namespace Humanizer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
        Justification = "This is a test only class, and doesn't need a 'proper' IDisposable implementation.")]
    public class EnumHumanizeWithCustomDescriptionPropertyNamesTests : IDisposable
    {
        public EnumHumanizeWithCustomDescriptionPropertyNamesTests()
        {
            Configurator.EnumDescriptionPropertyLocator = p => p.Name == "Info";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
            Justification = "This is a test only class, and doesn't need a 'proper' IDisposable implementation.")]
        public void Dispose()
        {
            Configurator.EnumDescriptionPropertyLocator = null;
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