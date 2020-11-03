using Xunit;

namespace Humanizer.Tests.Localisation.bnBD
{
    [UseCulture("bn-BD")]
    public class NumberToWordsTests
    {
        [InlineData(0, "শূন্য")]
        [InlineData(1, "এক")]
        [InlineData(10, "দশ")]
        [InlineData(11, "এগারো")]
        [InlineData(20, "বিশ")]
        [InlineData(122, "একশ বাইশ")]
        [InlineData(3501, "তিন হাজার পাঁচশ এক")]
        [InlineData(100, "একশ")]
        [InlineData(1000, "এক হাজার")]
        [InlineData(100000, "এক লক্ষ")]
        [InlineData(1000000, "দশ লক্ষ")]
        [InlineData(10000000, "এক কোটি")]
        [InlineData(100000000, "দশ কোটি")]
        [InlineData(1000000000, "একশ কোটি")]
        [InlineData(111, "একশ এগারো")]
        [InlineData(1111, "এক হাজার একশ এগারো")]
        [InlineData(111111, "এক লক্ষ এগারো হাজার একশ এগারো")]
        [InlineData(1111111, "এগারো লক্ষ এগারো হাজার একশ এগারো")]
        [InlineData(11111111, "এক কোটি এগারো লক্ষ এগারো হাজার একশ এগারো")]
        [InlineData(111111111, "এগারো কোটি এগারো লক্ষ এগারো হাজার একশ এগারো")]
        [InlineData(1111111111, "একশ এগারো কোটি এগারো লক্ষ এগারো হাজার একশ এগারো")]
        [InlineData(123, "একশ তেইশ")]
        [InlineData(1234, "এক হাজার দুইশ চৌঁতিরিশ")]
        [InlineData(12345, "বারো হাজার তিনশ পঁয়তাল্লিশ")]
        [InlineData(123456, "এক লক্ষ তেইশ হাজার চারশ ছাপ্পান্ন")]
        [InlineData(1234567, "বারো লক্ষ চৌঁতিরিশ হাজার পাঁচশ সাতষট্টি")]
        [InlineData(12345678, "এক কোটি তেইশ লক্ষ পঁয়তাল্লিশ হাজার ছয়শ আটাত্তর")]
        [InlineData(123456789, "বারো কোটি চৌঁতিরিশ লক্ষ ছাপ্পান্ন হাজার সাতশ উননব্বই")]
        [InlineData(1234567890, "একশ তেইশ কোটি পঁয়তাল্লিশ লক্ষ সাতষট্টি হাজার আটশ নব্বই")]
        [InlineData(-1234567890, "ঋণাত্মক একশ তেইশ কোটি পঁয়তাল্লিশ লক্ষ সাতষট্টি হাজার আটশ নব্বই")]
        [Theory]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "শূন্য তম")]
        [InlineData(1, "প্রথম")]
        [InlineData(2, "দ্বিতীয়")]
        [InlineData(3, "তৃতীয়")]
        [InlineData(4, "চতুর্থ")]
        [InlineData(5, "পঞ্চম")]
        [InlineData(6, "ষষ্ট")]
        [InlineData(7, "সপ্তম")]
        [InlineData(8, "অষ্টম")]
        [InlineData(9, "নবম")]
        [InlineData(10, "দশম")]
        [InlineData(11, "একাদশ")]
        [InlineData(12, "দ্বাদশ")]
        [InlineData(13, "ত্রয়োদশ")]
        [InlineData(14, "চতুর্দশ")]
        [InlineData(15, "পঞ্চদশ")]
        [InlineData(16, "ষোড়শ")]
        [InlineData(17, "সপ্তদশ")]
        [InlineData(18, "অষ্টাদশ")]
        [InlineData(19, "উনিশ তম")]
        [InlineData(20, "বিশ তম")]
        [InlineData(21, "একুশ তম")]
        [InlineData(100, "শত তম")]
        [InlineData(112, "একশ বারো তম")]
        [InlineData(118, "একশ আঠারো তম")]
        [InlineData(1000, "হাজার তম")]
        [InlineData(1001, "এক হাজার এক তম")]
        [InlineData(1021, "এক হাজার একুশ তম")]
        [InlineData(10000, "দশ হাজার তম")]
        [InlineData(10121, "দশ হাজার একশ একুশ তম")]
        [InlineData(100000, "লক্ষ তম")]
        [InlineData(1000000, "দশ লক্ষ তম")]
        [InlineData(10000000, "কোটি তম")]
        public void ToOrdinalWords(int number, string words)
        {
            Assert.Equal(words, number.ToOrdinalWords());
        }
    }
}
