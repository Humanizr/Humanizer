[MemoryDiagnoser(false)]
public class EnglishArticleBenchmarks
{
    [Benchmark]
    public string[] AppendArticlePrefix() =>
        EnglishArticle.AppendArticlePrefix(["Ant", "The Theater", "The apple", "Fox", "Bear"]);

    [Benchmark]
    public string[] PrependArticleSuffix() =>
        EnglishArticle.PrependArticleSuffix(["Ant", "apple The", "Bear", "Fox", "Theater The"]);
}