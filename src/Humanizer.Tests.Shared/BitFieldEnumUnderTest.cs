using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humanizer.Tests
{
    [Flags]
    public enum BitFieldEnumUnderTest : int
    {
        [Display( Description = BitFlagEnumTestsResources.MemberWithSingleWordDisplayAttribute)]
        RED = 1,
        [Display( Description = BitFlagEnumTestsResources.MemberWithMultipleWordDisplayAttribute)]
        DARK_GRAY = 2
    }

    [Flags]
    public enum ShortBitFieldEnumUnderTest : short
    {
        [Display( Description = BitFlagEnumTestsResources.MemberWithSingleWordDisplayAttribute)]
        RED = 1,
        [Display( Description = BitFlagEnumTestsResources.MemberWithMultipleWordDisplayAttribute)]
        DARK_GRAY = 2
    }

    public class BitFlagEnumTestsResources
    {
        public const string MemberWithSingleWordDisplayAttribute = "Red";
        public const string MemberWithMultipleWordDisplayAttribute = "Dark Gray";

        public const string ExpectedResultWhenBothValuesXored = "Red and Dark Gray";
    }
}
