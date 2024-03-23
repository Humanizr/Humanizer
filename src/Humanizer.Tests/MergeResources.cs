using System.Collections;
using System.Resources;

public class MergeResources
{
    [Fact]
    public async Task Foo()
    {

        var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var dir = @"C:\Code\ClassLibrary1\ClassLibrary1";
        foreach (var file in Directory.EnumerateFiles(dir,"*.cs"))
        {
            File.Delete(file);
        }
        var files = Directory.EnumerateFiles(@"C:\Code\Humanizer\src\Humanizer\Properties", "*.resx");
        var invarientFile = @"C:\Code\Humanizer\src\Humanizer\Properties\Resources.resx";
        var keys = await WriteInvarient(dir, invarientFile);
        await WriteClass(dir, CultureInfo.InvariantCulture, invarientFile, keys);
        foreach (var file in files)
        {
            if (file.EndsWith("Resources.resx"))
            {
                break;
            }
            var culture = Culture(file, cultureInfos);

            await WriteClass(dir, culture, file, keys);
        }
    }

    static async Task WriteClass(string dir, CultureInfo culture, string file, List<string> keys)
    {
        keys = new(keys);
        var className = GetClassName(culture);
        var combine = Path.Combine(dir, $"{className}.cs");
        using var writer = File.CreateText(combine);
        await writer.WriteLineAsync(
            $$"""
              class {{className}} : IResources
              {
                  public string Culture => "{{culture.Name}}";
                  public static {{className}} Instance => new();
              """);

        using var reader = new ResXResourceReader(file);
        foreach (DictionaryEntry entry in reader)
        {
            var key = (string) entry.Key;
            key = ScrubKey(key);
            keys.Remove(key);
            await writer.WriteLineAsync(
                $$"""
                      public string {{key}} => "{{entry.Value}}";
                  """);
        }


        if(File.Exists($@"C:\Code\Humanizer\src\Humanizer\Properties\Resources.{culture.Parent.Name}.resx"))
        {
            var parentClassName = GetClassName(culture.Parent);
            foreach (var key in keys)
            {
                await writer.WriteLineAsync(
                    $"""
                          public string {key} => {parentClassName}.Instance.{key};
                      """);
            }
        }
        else
        {
            foreach (var key in keys)
            {
                await writer.WriteLineAsync(
                    $$"""
                          public string {{key}} => InvariantResources.Instance.{{key}};
                      """);
            }
        }

        await writer.WriteLineAsync("}");
    }

    static string GetClassName(CultureInfo culture)
    {
        if (Equals(culture, CultureInfo.InvariantCulture))
        {
            return $"InvariantResources";
        }

        var name = culture.DisplayName
            .Replace(" ","")
            .Replace(",","")
            .Replace(")","")
            .Replace("(","");
        return $"{name}Resources";
    }

    static string ScrubKey(string key) =>
        key
            .Replace("DateHumanize_", "Date_")
            .Replace("TimeSpanHumanize_", "TimeSpan_")
            .Replace("_", "");

    static async Task<List<string>> WriteInvarient(string dir, string file)
    {
        var keys = new List<string>();
        var combine = Path.Combine(dir, "IResources.cs");
        using var writer = File.CreateText(combine);
        await writer.WriteLineAsync(
            $$"""
              interface IResources
              {
                  public string Culture { get; }
              """);

        using var reader = new ResXResourceReader(file);
        foreach (DictionaryEntry entry in reader)
        {
            var key = (string) entry.Key;
            key = ScrubKey(key);
            await writer.WriteLineAsync(
                $$"""
                      public string {{key}} { get; }
                  """);
            keys.Add(key);
        }

        await writer.WriteLineAsync("}");
        return keys;
    }

    static CultureInfo Culture(string file, CultureInfo[] cultureInfos)
    {
        if (file == "C:\\Code\\Humanizer\\src\\Humanizer\\Properties\\Resources.resx")
        {
            return CultureInfo.InvariantCulture;
        }
        var culture = Path
            .GetFileName(file)
            .Split('.')[1];
         return cultureInfos.Single(_ => _.Name == culture);
    }
}