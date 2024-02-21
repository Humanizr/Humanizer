[MemoryDiagnoser(false)]
public class EnglishArticleBenchmarks
{
    [Benchmark]
    public string[] AppendArticlePrefix() =>
        EnglishArticle.AppendArticlePrefix([ "Ant", "The Theater", "The apple", "Fox", "Bear"]);
}