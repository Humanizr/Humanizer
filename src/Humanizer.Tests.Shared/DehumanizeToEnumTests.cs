namespace Humanizer.Tests
{
    public class DehumanizeToEnumTests
    {
        [Fact]
        public void ThrowsForEnumNoMatch()
        {
            Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<DummyEnum>());
            Assert.Throws<NoMatchFoundException>(() => EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(DummyEnum)));
        }

        [Fact]
        public void CanReturnNullForEnumNoMatch() =>
            Assert.Null(EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(DummyEnum), OnNoMatch.ReturnsNull));

        [Fact]
        public void HonorsDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttribute, EnumTestsResources.MemberWithDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void HonorsDescriptionAttributeSubclasses()
        {
            const string calculatedDescription = "Overridden " + EnumTestsResources.MemberWithDescriptionAttributeSubclass;
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithDescriptionAttributeSubclass, calculatedDescription.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void HonorsAnyAttributeWithDescriptionStringProperty()
        {
            Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithCustomDescriptionAttribute, EnumTestsResources.MemberWithCustomDescriptionAttribute.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void DehumanizeMembersWithoutDescriptionAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithoutDescriptionAttribute, EnumTestsResources.MemberWithoutDescriptionAttributeSentence.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Theory]
        [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeTitle, EnumUnderTest.MemberWithoutDescriptionAttribute)]
        [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeLowerCase, EnumUnderTest.MemberWithoutDescriptionAttribute)]
        [InlineData(EnumTestsResources.MemberWithoutDescriptionAttributeSentence, EnumUnderTest.MemberWithoutDescriptionAttribute)]
        public void IsCaseInsensitive(string input, EnumUnderTest expectedEnum)
        {
            Assert.Equal(expectedEnum, input.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(expectedEnum, input.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void AllCapitalMembersAreReturnedAsIs()
        {
            Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.ALLCAPITALS, EnumUnderTest.ALLCAPITALS.ToString().DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void HonorsDisplayAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithDisplayAttribute, EnumTestsResources.MemberWithDisplayAttribute.DehumanizeTo(typeof(EnumUnderTest)));
        }

        [Fact]
        public void HonorsLocalizedDisplayAttribute()
        {
            Assert.Equal(EnumUnderTest.MemberWithLocalizedDisplayAttribute, EnumTestsResources.MemberWithLocalizedDisplayAttribute.DehumanizeTo<EnumUnderTest>());
            Assert.Equal(EnumUnderTest.MemberWithLocalizedDisplayAttribute, EnumTestsResources.MemberWithLocalizedDisplayAttribute.DehumanizeTo(typeof(EnumUnderTest)));
        }

        enum DummyEnum
        {
            First,
            Second
        }
    }
}
