using System.ComponentModel;

namespace Humanizer.Tests.Extensions
{
    public enum EnumUnderTest
    {
        [Description(EnumTestsResources.CustomDescription)]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute,
        ALLCAPITALS
    }
}