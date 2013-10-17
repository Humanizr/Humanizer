using System.ComponentModel;

namespace Humanizer.Tests
{
    public enum EnumUnderTest
    {
        [Description(EnumTestsResources.CustomDescription)]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute,
        ALLCAPITALS
    }
}