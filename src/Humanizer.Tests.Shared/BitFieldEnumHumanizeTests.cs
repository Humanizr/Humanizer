using Xunit;

namespace Humanizer.Tests
{
    [UseCulture("en")]
    public class BitFieldEnumHumanizeTests
    {
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

        [Fact]
        public void CanHumanizeShortSingleWordDescriptionAttribute()
        {
            Assert.Equal(BitFlagEnumTestsResources.MemberWithSingleWordDisplayAttribute, ShortBitFieldEnumUnderTest.RED.Humanize());
        }

        [Fact]
        public void CanHumanizeShortMultipleWordDescriptionAttribute()
        {
            Assert.Equal(BitFlagEnumTestsResources.MemberWithMultipleWordDisplayAttribute, ShortBitFieldEnumUnderTest.DARK_GRAY.Humanize());
        }

        [Fact]
        public void CanHumanizeShortMultipleValueBitFieldEnum()
        {
            var xoredBitFlag = (ShortBitFieldEnumUnderTest.RED | ShortBitFieldEnumUnderTest.DARK_GRAY);
            Assert.Equal(BitFlagEnumTestsResources.ExpectedResultWhenBothValuesXored, xoredBitFlag.Humanize());
        }

        [Fact]
        public void CanHumanizeBitFieldEnumWithZeroValue()
        {
            Assert.Equal(BitFlagEnumTestsResources.None, BitFieldEnumUnderTest.NONE.Humanize());
        }
    }
}
