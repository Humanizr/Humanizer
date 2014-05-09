using Humanizer.PhoneNumber;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.PhoneNumber
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("some text", "some text")]
        [InlineData("+79091234567 (ext. 123)", "+79091234567 (ext. 123)")]
        [InlineData("++79091234567", "++79091234567")]
        public void NotPhoneNumber(string phoneNumber, string expected)
        {
            Assert.Equal(phoneNumber.FormatPhone(), expected);
        }

        [Theory]
        [InlineData("+79091234567", "+7 909 123-45-67")]
        [InlineData("89091234567", "8 909 123-45-67")]
        [InlineData("+7 909 12-34-567", "+7 909 123-45-67")]
        [InlineData("8 909 12-34-567", "8 909 123-45-67")]
        public void RussianMobilePhoneNumber(string phoneNumber, string expected)
        {
            Assert.Equal(phoneNumber.FormatPhone(), expected);
        }
    }
}