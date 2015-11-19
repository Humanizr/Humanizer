using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Humanizer.Tests
{
    public class BitFieldEnumHumanizeTests : AmbientCulture
    {
        public BitFieldEnumHumanizeTests() : base("en") { }

        [Fact]
        public void CanHumanizeSingleWordDescriptionAttribute()
        {
            Assert.Equal(BitFlagEnumTestsResources.MemberWithSingleWordDisplayAttribute, BitFieldEnumUnderTest.RED.Humanize());
        }

        [Fact]
        public void CanHumanizeMultipleWordDescriptionAttribute()
        {
            Assert.Equal(BitFlagEnumTestsResources.MemberWithMultipleWordDisplayAttribute, BitFieldEnumUnderTest.DARK_GRAY.Humanize());
        }

        [Fact]
        public void CanHumanizeMultipleValueBitFieldEnum()
        {
            var xoredBitFlag = (BitFieldEnumUnderTest.RED | BitFieldEnumUnderTest.DARK_GRAY);
            Assert.Equal(BitFlagEnumTestsResources.ExpectedResultWhenBothValuesXored, xoredBitFlag.Humanize());
        }
    }
}
