public class ArticlePrefixSortTests
{
    [Theory]
    [InlineData(new[] { "Ant", "The Theater", "The apple", "Fox", "Bear" }, new[] { "Ant", "The apple", "Bear", "Fox", "The Theater" })]
    [InlineData(new[] { "An Ant", "The Theater", "the apple", "a Fox", "Bear" }, new[] { "An Ant", "the apple", "Bear", "a Fox", "The Theater" })]
    [InlineData(new[] { "Ant", "A Theater", "an apple", "Fox", "Bear" }, new[] { "Ant", "an apple", "Bear", "Fox", "A Theater" })]
    [InlineData(new[] { " Ant ", " A  Theater ", "  an apple  ", " Fox", "Bear " }, new[] { "A  Theater", "an apple", "Ant", "Bear", "Fox" })]
    [InlineData(new[] { "The General Theory of Relativity" }, new[] { "The General Theory of Relativity" })]
    public void SortStringArrayIgnoringArticlePrefixes(string[] input, string[] expectedOutput) =>
        Assert.Equal(expectedOutput, EnglishArticle.PrependArticleSuffix(EnglishArticle.AppendArticlePrefix(input)));

    [Fact]
    public void An_Empty_String_Array_Throws_ArgumentOutOfRangeException()
    {
        string[] items = [];
        void action() => EnglishArticle.AppendArticlePrefix(items);
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }
}