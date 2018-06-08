using System;
using Xunit;

namespace Humanizer.Tests
{
    public class ArticlePrefixSortTests
    {
        [Theory]
        [InlineData(new string[] { "Ant", "The Theater", "The apple", "Fox", "Bear" }, new string[] { "Ant", "The apple", "Bear", "Fox", "The Theater" })]
        public void SortStringArrayIgnoringArticlePrefixes(string[] input, string[] expectedOutput)
        {
            Assert.Equal(expectedOutput, EnglishArticle.PrependArticleSuffix(EnglishArticle.AppendArticlePrefix(input)));
        }

        [Fact]
        public void An_Empty_String_Array_Throws_ArgumentOutOfRangeException()
        {
            string[] items = new string[] { };
            Action action = () => EnglishArticle.AppendArticlePrefix(items);
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }
    }
}
