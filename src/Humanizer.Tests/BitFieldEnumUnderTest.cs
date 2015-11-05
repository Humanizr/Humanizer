using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humanizer.Tests
{
    [Flags]
    public enum BitFieldEnumUnderTest : int
    {
        [Description(BitFlagEnumTestsResources.MemberWithSingleWordDescriptionAttribute)]
        RED = 1,
        [Description(BitFlagEnumTestsResources.MemberWithMultipleWordDescriptionAttribute)]
        DARK_GRAY = 2
    }

    public class BitFlagEnumTestsResources
    {
        public const string MemberWithSingleWordDescriptionAttribute = "Red";
        public const string MemberWithMultipleWordDescriptionAttribute = "Dark Gray";

        public const string ExpectedResultWhenBothValuesXored = "Red and Dark Gray";
    }
}
